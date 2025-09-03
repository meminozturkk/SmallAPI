using SmallAPI.Models;
using SmallAPI.Models.DTOs;
using SmallAPI.Repositories.Interfaces;
using SmallAPI.Services.Interfaces;

namespace SmallAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordResetRepository = passwordResetRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Email kontrolü
                if (await _userRepository.EmailExistsAsync(registerDto.Email))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Bu email adresi zaten kullanılmaktadır."
                    };
                }

                // Şifreyi hashle
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Yeni kullanıcı oluştur
                var user = new User
                {
                    Email = registerDto.Email,
                    PasswordHash = hashedPassword,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    FcmToken = registerDto.FcmToken,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow,
                   
                };

                // Kullanıcıyı kaydet
                var savedUser = await _userRepository.AddAsync(user);

                // JWT token oluştur
                var token = _jwtService.GenerateToken(savedUser);

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Message = "Kayıt işlemi başarılı.",
                    User = new UserDto
                    {
                        Id = savedUser.Id,
                        Email = savedUser.Email,
                        FirstName = savedUser.FirstName,
                        LastName = savedUser.LastName,
                        IsAdmin = savedUser.IsAdmin,
                        CreatedAt = savedUser.CreatedAt,
                    }
                };
            }
            catch (Exception)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Kayıt işlemi sırasında bir hata oluştu."
                };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Kullanıcıyı email ile bul
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email veya şifre hatalı."
                    };
                }

                // Şifre kontrolü
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email veya şifre hatalı."
                    };
                }

                // JWT token oluştur
                var token = _jwtService.GenerateToken(user);

                user.FcmToken = loginDto.FcmToken; // FCM token'ı güncelle (farklı cihazda oturum açabilir)
                await _userRepository.UpdateAsync(user); // Kullanıcıyı güncelle

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Message = "Giriş işlemi başarılı.",
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsAdmin = user.IsAdmin,
                        CreatedAt = user.CreatedAt,
                       
                    }
                };
            }
            catch (Exception)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Giriş işlemi sırasında bir hata oluştu."
                };
            }
        }

        public async Task<AuthResponseDto> ResetPassword(ResetDto resetDto)
        {
            User? user = _userRepository.GetByEmailAsync(resetDto.Email).Result;

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            PasswordReset? passwordReset = _passwordResetRepository.FindAsync(pr => pr.Email == resetDto.Email && pr.RequestToken == resetDto.RequestToken && pr.IsUsed).Result.FirstOrDefault();

            if (passwordReset == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Geçersiz veya kullanılmış şifre sıfırlama isteği."
                };
            }

            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword);
                user.PasswordHash = hashedPassword;

                await _userRepository.UpdateAsync(user);
                await _passwordResetRepository.UpdateAsync(passwordReset);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Şifre başarıyla sıfırlandı."
                };
            }
            catch (Exception)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Şifre sıfırlama sırasında bir hata oluştu."
                };
            }

        }
    }
} 