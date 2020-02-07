using BaGet.Core;
using Coe.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BaGet.Extensions
{
    public static class CoeExtensions
    {
        private const string CONFIG_PREFIX = "COE_SECRETS_";

        public static void AddContainerMappingsForCoeSecrets<
            Overridden_SecretsSettingsBase
            >(this IServiceCollection services)
            where Overridden_SecretsSettingsBase : SecretsSettingsBase
        {
            services.AddSingleton<ISecretsSettings, Overridden_SecretsSettingsBase>();
            services.AddSingleton<ISecretsManagementService, SecretsManagementService>();
        }

        public static void UseCoeSecrets(this IApplicationBuilder app, IConfiguration configuration, BaGetOptions options)
        {
            if (app is null || configuration is null || options is null || string.IsNullOrWhiteSpace(options.ApiKey) == false)
                return;

            var path = configuration.GetValue(CONFIG_PREFIX + "PATH", "shared/nuget");
            var key = configuration.GetValue(CONFIG_PREFIX + "KEY", "key");
            options.ApiKey = app.ApplicationServices.GetService<ISecretsManagementService>()
                .GetString(path, key, shouldThrowWhenMissing: true)
                .GetAwaiter()
                .GetResult();

            Console.WriteLine($"Key found in {nameof(ISecretsManagementService)}");
        }
    }
}
