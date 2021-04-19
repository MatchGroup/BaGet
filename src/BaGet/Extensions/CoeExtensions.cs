using BaGet.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BaGet.Extensions
{
    public static class CoeExtensions
    {
        public static void AddContainerMappingsForCoeSecrets(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, CoeApiKeyAuthenticationService>();
        }

        public static void UseCoeSecrets(this IApplicationBuilder app, IConfiguration configuration, BaGetOptions options)
        {
            if (app is null || configuration is null || options is null || string.IsNullOrWhiteSpace(options.ApiKey) == false)
                return;

            options.ApiKey = CoeApiKeyAuthenticationService.GetSecret()
                .GetAwaiter()
                .GetResult();

            Console.WriteLine($"Key found in GetSecret()");
        }
    }
}
