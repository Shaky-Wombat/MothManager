using System;
using System.Collections.Generic;

namespace MothManager.Core.DeviceControl
{
    public abstract class DeviceManagerManagerBase
    {
        public void Initialize()
        {
            InitializeManager();
        }
        
        protected abstract void InitializeManager();

        public abstract void DiscoverDevices(Action<Dictionary<string, DiscoveredDeviceInfoBase>> completeCallback);
        public abstract Dictionary<string, DiscoveredDeviceInfoBase> DiscoverDevices();
        public abstract void ConnectDevice(DiscoveredDeviceInfoBase deviceId, List<DeviceSettingsBase> knownDeviceSettings, int attemptsAllowed);
        
    }
}