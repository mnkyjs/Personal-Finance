using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Personal.Finance.Application.Interface;
using Personal.Finance.Application.Tools;

namespace Personal.Finance.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors();
        }

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Root", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Authorize", policy => policy.RequireRole("Administrator", "User"));
            });
        }
    }
}
