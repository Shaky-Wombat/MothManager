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
    
    public class NeewerLEDControlManager : ControlManagerBase
    {
        private Thread discoverBleThread;
        private Dictionary<string, DiscoveredDeviceInfoBase> discoveredDevices = null;

        private Dictionary<string, NeewerLedDevice> devices =
            new Dictionary<string, NeewerLedDevice>();

        public IReadOnlyList<DiscoveredDeviceInfoBase> GetDiscoveredDeviceInfo()
        {
            return discoveredDevices?.Values.ToList() ?? new List<DiscoveredDeviceInfoBase>();
        }
        
        public IReadOnlyList<NeewerLedDevice> GetKnownDevices()
        {
            return devices?.Values.ToList() ?? new List<NeewerLedDevice>();
        }

        protected override void InitializeManager()
        
        {
            
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

        public override void ConnectDevice(DiscoveredDeviceInfoBase deviceInfo)
        {
            if (deviceInfo is DiscoveredNeewerLEDDeviceInfo neewerLedDeviceInfo)
            {
                if (!devices.TryGetValue(deviceInfo.Id, out var device))
                {
                    device = new NeewerLedDevice(new NeewerLedDeviceSettingsBase(neewerLedDeviceInfo));
                    devices.Add(device.Id, device);
                }
                
                device.Connect();
            }
        }
        
        private void DiscoverBleThread(object? cancellationToken)
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

        public void Cleanup()
        {
            foreach (var device in devices)
            {
                device.Value.Disconnect();
            }
            
            devices.Clear();
        }
    }
}