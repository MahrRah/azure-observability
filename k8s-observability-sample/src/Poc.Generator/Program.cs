// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using Poc.Shared;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

AppConfig appConfig = new();
InfraConfig infraConfig = new();
MqttClient? mqttClient = null;
MqttFactory mqttFactory = new MqttFactory();
string version = "1.0"; //TODO load version from debug/build?

//there will be two config files: ./configuration/appconfig.yaml and ./configuration/infraconfig.yaml

LoadAppConfig();
LoadInfraConfig();
//TODO setup file change handler to pickup changes in the folder

FileSystemWatcher watcher = new FileSystemWatcher();
watcher.Path = @"./configuration";
Console.WriteLine($"File watcher set to: {watcher.Path} ");

watcher.NotifyFilter = NotifyFilters.Attributes | 
                        NotifyFilters.CreationTime |
                        NotifyFilters.LastWrite |
                        NotifyFilters.Size;
//for now only 

watcher.Changed += OnConfigChanged;
watcher.Deleted += OnConfigChanged;
watcher.Created += OnConfigChanged;
watcher.Error += OnError;

//temporarily disable this, due to Symlink issue with .NET and configmaps. 
//Use hack to reload appconfig.yaml for demo in our while loop
watcher.IncludeSubdirectories = false;
watcher.Filter = "*.yaml";
//watcher.EnableRaisingEvents = true;

LoadAppConfig();
LoadInfraConfig();

// await ConnectMqttAsync();
await SendMessagesAsync();


async Task ConnectMqttAsync()
{
    //connect to MQTT broker
    if(mqttClient != null && mqttClient.IsConnected)
    {
        await mqttClient.DisconnectAsync();
    }

    mqttClient = (MqttClient)mqttFactory.CreateMqttClient();

    var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(infraConfig.MqttUrl)
            .Build();

    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
    Console.WriteLine("Connected to MQTT");

}

async Task SendMessagesAsync()
{
    int counter = 0;
    if (mqttClient == null)
    {
        await ConnectMqttAsync();
    }

    while (true)
    {
        await Task.Delay(appConfig.FrequencySeconds * 1000);
        counter++;

        var message = new SampleMessage
        {
            Timestamp = DateTime.Now.ToUniversalTime(),
            ModuleVersion = version,
            Medium = appConfig.Medium,
            Angle = new Random().NextDouble(),
            RandomInt = new Random().Next(0, 99)
        };

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(infraConfig.Topic)
            .WithPayload(JsonSerializer.Serialize(message))
            .Build();

        //connection may be in the process of resetting, skip for the next iteration
        if (mqttClient != null && mqttClient.IsConnected)
        {
            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Console.WriteLine($"Message sent at {message.Timestamp.ToString()}, in topic {infraConfig.Topic}.");
        }
        else
        {
            Console.WriteLine($"mqttClient either null or not connected, skipping");
        }

        if(counter % 10 == 0)
        {
            //hack to load the file contents by polling 
            //for infraconfig it just reloads and does not reconnect, so it will simply change the Topic messages are sent to
            LoadAppConfig();
            LoadInfraConfig();
        }
        
    }

}



async void OnConfigChanged(object sender, FileSystemEventArgs e)
{
    if (e.ChangeType != WatcherChangeTypes.Changed)
    {
        return;
    }
    string originalFileName = e.FullPath;
    Console.WriteLine($"File change detected {originalFileName}");
    if(originalFileName.Contains("appconfig.yaml"))
        LoadAppConfig();
    else if(originalFileName.Contains("infraconfig.yaml"))
    {
        InfraConfig current = new InfraConfig {MqttUrl = infraConfig.MqttUrl, Topic = infraConfig.Topic };
        LoadInfraConfig();
        //only fire a reconnection if there is a change in config:
        if(current.MqttUrl != current.Topic)
        {
            Console.WriteLine($"Config changed, reconnecting MQTT");
            await ConnectMqttAsync();
        }
        
    }
}

void OnError (object sender, ErrorEventArgs e)
{
    Console.WriteLine("OnError fired");
    if(e !=null)
    {
        Console.WriteLine($"Message: {e.GetException().Message}");
    }
}


void LoadAppConfig()
{
    var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
    appConfig = deserializer.Deserialize<AppConfig>(File.ReadAllText(@"./configuration/appconfig.yaml"));
    Console.WriteLine($"AppConfig check, Medium= {appConfig.Medium}");
    if (appConfig == null)
    {
        throw new InvalidCastException("./configuration/appconfig.yaml  - could not deserialize");
    }
}

void LoadInfraConfig()
{
    var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
    infraConfig = deserializer.Deserialize<InfraConfig>(File.ReadAllText(@"./configuration/infraconfig.yaml"));
    if (infraConfig == null)
    {
        throw new InvalidCastException("./configuration/infraconfig.yaml - could not deserialize");
    }
}

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





