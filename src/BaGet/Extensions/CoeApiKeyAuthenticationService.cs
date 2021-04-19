using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using BaGet.Core;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BaGet.Extensions
{
    public class CoeApiKeyAuthenticationService : IAuthenticationService
    {
        private readonly static Regex _secretCleaner = new Regex("\\{|\"|:|}|ApiKey");

        private readonly static SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
        private static bool _checked;
        private static string _apiKey;

        public async Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken)
        {
            if (_checked == false)
            {
                await _locker.WaitAsync();
                try
                {
                    if (_checked == false)
                    {
                        _apiKey = await GetSecret();
                        _checked = true;
                    }
                }
                finally { _locker.Release(); }
            }

            return (_apiKey is null)
                ? true
                : (_apiKey == apiKey);
        }

        public static async Task<string> GetSecret()
        {
            const string secretName = "nuget-api-key";
            const string region = "us-east-1";

            var request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified
            };

            using (IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region)))
            {
                var response = await client.GetSecretValueAsync(request);

                string secret = null;

                if (response.SecretString is object)
                {
                    secret = response.SecretString;
                }
                else
                {
                    using (var memoryStream = response.SecretBinary)
                    using (var reader = new StreamReader(memoryStream))
                        secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }

                return _secretCleaner.Replace(secret, "");
            }
        }
    }
}
