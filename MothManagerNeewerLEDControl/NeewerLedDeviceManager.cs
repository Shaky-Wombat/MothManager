using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MothManager.Core.DeviceControl;
using MothManager.Core.Logger;

namespace MothManager.NeewerLEDControl
{
    public class DiscoveredNeewerLEDDeviceInfo : DiscoveredDeviceInfoBase
    {
        private static readonly Dictionary<string, DeviceCapabilities> DeviceSpecs = new Dictionary<string, DeviceCapabilities>
        {
            //{"", new DeviceSpec()},
            { "Apollo", DeviceCapabilities.White(5600) },
            { "GL1", DeviceCapabilities.White(2900, 7000) },
            { "NL140", DeviceCapabilities.White() },
            { "SNL1320", DeviceCapabilities.White() },
            { "SNL1920", DeviceCapabilities.White() },
            { "SNL480", DeviceCapabilities.White() },
            { "SNL530", DeviceCapabilities.White() },
            { "SNL660", DeviceCapabilities.White() },
            { "SNL960", DeviceCapabilities.White() },
            { "SRP16", DeviceCapabilities.White() },
            { "SRP18", DeviceCapabilities.White() },
            { "WRP18", DeviceCapabilities.White() },
            { "ZRP16", DeviceCapabilities.White() },
            { "BH30S", DeviceCapabilities.RGBHigh() },
            { "CB60", DeviceCapabilities.RGBHigh(6500) },
            { "CL124", DeviceCapabilities.RGBHigh() },
            { "RGBC80", DeviceCapabilities.RGBHigh() },
            { "RGBCB60", DeviceCapabilities.RGBHigh() },
            { "RGB1000", DeviceCapabilities.RGBHigh() },
            { "RGB1200", DeviceCapabilities.RGBHigh() },
            { "RGB140", DeviceCapabilities.RGBHigh() },
            { "RGB168", DeviceCapabilities.RGBHigh(8500) },
            { "RGB176A1", DeviceCapabilities.RGBHigh() },
            { "RGB512", DeviceCapabilities.RGBHigh() },
            { "RGB800", DeviceCapabilities.RGBHigh() },
            { "SL-90", DeviceCapabilities.RGBHigh() },
            { "RGB1", DeviceCapabilities.RGB() },
            { "RGB176", DeviceCapabilities.RGB() },
            { "RGB18", DeviceCapabilities.RGB() },
            { "RGB190", DeviceCapabilities.RGB() },
            { "RGB450", DeviceCapabilities.RGB() },
            { "RGB480", DeviceCapabilities.RGB() },
            { "RGB530PRO", DeviceCapabilities.RGB() },
            { "RGB530", DeviceCapabilities.RGB() },
            { "RGB650", DeviceCapabilities.RGB() },
            { "RGB660PRO", DeviceCapabilities.RGB() },
            { "RGB660", DeviceCapabilities.RGB() },
            { "RGB960", DeviceCapabilities.RGB() },
            { "RGB-P200", DeviceCapabilities.RGB() },
            { "RGB-P280", DeviceCapabilities.RGB() },
            { "SL70", DeviceCapabilities.RGB(8500) },
            { "SL80", DeviceCapabilities.RGB(8500) },
            { "ZK-RY", DeviceCapabilities.RGB(5600, 5600) }
        };
        
        private static readonly IReadOnlyList<string> ExistingDeviceNames = DeviceSpecs.Keys.ToList();
        
        private static readonly DeviceCapabilities DefaultDeviceCapabilities = DeviceCapabilities.RGB();

        public static DeviceCapabilities GetDeviceSpec(string deviceName)
        {
            deviceName = deviceName.Replace(" ", "");

            foreach (var existingDeviceName in ExistingDeviceNames)
            {
                if (deviceName.Contains(existingDeviceName))
                {
                    return DeviceSpecs[existingDeviceName];
                }
            }

            return DefaultDeviceCapabilities;
        }
        
        public DeviceCapabilities Capabilities { get; }
        
        public DiscoveredNeewerLEDDeviceInfo(string id, string deviceName) : base(id, deviceName)
        {
            Capabilities = GetDeviceSpec(deviceName);
        }
    }
    
    public class NeewerLedDeviceManager : DeviceManagerManagerBase
    {
        private Thread discoverBleThread;
        private Dictionary<string, DiscoveredDeviceInfoBase> discoveredDevices = null;

        private readonly Dictionary<string, NeewerLedDevice> _devices = new Dictionary<string, NeewerLedDevice>();

        public event Action<NeewerLedDevice> OnDeviceAdded;

        public IReadOnlyList<DiscoveredDeviceInfoBase> GetDiscoveredDeviceInfo()
        {
            return discoveredDevices?.Values.ToList() ?? new List<DiscoveredDeviceInfoBase>();
        }
        
