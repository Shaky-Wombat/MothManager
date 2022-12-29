namespace MothManager.Core.DeviceControl;

public abstract class DiscoveredDeviceInfoBase
{
    public string Id { get; }
    public string DeviceName { get; }
        
    protected DiscoveredDeviceInfoBase(string id, string deviceName)
    {
        Id = id;
        DeviceName = deviceName;
    }

    public override string ToString()
    {
        return $"[{Id}] {DeviceName}";
    }
}