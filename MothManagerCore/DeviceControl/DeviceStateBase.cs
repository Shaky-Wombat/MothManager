﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MothManager.Core.DeviceControl;

[Serializable]
public abstract class DeviceStateBase : INotifyPropertyChanged
{
    private class MultiPropertyChangeScope : IDisposable
    {
        private readonly HashSet<string> _changedProperties = new HashSet<string>();
        private readonly MultiPropertyChangeScope? _previousMultiPropertyChangeScope;
        private readonly DeviceStateBase _owner;
            
        public MultiPropertyChangeScope(DeviceStateBase owner)
        {
            _owner = owner;
            _previousMultiPropertyChangeScope = owner.ChangeScope;
            owner.ChangeScope = this;
        }
            
        public void Dispose()
        {
            _owner.ChangeScope = _previousMultiPropertyChangeScope;

            if (_previousMultiPropertyChangeScope == null) 
            {
                _owner.OnMultiPropertyChanged(_changedProperties);   
            }
            else
            {
                _previousMultiPropertyChangeScope?._changedProperties.UnionWith(_changedProperties);
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            _changedProperties.Add(propertyName);
        }
    }

    private bool _power;
    private DeviceBase.DeviceMode _mode;
    private int _temperature;
    private float _hue;
    private float _saturation;
    private float _brightness;
    private int _customSceneId;
        
    private MultiPropertyChangeScope? ChangeScope { get; set; }

    [JsonIgnore]
    public abstract Type CustomModeIdEnumType { get; }

    public bool Power
    {
        get => _power;
        set => SetField(ref _power, value);
    }

    public DeviceBase.DeviceMode Mode
    {
        get => _mode;
        set => SetField(ref _mode, value);
    }

    public int Temperature
    {
        get => _temperature;
        set => SetField(ref _temperature, value);
    }

    public float Hue
    {
        get => _hue;
        set => SetField(ref _hue, value);
    }

    public float Saturation
    {
        get => _saturation;
        set => SetField(ref _saturation, value);
    }

    public float Brightness
    {
        get => _brightness;
        set => SetField(ref _brightness, value);
    }

    public int CustomSceneId
    {
        get => _customSceneId;
        set => SetField(ref _customSceneId, value);
    }
        
    protected DeviceStateBase()
    {
    }

    protected DeviceStateBase(DeviceStateBase state)
    {
        _power = state.Power;
        _mode = state.Mode;
        _temperature = state.Temperature;
        _hue = state.Hue;
        _saturation = state.Saturation;
        _brightness = state.Brightness;
        _customSceneId = state.CustomSceneId;
    }

    public void SetWhite(int temperature, float brightness)
    {
        using (new MultiPropertyChangeScope(this))
        {
            Mode = DeviceBase.DeviceMode.White;
            Temperature = temperature;
            Brightness = brightness;
        }
    }

    public void SetColor(float hue, float saturation, float brightness)
    {
        using (new MultiPropertyChangeScope(this))
        {
            Mode = DeviceBase.DeviceMode.Color;
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
        }
    }

    public void SetCustomScene(int customSceneId, float brightness)
    {
        using (new MultiPropertyChangeScope(this))
        {
            Mode = DeviceBase.DeviceMode.Custom;
            CustomSceneId = customSceneId;
            Brightness = brightness;
        }
    }

    public abstract DeviceStateBase Clone();

    public virtual void CopyFrom(DeviceStateBase state)
    {
        using (new MultiPropertyChangeScope(this))
        {
            Power = state.Power;
            Mode = state.Mode;
            Temperature = state.Temperature;
            Hue = state.Hue;
            Saturation = state.Saturation;
            Brightness = state.Brightness;
            CustomSceneId = state.CustomSceneId;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
        
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (ChangeScope != null && propertyName != null)
        {
            ChangeScope.OnPropertyChanged(propertyName);
        }
        else
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));    
        }
    }
        

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
        
        
    private void OnMultiPropertyChanged(HashSet<string> changedProperties)
    {
        foreach (var propertyName in changedProperties)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}