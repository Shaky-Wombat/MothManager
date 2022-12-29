using System;
using System.Collections.Generic;

namespace MothManager.Core.DeviceControl
{
    public abstract class DeviceManagerManagerBase<TDiscoveredDeviceInfo, TDeviceSettings, TState, TSceneIdEnum> 
        where TDiscoveredDeviceInfo:DiscoveredDeviceInfoBase 
        where TDeviceSettings  : DeviceSettingsBase<TDeviceSettings, TDiscoveredDeviceInfo, TState, TSceneIdEnum>
        where TState : DeviceStateBase<TState, TSceneIdEnum>
        where TSceneIdEnum : Enum
    {
        public void Initialize()
        {
            InitializeManager();
        }
        
        protected abstract void InitializeManager();

        public abstract void DiscoverDevices(Action<Dictionary<string, TDiscoveredDeviceInfo>> completeCallback);
        public abstract Dictionary<string, TDiscoveredDeviceInfo> DiscoverDevices();
        public abstract void ConnectDevice(TDiscoveredDeviceInfo deviceId, List<TDeviceSettings> knownDeviceSettings, int attemptsAllowed);
        
    }
}