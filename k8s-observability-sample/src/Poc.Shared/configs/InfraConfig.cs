 namespace Poc.Shared.Configs
{

    public class InfraConfig
    {
        public string? MqttUrl { get; set; } = "localhost";
        public string? Topic { get; set; }
        public override string ToString()
        {
            return $"Config: ( mqttUrl: {MqttUrl},  topic: {Topic}) ";
        }
    }



}