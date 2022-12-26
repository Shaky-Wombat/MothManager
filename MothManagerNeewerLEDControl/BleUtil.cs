using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using InTheHand.Bluetooth;

// Ref : https://github.com/katascope/LightSuit/blob/ea82fe07368e9bd24e4b46867e20a01864f696ad/KataTracks/ConsoleBLE/DeviceSearchBLE.cs
//       https://github.com/taburineagle/NeewerLite-Python/blob/main/NeewerLite-Python.py

namespace MothManagerNeewerLEDControl
{
    public class DeviceSearchBLE
    {
        private static BluetoothLEScanFilter scanFilter = new BluetoothLEScanFilter();
        private static RequestDeviceOptions rdo = new RequestDeviceOptions();
        private static Dictionary<string, string> discoveredBLE = new Dictionary<string, string>();
        private static IReadOnlyCollection<BluetoothDevice> discoveredDevices = null;

        public static async Task<Dictionary<string, string>> DiscoverDevicesAsync()
        {
            scanFilter = new BluetoothLEScanFilter();
            rdo = new RequestDeviceOptions();
            discoveredBLE = new Dictionary<string, string>();
            scanFilter.NamePrefix = "NEEWER";
            rdo.Filters.Add(scanFilter);
            discoveredDevices = await Bluetooth.ScanForDevicesAsync();

            foreach (BluetoothDevice bd in discoveredDevices)
            {
                if (bd.Name.Contains("NEEWER"))
                {
                    if (!discoveredBLE.ContainsKey(bd.Id))
                    {
                        discoveredBLE[bd.Id] = bd.Name;
                    }
                }
            }

            return discoveredBLE;
        }
    }

    public class DeviceSpec
    {
        public readonly bool cctOnly;
        public readonly int minTemperature;
        public readonly int maxTemperature;

        public DeviceSpec(bool cctOnly, int minTemperature, int maxTemperature)
        {
            this.minTemperature = minTemperature;
            this.maxTemperature = maxTemperature;
            this.cctOnly = cctOnly;
        }

        public static DeviceSpec White(int minTemperature = 3200, int maxTemperature = 5600)
        {
            return new DeviceSpec(true, minTemperature, maxTemperature);
        }

        public static DeviceSpec RGB(int minTemperature, int maxTemperature)
        {
            return new DeviceSpec(false, minTemperature, maxTemperature);
        }

        public static DeviceSpec RGB(int maxTemperature = 5600)
        {
            return new DeviceSpec(false, 3200, maxTemperature);
        }

        public static DeviceSpec RGBHigh(int maxTemprature = 10000)
        {
            return new DeviceSpec(false, 2500, maxTemprature);
        }
    }

    public static class NeewerLightUtil
    {
        public enum SceneId : byte
        {
            CopCar = 1,
            Ambulance = 2,
            FireEngine = 3,
            Fireworks = 4,
            Party = 5,
            CandleLight = 6,
            Lightning = 7,
            Paparazzi = 8,
            TVScreen = 9
        }

        public enum CommandType : byte
        {
            ChannellRecieve = 1,
            PowerRecieve = 2,
            PowerSend = 129,
            CCTBrightnessSend = 130,
            CCTTemperatureSend = 131,
            PowerRequest = 132,
            ChannelRequest = 133,
            HSVSend = 134,
            CCTSend = 135,
            SceneSend = 136
        }

        private const byte CommandPrefix = 120;
        private static readonly DeviceSpec DefaultDeviceSpec = DeviceSpec.RGB();

        public static readonly BluetoothUuid LightControlServiceId = new Guid("69400001-b5a3-f393-e0a9-e50e24dcca99");
        public static readonly BluetoothUuid SendCharacteristicId = new Guid("69400002-b5a3-f393-e0a9-e50e24dcca99");
        public static readonly BluetoothUuid ReceiveCharacteristicId = new Guid("69400003-b5a3-f393-e0a9-e50e24dcca99");


