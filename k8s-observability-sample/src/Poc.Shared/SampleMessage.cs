namespace Poc.Shared;
public class SampleMessage
{
    public DateTime Timestamp {get;set;}
    public string? ModuleVersion { get;set; } = "1.0";
    public string? Medium {get;set;}
    public double Angle {get;set;}
    public int RandomInt {get;set;}

}