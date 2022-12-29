using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MothManager.Core.DeviceControl;

public abstract class DeviceBase<TDiscoveredDeviceInfo, TSettings, TState, TSceneIdEnum> : INotifyPropertyChanged 
    where TDiscoveredDeviceInfo:DiscoveredDeviceInfoBase  
    where TSettings : DeviceSettingsBase<TSettings, TDiscoveredDeviceInfo, TState, TSceneIdEnum> 
    where TState : DeviceStateBase<TState, TSceneIdEnum>
    where TSceneIdEnum : Enum
{
    public TSettings Settings { get; }
    protected TState State { get; set; }
    public string Id => Settings.Id;
    public string Name => Settings.Name;

    public abstract bool Connected { get; }

    public TState CurrentState
    {
        get => State;
        set
        {
            SetCurrentState(value);
            OnPropertyChanged();
        }
    }

    public bool Power
    {
        get => State.Power;
        set
        {
            SetPower(value);
            OnPropertyChanged();
        }
    }

    public IDeviceState.DeviceMode Mode
    {
        get => State.Mode;
        set
        {
            SetMode(value);
            OnPropertyChanged();
        }
    }

    public int Temperature
    {
        get => State.Temperature;
        set
        {
            SetTemperature(value);
            OnPropertyChanged();
        }
    }

    public float Hue
    {
        get => State.Hue;
        set
        {
            SetHue(value);
            OnPropertyChanged();
        }
    }

    public float Saturation
    {
        get => State.Saturation;
        set
        {
            SetSaturation(value);
            OnPropertyChanged();
        }
    }

    public float Brightness
    {
        get => State.Brightness;
        set
        {
            SetBrightness(value);
            OnPropertyChanged();
        }
    }

    public TSceneIdEnum CustomSceneId
    {
        get => State.CustomSceneId;
        set
        {
            SetCustomSceneId(value);
            OnPropertyChanged();
        }
    }

    public abstract void Connect(int attemptsAllowed);
    public abstract void Disconnect();

    protected abstract void SetCurrentState(TState value);
    protected abstract void SetPower(bool value);
    protected abstract void SetMode(IDeviceState.DeviceMode value);
    protected abstract void SetTemperature(int value);
    protected abstract void SetHue(float value);
    protected abstract void SetSaturation(float value);
    protected abstract void SetBrightness(float value);
    protected abstract void SetCustomSceneId(TSceneIdEnum value);

    public abstract void SetWhite(int temperature, float brightness);
    public abstract void SetColor(float hue, float saturation, float brightness);
    public abstract void SetCustomScene(TSceneIdEnum customSceneId, float brightness);

    protected DeviceBase(TSettings settings)
    {
        Settings = settings;
        State = (TState)settings.State.Clone();
    }

    public void SaveStateToSettings()
    {
        Settings.State.CopyFrom(State);
    }

    public void LoadStateFromSettings()
    {
        State.CopyFrom(Settings.State);
        SetCurrentState(State);
    }

    public void ApplySettings(TSettings settings)
    {
        Settings.CopyFrom(settings);
        SetCurrentState(Settings.State);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}