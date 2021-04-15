using System;
using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable AssignNullToNotNullAttribute

namespace Infrastructure
{
    public static class InfrastructureInjection
    {
        private static string GetHerokuConnectionString()
        {
            // Get the connection string from the ENV variables
            // postgres://{user}:{password}@{hostname}:{port}/{database-name}
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // parse the connection string
            var dbUri = new Uri(connUrl);

            var db = dbUri.LocalPath.TrimStart('/');
            var userInfo = dbUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return
                $"User ID={userInfo[0]};Password={userInfo[1]};Host={dbUri.Host};Port={dbUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }

        private static void SetupDatabase(
            IServiceCollection s,
            IConfiguration conf,
            IWebHostEnvironment env)
        {
            if (env.IsProduction()) // heroku provides postgree sql db
            {
                var conn = GetHerokuConnectionString();
                s.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(conn,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            else
            {
                s.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(conf.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SimpleWebAppDb"));
            }
            else
            {
                SetupDatabase(services, configuration, webHostEnvironment);
            }

            services
                .AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthentication();

            return services;
        }
    }
}
