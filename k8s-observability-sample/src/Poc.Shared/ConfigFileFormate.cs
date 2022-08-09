namespace Poc.Shared
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

    public class AppConfig
    {
        public int FrequencySeconds { get; set; }
        public string? Medium { get; set; }
        public override string ToString()
        {
            return $"Config: ( medium: {Medium},  frequencySeconds: {FrequencySeconds}) ";
        }
    }

}