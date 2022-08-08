// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using Poc.Shared;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


// EventHub settings
// LoadInfraConfig() Use configs fiels instead of env variables
var mqttUrl = System.Environment.GetEnvironmentVariable("MQTT_URL");
var mqttTopic = System.Environment.GetEnvironmentVariable("MQTT_TOPIC");
var eventhubConnectionString = System.Environment.GetEnvironmentVariable("EVENTHUB_CONNECTION_STRING");
var eventHubName = System.Environment.GetEnvironmentVariable("EVENTHUB_NAME");


var mqttFactory = new MqttFactory();


using (var mqttClient = mqttFactory.CreateMqttClient())
{
    var mqttClientOptions = new MqttClientOptionsBuilder()
        .WithTcpServer(mqttUrl)
        .Build();

    // Setup message handling before connecting so that queued messages
    // are also handled properly. When there is no event handler attached all
    // received messages get lost.
    mqttClient.ApplicationMessageReceivedAsync += async e =>
    {
        Console.WriteLine("Received application message.");
        Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());

        await using (var producer = new EventHubProducerClient(eventhubConnectionString, eventHubName))
        {
            var message = e.ApplicationMessage.ConvertPayloadToString();
            using EventDataBatch eventBatch = await producer.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(new BinaryData(message)));
            await producer.SendAsync(eventBatch);
            Console.WriteLine("Message was send to Eventhub.");
        }
    };

    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(f => { f.WithTopic(mqttTopic); })
        .Build();

    var response = mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
    Console.WriteLine("MQTT client subscribed to topic.");

    while (true)
    {
        Task.WaitAll(response);

    }

}

// Move this to shared libary
InfraConfig LoadInfraConfig()
{
    var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
    // assemblyOfThatType.GetType(fullName)
    InfraConfig infraConfig = deserializer.Deserialize<InfraConfig>(File.ReadAllText(@"./configuration/infraconfig.yaml"));
    if (infraConfig == null)
    {
        throw new InvalidCastException("./configuration/infraconfig.yaml - could not deserialize");
    }
    return infraConfig;
}





