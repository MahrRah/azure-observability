using System.Diagnostics;
using System.Diagnostics.Metrics;

using Microsoft.Extensions.Logging;

using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;

namespace Poc.Shared.Observability;
public class TelemetryExporter
{
    public String serviceName;
    public String serviceVersion;

    public TelemetryExporter(String serviceName, String serviceVersion)
    {
        this.serviceName = serviceName;
        this.serviceVersion = serviceVersion;

    }


    public Counter<T> getCounter<T>(String counterType) where T : struct, IComparable
    {

        Meter MyMeter = new(serviceName, serviceVersion);
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
                    .AddMeter(serviceName)
                    .AddOtlpExporter()
                    .Build();

        return MyMeter.CreateCounter<T>(counterType);


    }

    public ILogger getLogger<T>()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .SetMinimumLevel(
                            (LogLevel)Enum.Parse(typeof(LogLevel),
                                                        "Information",
                                                        true)) // TODO: load from config
                        .AddOpenTelemetry(options =>
                        {
                            options.IncludeFormattedMessage = true;
                            options.IncludeScopes = true;
                            options.ParseStateValues = true;
                            options.AddOtlpExporter();
                        }
                                    );
                });

        return loggerFactory.CreateLogger<T>();
    }

    ActivitySource getTracer(string serviceName)
    {

        var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .AddOtlpExporter(opt =>
            {
                opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                System.Console.WriteLine($"OTLP Exporter is using {opt.Protocol} protocol and endpoint {opt.Endpoint}");
            }
            )
            .AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion)).Build();
        ;
        return new ActivitySource(serviceName);


    }
}