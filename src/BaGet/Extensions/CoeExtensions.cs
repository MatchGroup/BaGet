using BaGet.Core;
using Coe.Secrets;
using Microsoft.Extensions.DependencyInjection;

namespace BaGet.Extensions
{
    public static class CoeExtensions
    {
        public static void AddContainerMappingsForCoeSecrets<
            Overridden_SecretsSettingsBase
            >(this IServiceCollection services)
            where Overridden_SecretsSettingsBase : SecretsSettingsBase
        {
            services.AddSingleton<ISecretsSettings, Overridden_SecretsSettingsBase>();
            services.AddSingleton<ISecretsManagementService, SecretsManagementService>();
            services.AddTransient<IAuthenticationService, CoeApiKeyAuthenticationService>();
        }
    }
}
