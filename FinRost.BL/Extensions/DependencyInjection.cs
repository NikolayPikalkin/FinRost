using FinRost.BL.Infrastructure;
using FinRost.BL.Services;
using FinRost.BL.Services.Auth;
using FinRost.DAL.Mappings;
using FinRost.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinRost.DAL.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceConfig(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddSingleton<AesCriptoService>();
            services.AddSingleton<TelegramBotService>();

            services.AddAutoMapper(typeof(AppMappingProfile));

            services.AddScoped<LoginService>();
            services.AddScoped<UserService>();
            services.AddScoped<ChatUserService>();
            services.AddScoped<LogService>();
            services.AddScoped<InvestorService>();
            services.AddScoped<OrderService>();
            services.AddScoped<LotService>();
            services.AddScoped<NotificationService>();

            return services;
        }
    }
}
