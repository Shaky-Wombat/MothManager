using MothManager.Core.DeviceControl;

namespace MothManagerTrayApp;

[Serializable]
public class DeviceUserSettings
{
    public bool AutoConnectOnLoad { get; set; } = true;
    public List<DeviceSettingsBase> DeviceSettings { get; set; } = new List<DeviceSettingsBase>();
    public int ConnectAttemptsAllowed { get; set; } = 10;
}