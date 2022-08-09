using System.Text.Json;
namespace Poc.Shared;
public class SampleMessage
{
    public DateTime Timestamp { get; set; }
    public string? ModuleVersion { get; set; } = "1.0";
    public string? Medium { get; set; }
    public double Angle { get; set; }
    public int RandomInt { get; set; }
    public static SampleMessage FromByteArray(byte[] data)
    {
        Console.WriteLine(data);
        using MemoryStream ms = new MemoryStream(data);
        SampleMessage message = JsonSerializer.Deserialize<SampleMessage>(ms) ?? throw new ArgumentException("Could not be deserielized");
        return message;
    }

}