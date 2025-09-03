using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmallAPI.Models.DTOs;
using SmallAPI.Services.Email;
using SmallAPI.Services.Interfaces;
using WebAPI.Controllers;

namespace SmallAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Authentication")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        /// <param name="registerDto">Kayıt bilgileri</param>
        /// <returns>Kayıt sonucu ve JWT token</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(typeof(AuthResponseDto), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri</param>
        /// <returns>Giriş sonucu ve JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(typeof(AuthResponseDto), 401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        /// <summary>
        /// Kullanıcı şifresini sıfırlamak için kod gönderme
        /// </summary>
        [HttpPost("sendresetcode")]
        [ProducesResponseType(typeof(EmailResponseDto), 200)]
        [ProducesResponseType(typeof(EmailResponseDto), 401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendResetCode(string email)
        {
            var success = await _emailService.SendPasswordResetCodeAsync(email);

            if (success)
            {
                EmailResponseDto response = new()
                {
                    Success = true,
                    Message = "Şifre sıfırlama kodu e-posta adresinize gönderildi."
                };

                return Ok(response);
            }
            else
            {
                EmailResponseDto response = new()
                {
                    Success = false,
                    Message = "E-posta gönderilirken bir hata oluştu. Lütfen tekrar deneyin."
                };

                return BadRequest(response);
            }
        }

        /// <summary>
        /// Şifre sıfırlama kodunu doğrulama
        /// </summary>
        [HttpPost("confirmresetcode")]
        [ProducesResponseType(typeof(EmailResponseDto), 200)]
        public async Task<IActionResult> ConfirmResetCode([FromBody] ConfirmResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid? result = await _emailService.ConfirmResetCode(resetPasswordDto.Email, resetPasswordDto.ResetCode);

            if (result.HasValue)
            {
                var success = new EmailResponseDto
                {
                    Success = true,
                    Message = "Şifre sıfırlama kodu doğrulandı. Yeni şifre belirleyebilirsiniz.",
                    RequestToken = result.Value.ToString()
                };

                return Ok(success);
            }
            else
            {
                return BadRequest(new EmailResponseDto
                {
                    Success = false,
                    Message = "Şifre sıfırlama kodu geçersiz veya süresi dolmuş."
                });
            }
        }

        /// <summary>
        /// Kullanıcı yeni şifre belirleme
        /// </summary>
        [HttpPost("resetpassword")]
        [ProducesResponseType(typeof(EmailResponseDto), 200)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetDto resetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ResetPassword(resetDto);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
} 