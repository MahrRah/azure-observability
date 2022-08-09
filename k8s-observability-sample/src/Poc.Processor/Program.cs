// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using Poc.Shared;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;


internal class Program
{
    private static void Main(string[] args)
    {
        // EventHub settings
        var eventhubConnectionString = Environment.GetEnvironmentVariable("EVENTHUB_CONNECTION_STRING") ?? throw new ArgumentException("EVENTHUB_CONNECTION_STRING is not defined");
        var eventHubName = Environment.GetEnvironmentVariable("EVENTHUB_NAME") ?? throw new ArgumentException("EVENTHUB_NAME is not defined");


        // var mqttFactory = new MqttFactory();
        var response = await getMqttSubClient(
        async e =>
                    {
                        Console.WriteLine($"Received application message {e.ApplicationMessage.ConvertPayloadToString()}");
                        SampleMessage sampleMessage = SampleMessage.FromByteArray(e.ApplicationMessage.Payload);

                        //create own service method
                        await using (var producer = new EventHubProducerClient(eventhubConnectionString, eventHubName))
                        {
                            var message = e.ApplicationMessage.ConvertPayloadToString();
                            using EventDataBatch eventBatch = await producer.CreateBatchAsync();
                            eventBatch.TryAdd(new EventData(new BinaryData(message)));
                            await producer.SendAsync(eventBatch);
                        }
                    }

        );
        while (true)
        {
            Task.WaitAll(response);

        }

        // using (var mqttClient = mqttFactory.CreateMqttClient())
        // {
        //     InfraConfig config = ConfigLoader.LoadInfraConfig();
        //     var mqttClientOptions = new MqttClientOptionsBuilder()
        //         .WithTcpServer(config.MqttUrl)
        //         .Build();

        //     // Setup message handling before connecting so that queued messages
        //     // are also handled properly. When there is no event handler attached all
        //     // received messages get lost.
        //     mqttClient.ApplicationMessageReceivedAsync += async e =>
        //     {
        //         Console.WriteLine($"Received application message {e.ApplicationMessage.ConvertPayloadToString()}");
        //         SampleMessage sampleMessage = SampleMessage.FromByteArray(e.ApplicationMessage.Payload);

        //         //create own service method
        //         await using (var producer = new EventHubProducerClient(eventhubConnectionString, eventHubName))
        //         {
        //             var message = e.ApplicationMessage.ConvertPayloadToString();
        //             using EventDataBatch eventBatch = await producer.CreateBatchAsync();
        //             eventBatch.TryAdd(new EventData(new BinaryData(message)));
        //             await producer.SendAsync(eventBatch);
        //         }
        //     };

        //     await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        //     var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
        //         .WithTopicFilter(f => { f.WithTopic(config.Topic); })
        //         .Build();

        //     var response = mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        //     Console.WriteLine("MQTT client subscribed to topic.");

        // while (true)
        // {
        //     Task.WaitAll(response);

        // }

        // async e =>
        //         {
        //             Console.WriteLine($"Received application message {e.ApplicationMessage.ConvertPayloadToString()}");
        //             SampleMessage sampleMessage = SampleMessage.FromByteArray(e.ApplicationMessage.Payload);

        //             //create own service method
        //             await using (var producer = new EventHubProducerClient(eventhubConnectionString, eventHubName))
        //             {
        //                 var message = e.ApplicationMessage.ConvertPayloadToString();
        //                 using EventDataBatch eventBatch = await producer.CreateBatchAsync();
        //                 eventBatch.TryAdd(new EventData(new BinaryData(message)));
        //                 await producer.SendAsync(eventBatch);
        //             }
        //         };
        // }

        async Task<MqttClientSubscribeResult>? getMqttSubClient(Func<MqttApplicationMessageReceivedEventArgs, Task> lambda)
        {

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
                mqttClient.ApplicationMessageReceivedAsync += lambda;

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => { f.WithTopic(config.Topic); })
                    .Build();

                var response = mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                Console.WriteLine("MQTT client subscribed to topic.");
                return  response;

            }
        }
    }
}