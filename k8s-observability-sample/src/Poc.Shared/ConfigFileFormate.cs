public class InfraConfig
{
    public string? MqttUrl {get;set;} = "localhost";
    public string? Topic {get;set;}
}

public class AppConfig
{
    public int FrequencySeconds {get;set;}
    public string? Medium {get;set;}
}
