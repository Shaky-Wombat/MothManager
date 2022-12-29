using System;

namespace MothManager.Core.DeviceControl;

[Serializable]
public abstract class DeviceSettingsBase
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DeviceName { get; set; }
    public DeviceStateBase State {get; set; }

    protected DeviceSettingsBase(DeviceStateBase defaultState)
    {
        State = defaultState;
    }

    protected DeviceSettingsBase(DiscoveredDeviceInfoBase info, DeviceStateBase defaultSate)
    {
        Id = info.Id;
        Name = info.DeviceName;
        DeviceName = info.DeviceName;
        State = defaultSate;
    }

    public virtual void CopyFrom(DeviceSettingsBase settings, bool overwriteId = false)
    {
        if (overwriteId)
        {
            Id = settings.Id;
        }

        Name = settings.Name;
        DeviceName = settings.DeviceName;
        State.CopyFrom(settings.State);
    }
}