        public IReadOnlyList<NeewerLedDevice> GetKnownDevices()
        {
            return _devices.Values.ToList();
        }

        protected override void InitializeManager()
        
        {
            
        }

        public override void DiscoverDevices(Action<Dictionary<string, DiscoveredDeviceInfoBase>> completeCallback)
        {
            // ThreadStart starter = DiscoverBleThread;
            //
            // starter += () => completeCallback.Invoke(discoveredDevices);
            // discoverBleThread = new Thread(starter) { IsBackground = true };
            // discoverBleThread.Start();
        }

        public override Dictionary<string, DiscoveredDeviceInfoBase> DiscoverDevices()
        {
            discoverBleThread = new Thread(DiscoverBleThread);
            discoverBleThread.Start();

            while (discoverBleThread.IsAlive)
            {
            }

            return discoveredDevices;
        }

        public override void ConnectDevice(DiscoveredDeviceInfoBase deviceInfo, List<DeviceSettingsBase> knownDeviceSettings, int attemptsAllowed = 4)
        {
            Logger.WriteLine($"Neewer Manager ConnectDevice : preCount = {_devices.Count}");
            
            if (deviceInfo is DiscoveredNeewerLEDDeviceInfo neewerLedDeviceInfo)
            {
                if (!_devices.TryGetValue(deviceInfo.Id, out var device))
                {
                    NeewerLedDeviceSettings? settings = null; 
                    
                    foreach (var knownDeviceSetting in knownDeviceSettings)
                    {
                        if (knownDeviceSetting is not NeewerLedDeviceSettings knownNeewerDeviceSetting)
                        {
                            continue;
                        }
                        if (knownNeewerDeviceSetting.Id == deviceInfo.Id)
                        {
                            settings = knownNeewerDeviceSetting;
                        }
                    }
                    
                    device = new NeewerLedDevice(settings ?? new NeewerLedDeviceSettings(neewerLedDeviceInfo));

                    AddDevice(device);
                }
                
                device.Connect(attemptsAllowed);
            }
            
            Logger.WriteLine($"Neewer Manager ConnectDevice : postCount = {_devices.Count}");
        }

        private void AddDevice(NeewerLedDevice device)
        {
            Logger.WriteLine($"Neewer Manager AddDevice : preCount = {_devices.Count}");
            
            _devices.Add(device.Id, device);
            OnDeviceAdded.Invoke(device);
            
            Logger.WriteLine($"Neewer Manager AddDevice : postCount = {_devices.Count}");
        }

        private void DiscoverBleThread()
        {
            discoveredDevices = DeviceSearchBLE.DiscoverDevicesAsync().Result;

            if (discoveredDevices != null)
            {
                var logText = "Found " + discoveredDevices.Count + " Neewer LED Devices\n";
                foreach (var kvp in discoveredDevices)
                {
                    var deviceInfo = kvp.Value as DiscoveredNeewerLEDDeviceInfo;
                    logText += $"{deviceInfo.DeviceName} : {deviceInfo.Id} - {deviceInfo.Capabilities.minTemperature}K-{deviceInfo.Capabilities.maxTemperature}K - White Only:{deviceInfo.Capabilities.cctOnly}\n"; 
                   
                   //BleDeviceManager.connectionList.Add(kvp.Key);
                }

                Logger.WriteLine($" * <color:blue>{logText}</color>");
            }
        }
        
        public void UpdateKnownDeviceSettings(List<DeviceSettingsBase> deviceSettings, bool autoConnectOnLoad)
        {
            Logger.WriteLine($"Neewer Manager UpdateKnownDeviceSettings : preCount = {_devices.Count} -> in count: {deviceSettings.Count()}");
            
            foreach (var deviceSetting in deviceSettings)
            {
                if (_devices.TryGetValue(deviceSetting.Id, out var device))
                {
                    device.LoadStateFromSettings();
                }
                else
                {
                   AddDevice(new NeewerLedDevice(deviceSetting as NeewerLedDeviceSettings ?? new NeewerLedDeviceSettings()));    
                }
            }
            
            Logger.WriteLine($"Neewer Manager UpdateKnownDeviceSettings : postCount {_devices.Count}");
        }
        
        public List<DeviceSettingsBase> GetKnownDeviceSettings()
        {
            var retVal = new List<DeviceSettingsBase>();

            foreach (var device in _devices)
            {
                device.Value.SaveStateToSettings();
                retVal.Add(device.Value.Settings);
            }
            
            Logger.WriteLine($"Neewer Manager GetKnownDeviceSettings : {retVal.Count}");

            return retVal;
        }

        public void Cleanup()
        {
            Logger.WriteLine("Neewer Manager Cleanup!");
            
            foreach (var device in _devices)
            {
                device.Value.Disconnect();
            }
            
            _devices.Clear();
        }
    }
}