        private static readonly Dictionary<string, DeviceSpec> DeviceSpecs = new Dictionary<string, DeviceSpec>
        {
            //{"", new DeviceSpec()},
            { "Apollo", DeviceSpec.White(5600) },
            { "GL1", DeviceSpec.White(2900, 7000) },
            { "NL140", DeviceSpec.White() },
            { "SNL1320", DeviceSpec.White() },
            { "SNL1920", DeviceSpec.White() },
            { "SNL480", DeviceSpec.White() },
            { "SNL530", DeviceSpec.White() },
            { "SNL660", DeviceSpec.White() },
            { "SNL960", DeviceSpec.White() },
            { "SRP16", DeviceSpec.White() },
            { "SRP18", DeviceSpec.White() },
            { "WRP18", DeviceSpec.White() },
            { "ZRP16", DeviceSpec.White() },
            { "BH30S", DeviceSpec.RGBHigh() },
            { "CB60", DeviceSpec.RGBHigh(6500) },
            { "CL124", DeviceSpec.RGBHigh() },
            { "RGBC80", DeviceSpec.RGBHigh() },
            { "RGBCB60", DeviceSpec.RGBHigh() },
            { "RGB1000", DeviceSpec.RGBHigh() },
            { "RGB1200", DeviceSpec.RGBHigh() },
            { "RGB140", DeviceSpec.RGBHigh() },
            { "RGB168", DeviceSpec.RGBHigh(8500) },
            { "RGB176A1", DeviceSpec.RGBHigh() },
            { "RGB512", DeviceSpec.RGBHigh() },
            { "RGB800", DeviceSpec.RGBHigh() },
            { "SL-90", DeviceSpec.RGBHigh() },
            { "RGB1", DeviceSpec.RGB() },
            { "RGB176", DeviceSpec.RGB() },
            { "RGB18", DeviceSpec.RGB() },
            { "RGB190", DeviceSpec.RGB() },
            { "RGB450", DeviceSpec.RGB() },
            { "RGB480", DeviceSpec.RGB() },
            { "RGB530PRO", DeviceSpec.RGB() },
            { "RGB530", DeviceSpec.RGB() },
            { "RGB650", DeviceSpec.RGB() },
            { "RGB660PRO", DeviceSpec.RGB() },
            { "RGB660", DeviceSpec.RGB() },
            { "RGB960", DeviceSpec.RGB() },
            { "RGB-P200", DeviceSpec.RGB() },
            { "RGB-P280", DeviceSpec.RGB() },
            { "SL70", DeviceSpec.RGB(8500) },
            { "SL80", DeviceSpec.RGB(8500) },
            { "ZK-RY", DeviceSpec.RGB(5600, 5600) }
        };

        private static readonly IReadOnlyList<string> ExistingDeviceNames = DeviceSpecs.Keys.ToList();

        public static DeviceSpec GetDeviceSpec(string deviceName)
        {
            deviceName = deviceName.Replace(" ", "");

            foreach (var existingDeviceName in ExistingDeviceNames)
            {
                if (deviceName.Contains(existingDeviceName))
                {
                    return DeviceSpecs[existingDeviceName];
                }
            }

            return DefaultDeviceSpec;
        }

        public static byte[] GetCommandBytes(CommandType commandType, params byte[] values)
        {
            var retVal = new byte[values.Length + 4];

            retVal[0] = CommandPrefix;
            retVal[1] = (byte)commandType;
            retVal[2] = (byte)values.Length;

            var checkSum = retVal[0] + retVal[1] + retVal[2];

            for (var i = 0; i < values.Length; i++)
            {
                retVal[i + 3] = values[i];
                checkSum += values[i];
            }

            retVal[^1] = unchecked((byte)checkSum);

            Logger.Logger.WriteLine($"<color:black,white>{commandType}</color> - {BitConverter.ToString(retVal)}");


            return retVal;
        }

        public static byte[] HsvToBytes(this ColorStruct.HSV hsv)
        {
            var h = (int)(hsv.H * 360f) % 360;
            return new[] { (byte)(h & 255), (byte)((h & 65280) >> 8), (byte)(hsv.S * 100), (byte)(hsv.V * 100) };
        }
    }
}