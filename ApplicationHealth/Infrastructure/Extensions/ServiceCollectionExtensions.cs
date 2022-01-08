using ApplicationHealth.Domain;
using ApplicationHealth.Infrastructure.Context;
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

            //services.AddScoped<IPowerPlantDefRepository, PowerPlantDefRepository>();
            //services.AddScoped<IPowerPlantHourlyDatumRepository, PowerPlantHaurlyDatumReposiytory>();

            return services;
        }
        public static IServiceCollection AddEntityServices(this IServiceCollection services)
        {
            //services.AddScoped<IPowerPlantDefService, PowerPlantDefManager>();
            //services.AddScoped<IPowerPlantDatumService, PowerPlantDatumManager>();
            return services;
        }
    }
}
