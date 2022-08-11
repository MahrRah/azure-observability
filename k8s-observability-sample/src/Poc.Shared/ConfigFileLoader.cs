using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Poc.Shared.Configs;

namespace Poc.Shared
{

    public class ConfigLoader
    {

        public static InfraConfig LoadInfraConfig()
        {
            var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
            InfraConfig infraConfig = deserializer.Deserialize<InfraConfig>(File.ReadAllText(@"./configuration/infraconfig.yaml"));
            if (infraConfig == null)
            {
                throw new InvalidCastException("./configuration/infraconfig.yaml - could not deserialize");
            }
            return infraConfig;
        }

        public static AppConfig LoadAppConfig()
        {
            var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
            AppConfig appConfig = deserializer.Deserialize<AppConfig>(File.ReadAllText(@"./configuration/appconfig.yaml"));
            if (appConfig == null)
            {
                throw new InvalidCastException("./configuration/appconfig.yaml  - could not deserialize");
            }
            return appConfig;
        }
    }
}