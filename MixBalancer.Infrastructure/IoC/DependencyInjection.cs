using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MixBalancer.Application.Services;
using MixBalancer.Application.Services.Players;
using MixBalancer.Domain.Interfaces;
using MixBalancer.Infrastructure.Context;
using MixBalancer.Infrastructure.Repositories;
using System.Text;

namespace MixBalancer.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            // HttpClient configurado
            services.AddHttpClient("GamerClubApi", client =>
            {
                client.BaseAddress = new Uri(configuration["GamerClubApi:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // JWT Configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
                    };
                });

            return services;
        }

        // Application services
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Configurar serviços de aplicação
            services.AddScoped<AuthService>();
            services.AddScoped<IPlayerManagementService, PlayerManagementService>();

            return services;
        }

        // Configurações do banco
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MixBalancerContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}