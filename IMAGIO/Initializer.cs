using AutoMapper;
using IMAGIO.Entities;
using IMAGIO.Interfaces;
using IMAGIO.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using NuGet.ContentModel;

namespace IMAGIO
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
