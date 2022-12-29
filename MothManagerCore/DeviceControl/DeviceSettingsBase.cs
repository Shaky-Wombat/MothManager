using System;

namespace MothManager.Core.DeviceControl;

public interface IDeviceSettings
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DeviceName { get; set; }
    public abstract IDeviceState State {get; set; }
}

[Serializable]
public abstract class DeviceSettingsBase<TDeviceSettingsBase, TDiscoveredDeviceInfo, TState, TSceneIdEnum>:IDeviceSettings
    where TDeviceSettingsBase : DeviceSettingsBase<TDeviceSettingsBase, TDiscoveredDeviceInfo, TState, TSceneIdEnum>
    where TDiscoveredDeviceInfo:DiscoveredDeviceInfoBase 
    where TState : DeviceStateBase<TState, TSceneIdEnum>
    where TSceneIdEnum : Enum
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DeviceName { get; set; }
    
    IDeviceState IDeviceSettings.State
    {
        get => State;
        set
        {
            if (value is TState tState)
            {
                State = tState;
            }
        }
    }

    public TState State {get; set; }

    protected DeviceSettingsBase(TState defaultState)
    {
        State = defaultState;
    }

    protected DeviceSettingsBase(TDiscoveredDeviceInfo info, TState defaultSate)
    {
        Id = info.Id;
        Name = info.DeviceName;
        DeviceName = info.DeviceName;
        State = defaultSate;
    }

    public virtual void CopyFrom(TDeviceSettingsBase settings, bool overwriteId = false)
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