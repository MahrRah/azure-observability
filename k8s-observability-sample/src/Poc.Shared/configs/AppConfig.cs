namespace Poc.Shared.Configs
{
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