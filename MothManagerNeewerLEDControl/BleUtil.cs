using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using InTheHand.Bluetooth;
using MothManager.Core;
using MothManager.Core.DeviceControl;
using MothManager.Core.Logger;

// Ref : https://github.com/katascope/LightSuit/blob/ea82fe07368e9bd24e4b46867e20a01864f696ad/KataTracks/ConsoleBLE/DeviceSearchBLE.cs
//       https://github.com/taburineagle/NeewerLite-Python/blob/main/NeewerLite-Python.py

namespace MothManager.NeewerLEDControl
{
    public class DeviceSearchBLE
    {
        private static BluetoothLEScanFilter scanFilter = new BluetoothLEScanFilter();
        private static RequestDeviceOptions rdo = new RequestDeviceOptions();
        //private static Dictionary<string, string> discoveredBLE = new Dictionary<string, string>();
        private static IReadOnlyCollection<BluetoothDevice> discoveredDevices = null;

        public static async Task<Dictionary<string, DiscoveredNeewerLEDDeviceInfo>> DiscoverDevicesAsync()
        {
            scanFilter = new BluetoothLEScanFilter();
            rdo = new RequestDeviceOptions();
            var discoveredBLE = new Dictionary<string, DiscoveredNeewerLEDDeviceInfo>();
            scanFilter.NamePrefix = "NEEWER";
            rdo.Filters.Add(scanFilter);

            discoveredDevices = await Bluetooth.ScanForDevicesAsync();

            foreach (BluetoothDevice bd in discoveredDevices)
            {
                if (bd.Name.Contains("NEEWER"))
                {
                    Logger.WriteLine($"<color:Magenta>Device ID = {bd.Id}, {bd.Name}</color>");
                    
                    if (!discoveredBLE.ContainsKey(bd.Id))
                    {
                        discoveredBLE[bd.Id] = new DiscoveredNeewerLEDDeviceInfo(bd.Id, bd.Name);
                    }
                }
            }

            return discoveredBLE;
        }
    }
    
    public class IncrementalDeviceSearchBLE
    {
        private BluetoothLEScanFilter scanFilter = new BluetoothLEScanFilter();
        private RequestDeviceOptions rdo = new RequestDeviceOptions();
        //private static Dictionary<string, string> discoveredBLE = new Dictionary<string, string>();
        private IReadOnlyCollection<BluetoothDevice> discoveredDevices = null;

        public async Task<Dictionary<string, DiscoveredDeviceInfoBase>> DiscoverDevicesAsync()
        {
            scanFilter = new BluetoothLEScanFilter();
            rdo = new RequestDeviceOptions();
            var discoveredBLE = new Dictionary<string, DiscoveredDeviceInfoBase>();
            scanFilter.NamePrefix = "NEEWER";
            rdo.Filters.Add(scanFilter);
            discoveredDevices = await Bluetooth.ScanForDevicesAsync();

            foreach (BluetoothDevice bd in discoveredDevices)
            {
                if (bd.Name.Contains("NEEWER"))
                {
                    Logger.WriteLine($"<color:Magenta>Device ID = {bd.Id}, {bd.Name}</color>");
                    
                    if (!discoveredBLE.ContainsKey(bd.Id))
                    {
                        discoveredBLE[bd.Id] = new DiscoveredNeewerLEDDeviceInfo(bd.Id, bd.Name);
                    }
                }
            }

            return discoveredBLE;
        }
    }

    public class DeviceCapabilities
    {
        public readonly bool cctOnly;
        public readonly int minTemperature;
        public readonly int maxTemperature;

        public DeviceCapabilities(bool cctOnly, int minTemperature, int maxTemperature)
        {
            this.minTemperature = minTemperature;
            this.maxTemperature = maxTemperature;
            this.cctOnly = cctOnly;
        }

        public static DeviceCapabilities White(int minTemperature = 3200, int maxTemperature = 5600)
        {
            return new DeviceCapabilities(true, minTemperature, maxTemperature);
        }

        public static DeviceCapabilities RGB(int minTemperature, int maxTemperature)
        {
            return new DeviceCapabilities(false, minTemperature, maxTemperature);
        }

        public static DeviceCapabilities RGB(int maxTemperature = 5600)
        {
            return new DeviceCapabilities(false, 3200, maxTemperature);
        }

        public static DeviceCapabilities RGBHigh(int maxTemprature = 10000)
        {
            return new DeviceCapabilities(false, 2500, maxTemprature);
        }
    }
}