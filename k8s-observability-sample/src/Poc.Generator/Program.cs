// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using Poc.Shared;
using Poc.Shared.Configs;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string version = "1.0"; //TODO load version from debug/build?

        //there will be two config files: ./configuration/appconfig.yaml and ./configuration/infraconfig.yaml

        InfraConfig infraConfig = ConfigLoader.LoadInfraConfig();
        AppConfig appConfig = ConfigLoader.LoadAppConfig();

        MqttClient? mqttClient = null;
        MqttFactory mqttFactory = new MqttFactory();
        
        await SendMessagesAsync();


        async Task ConnectMqttAsync()
        {
            //connect to MQTT broker
            if (mqttClient != null && mqttClient.IsConnected)
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

                if (counter % 10 == 0)
                {
                    // Hack to load the file contents by polling 
                    // for infraconfig it just reloads and does not reconnect, so it will simply change the Topic messages are sent to
                    infraConfig = ConfigLoader.LoadInfraConfig();
                    appConfig = ConfigLoader.LoadAppConfig();
                }

            }

        }

    }
}
