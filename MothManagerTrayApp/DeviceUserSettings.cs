using MothManager.Core.DeviceControl;
using MothManager.NeewerLEDControl;

namespace MothManagerTrayApp;

[Serializable]
public class DeviceUserSettings
{
    public bool AutoConnectOnLoad { get; set; } = true;
    public List<NeewerLedDeviceSettings> NeewerDeviceSettings { get; set; } = new List<NeewerLedDeviceSettings>();
    public int ConnectAttemptsAllowed { get; set; } = 10;
}