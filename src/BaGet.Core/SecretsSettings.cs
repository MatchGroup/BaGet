using Coe.Secrets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BaGet.Core
{
    public class SecretsSettings : SecretsSettingsBase
    {
        public SecretsSettings(IConfiguration configuration) : base(configuration)
        {
        }
        public override string RoleName => "nuget";
        public override string JsonConverterSerializeObject(object obj) => JsonConvert.SerializeObject(obj);
    }
}
