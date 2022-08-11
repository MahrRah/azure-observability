// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using Poc.Shared;
using Poc.Shared.Configs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;


internal class Program
{
    private static async Task Main(string[] args)
    {
        // EventHub settings
        String eventhubConnectionString = Environment.GetEnvironmentVariable("EVENTHUB_CONNECTION_STRING") ?? throw new ArgumentException("EVENTHUB_CONNECTION_STRING is not defined");
        String eventHubName = Environment.GetEnvironmentVariable("EVENTHUB_NAME") ?? throw new ArgumentException("EVENTHUB_NAME is not defined");


        var mqttFactory = new MqttFactory();

        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            InfraConfig config = ConfigLoader.LoadInfraConfig();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(config.MqttUrl)
                .Build();

            // Setup message handling before connecting so that queued messages
            // are also handled properly. When there is no event handler attached all
            // received messages get lost.
            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                Console.WriteLine($"Received application message {e.ApplicationMessage.ConvertPayloadToString()}");
                await using (var eventHubProducerClient = new EventHubProducerClient(eventhubConnectionString, eventHubName))
                {
                    using (EventDataBatch eventBatch = await eventHubProducerClient.CreateBatchAsync())
                    {

                        eventBatch.TryAdd(new EventData(new BinaryData(e.ApplicationMessage.Payload)));
                        await eventHubProducerClient.SendAsync(eventBatch);

                    };
                }
            };

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(config.Topic); })
                .Build();

            var response = mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            Console.WriteLine("MQTT client subscribed to topic.");

            while (true)
            {
                Task.WaitAll(response);

            }
        }



    }
}



///
///  Doe not seem to work 
//         async Task<MqttClientSubscribeResult> GetMqttSubClientAsync(Func<MqttApplicationMessageReceivedEventArgs, Task> lambda)
//          {

//             MqttFactory mqttFactory = new MqttFactory();


//             using (IMqttClient mqttClient = mqttFactory.CreateMqttClient())
//             {
//                 InfraConfig config = ConfigLoader.LoadInfraConfig();
//                 var mqttClientOptions = new MqttClientOptionsBuilder()
//                     .WithTcpServer(config.MqttUrl)
//                     .Build();

//                 // Setup message handling before connecting so that queued messages
//                 // are also handled properly. When there is no event handler attached all
//                 // received messages get lost.
//                 mqttClient.ApplicationMessageReceivedAsync += async (e) =>
//                     {
//                         Console.WriteLine($"Received application message {e.ApplicationMessage.ConvertPayloadToString()}");
//                         SampleMessage sampleMessage = SampleMessage.FromByteArray(e.ApplicationMessage.Payload);

//                         //create own service method
//                         await using (var producer = new EventHubProducerClient(eventhubConnectionString, eventHubName))
//                         {
//                             var message = e.ApplicationMessage.ConvertPayloadToString();
//                             using EventDataBatch eventBatch = await producer.CreateBatchAsync();
//                             eventBatch.TryAdd(new EventData(new BinaryData(message)));
//                             await producer.SendAsync(eventBatch);
//                         }
//                     }
// ;

//                 await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
//                 var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
//                     .WithTopicFilter(f => { f.WithTopic(config.Topic); })
//                     .Build();

//                 var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
//                 Console.WriteLine("MQTT client subscribed to topic.");
//                 return response;

//             }
//         }
//     }
// }