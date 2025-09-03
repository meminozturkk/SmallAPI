using Microsoft.EntityFrameworkCore;
using SmallAPI.Data;
using SmallAPI.Repositories;
using SmallAPI.Repositories.Interfaces;

using SmallAPI.Services.Email;
using SmallAPI.Services.Interfaces;
using SendGrid;

namespace SmallAPI.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Entity Framework konfigürasyonu
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repository servisleri
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
           


            // Business servisleri
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
           
            services.AddScoped<IEmailService, EmailService>();

            // SendGrid(Email) konfigürasyonu
            services.AddSingleton<ISendGridClient>(x =>
              new SendGridClient(configuration["SendGrid:ApiKey"]));

       
          
            return services;
        }
    }
}
