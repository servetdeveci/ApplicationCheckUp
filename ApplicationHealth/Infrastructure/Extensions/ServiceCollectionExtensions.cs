using ApplicationHealth.Domain;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Infrastructure.Context;
using ApplicationHealth.Services.Managers;
using ApplicationHealth.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApplicationHealth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionstring = configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionstring);
            }
            );

            services.AddScoped<Func<AppDbContext>>((provider) => () => provider.GetService<AppDbContext>());
            services.AddScoped<AppDbFactory>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            }
            );

            services.AddScoped<Func<AppDbContext>>((provider) => () => provider.GetService<AppDbContext>());
            services.AddScoped<AppDbFactory>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));

            services.AddScoped<IAppDefRepository, AppDefRepository>();
            services.AddScoped<IAppNotificationRepository, AppNotificationRepository>();
            services.AddScoped<IAppContactRepository, AppContactRepository>();

            return services;
        }
        public static IServiceCollection AddEntityServices(this IServiceCollection services)
        {
            services.AddScoped<IAppDefService, AppDefManager>();
            services.AddScoped<IAppNotificationService, AppNotificationManager>();
            services.AddScoped<IAppContactService, AppContactManager>();
            services.AddScoped<IMailService, MailManager>();

            return services;
        }
    }
}
