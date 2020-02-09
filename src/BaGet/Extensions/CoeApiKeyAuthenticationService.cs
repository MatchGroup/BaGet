using BaGet.Core;
using Coe.Secrets;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace BaGet.Extensions
{
    public class CoeApiKeyAuthenticationService : IAuthenticationService
    {
        private const string CONFIG_PREFIX = "COE_SECRETS_";

        private readonly ISecretsManagementService _sms;
        private readonly IConfiguration _configuration;
        private readonly static SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
        private static bool _checked;
        private static string _apiKey;

        public CoeApiKeyAuthenticationService(ISecretsManagementService sms, IConfiguration configuration)
        {
            _sms = sms;
            _configuration = configuration;
        }

        public async Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken)
        {
            if (_checked == false)
            {
                await _locker.WaitAsync();
                try
                {
                    if (_checked == false)
                    {
                        var path = _configuration.GetValue(CONFIG_PREFIX + "PATH", "shared/nuget");
                        var key = _configuration.GetValue(CONFIG_PREFIX + "KEY", "key");
                        _apiKey = await _sms.GetString(path, key, shouldThrowWhenMissing: true);
                        _checked = true;
                    }
                }
                finally { _locker.Release(); }
            }

            return (_apiKey is null)
                ? true
                : (_apiKey == apiKey);
        }
    }
}
