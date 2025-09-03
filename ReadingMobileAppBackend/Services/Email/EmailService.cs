using SmallAPI.Models;
using SmallAPI.Repositories.Interfaces;
using SmallAPI.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SmallAPI.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(ISendGridClient sendGridClient, IConfiguration configuration, IPasswordResetRepository passwordResetRepository, IUserRepository userRepository)
        {
            _sendGridClient = sendGridClient;
            _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@yourapp.com";
            _fromName = configuration["SendGrid:FromName"] ?? "Reading App";
            _passwordResetRepository = passwordResetRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> SendPasswordResetCodeAsync(string toEmail)
        {
            try
            {
                User? user = await _userRepository.GetByEmailAsync(toEmail);

                // Eğer kullanıcı bulunamazsa veya e-posta adresi geçerli değilse, false döndür
                if (user == null)
                {
                    return false;
                }

                // Eğer kullanıcı zaten şifre sıfırlama kodu talep ettiyse ve bu kod hala geçerliyse, false döndür
                var doesExist = await _passwordResetRepository.FindAsync(pr => pr.Email == toEmail && !pr.IsUsed && pr.ExpiresAt > DateTime.UtcNow);

                if (doesExist.Any())
                {
                    return false;
                }

                var resetCode = new Random().Next(100000, 999999).ToString();

                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(toEmail);
                var subject = "Şifre Sıfırlama";
                var plainTextContent = $"Şifre sıfırlama kodunuz: {resetCode}";
                var htmlContent = $@"
                    <h2>Şifre Sıfırlama</h2>
                    <p>Şifre sıfırlama kodunuz:</p>
                    <h3 style='color: #007bff; font-family: monospace;'>{resetCode}</h3>
                    <p>Bu kod 10 dakika içinde geçersiz olacaktır.</p>
                    <p>If you didn't request this, please ignore this email.</p>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    PasswordReset passwordReset = new()
                    {
                        Email = toEmail,
                        Code = int.Parse(resetCode),
                        ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                        IsUsed = false
                    };

                    await _passwordResetRepository.AddAsync(passwordReset);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Guid?> ConfirmResetCode(string userEmail, int code)
        {
            var passwordReset = await _passwordResetRepository.FindAsync(pr => pr.Email == userEmail && pr.Code == code && !pr.IsUsed && pr.ExpiresAt > DateTime.UtcNow);
            if (passwordReset.Any())
            {
                var resetEntry = passwordReset.First();
                resetEntry.IsUsed = true;
                resetEntry.RequestToken = Guid.NewGuid();
                await _passwordResetRepository.UpdateAsync(resetEntry);

                return resetEntry.RequestToken;
            }

            return null;
        }
    }
}