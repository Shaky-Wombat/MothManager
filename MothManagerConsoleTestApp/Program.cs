// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MothManager.Core;
using MothManager.Core.DeviceControl;
using MothManager.Core.Logger;
using MothManager.NeewerLEDControl;

namespace MothManagerConsoleTestApp
{
    internal class MenuEntry
    {
        public readonly string label;
        public readonly Func<Menu, Menu> selectedFunc;

        public MenuEntry(string label, Func<Menu, Menu> selectedFunc)
        {
            this.label = label;
            this.selectedFunc = selectedFunc;
        }

        
    }

    internal class Menu
    {
        private readonly string header;
        private readonly Func<string[]>? infoCallback;
        private readonly MenuEntry[] entries;
        public Menu BackMenu { get; set; }

        public Menu(string header, Func<string[]>? infoCallback, MenuEntry[] entries)
        {
            this.header = header;
            this.infoCallback = infoCallback;
            this.entries = entries;
        }

        public Menu? ExecuteMenu()
        {
            StringBuilder strBuilder = new StringBuilder();
            var headerLength = header.Length;

            // Header
            
            strBuilder.Append("<color:white,darkblue>");
            AppendDivider(ref strBuilder, headerLength, "+--", "-", "--+\n");
            strBuilder.AppendFormat("|  {0}  |\n", header);
            AppendDivider(ref strBuilder, headerLength, "+--", "-", "--+\n");
            strBuilder.Append("</color>");
            
            // Info

            if (infoCallback != null)
            {
                
                var infoLines = infoCallback.Invoke();
                var maxLength = infoLines.Select(infoLine => infoLine.Length).Max();
                
                strBuilder.Append("<color:cyan>");
                AppendDivider(ref strBuilder, maxLength, "+-", "-", "-+\n");

                foreach (var infoLine in infoLines)
                {
                    AppendPaddedLine(ref strBuilder, maxLength, "| ", infoLine, " |\n");
                }
                
                AppendDivider(ref strBuilder, maxLength, "+-", "-", "-+\n");
                strBuilder.Append("</color>");
            }
            
            // Entries

            for (var i = 0; i < entries.Length; i++)
            {
                strBuilder.AppendFormat("<color:white>{0})</color> - <color:yellow>{1}</color>\n", i, entries[i].label);
            }

            if (BackMenu != null)
            {
                strBuilder.AppendLine("<color:white,darkgreen>b) - Back</color>");
            }
 
            strBuilder.AppendLine("<color:white,red>x) - Exit</color>");

            // Selection
            
            strBuilder.AppendLine("<color:yellow,darkblue>+----------------------------------+");
            strBuilder.AppendLine("|  Type selection and hit [Enter]  |");
            strBuilder.AppendLine("+----------------------------------+</color>");

            Logger.WriteLine(LogEntryType.Raw,strBuilder.ToString());

            var selection = Console.ReadLine().Trim().ToLower();

            if (int.TryParse(selection, out var selectionIndex))
            {
                if (selectionIndex < 0 || selectionIndex >= entries.Length)
                {
                    return this;
                }

                return entries[selectionIndex].selectedFunc.Invoke(this);
            }
            
            if (selection.Equals("b") && BackMenu != null)
            {
                return BackMenu;
            }

            if (selection.Equals("x"))
            {
                return null;
            }

            return this;
        }

        public void AppendDivider(ref StringBuilder strBuilder, int length, string start, string mid, string end)
        {
            strBuilder.Append(start);

            for (var i = 0; i < length; i++)
            {
                strBuilder.Append(mid);
            }

            strBuilder.Append(end);
        }
        
        public void AppendPaddedLine(ref StringBuilder strBuilder, int length, string start, string line, string end)
        {
            strBuilder.Append(start);
            strBuilder.Append(line);
            
            for (var i = line.Length; i < length; i++)
            {
                strBuilder.Append(' ');
            }

            strBuilder.Append(end);
        }
    }
    
    internal class Program
    {
        private static readonly HashSet<DiscoveredNeewerLEDDeviceInfo> discoveredDeviceIdSelection = new HashSet<DiscoveredNeewerLEDDeviceInfo>();
        private static readonly HashSet<NeewerLedDevice> knownDeviceIdSelection = new HashSet<NeewerLedDevice>();
        
        private static Menu MainMenu = new Menu(
            "Main Menu",
            MainDeviceStatus,
            new[]
            {
                new MenuEntry("Discover Devices", DiscoverDevices),
                new MenuEntry("Set Discovered Device Selection", SetDiscoveredDeviceSelection),
                new MenuEntry("Connect to Discovered Devices", ConnectToDiscoveredDevices),
                new MenuEntry("Set Known Device Selection", SetKnownDeviceSelection),
                new MenuEntry("Set Power State", SetPowerState),
                new MenuEntry("Set White", SetWhite),
                new MenuEntry("Set Color", SetColor),
                new MenuEntry("Set Custom Scene", SetCustomScene),
                new MenuEntry("Disconnect Devices", DisconnectDevices)
            }
        );

        private static string[] MainDeviceStatus()
        {
            var lightNameCount = new Dictionary<string, int>();

            foreach (var device in knownDeviceIdSelection)
            {
                if (!lightNameCount.TryGetValue(device.Name, out var count))
                {
                    count = 0;
                }

                lightNameCount[device.Name] = count + 1;
            }

            var selectionStrings = new List<string>(lightNameCount.Count);

            foreach (var kvp in lightNameCount)
            {
                if (kvp.Value <= 1)
                {
                    selectionStrings.Add(kvp.Key);
                }
                else
                {
                    selectionStrings.Add($"{kvp.Key} x {kvp.Value}");
                }
            }
            
            return new[]
            {
               $"{_neewerManager.GetDiscoveredDeviceInfo().Count} devices discovered.",
               $"{_neewerManager.GetKnownDevices().Count} devices known.",
               $"Known Selection : {string.Join(", ",  selectionStrings)}",
            };
        }
        
        private static Menu DiscoverDevices(Menu backMenu)
        {
            _neewerManager.DiscoverDevices();
            discoveredDeviceIdSelection.UnionWith(_neewerManager.GetDiscoveredDeviceInfo());
            return MainMenu;
        }

        private static Menu SetDiscoveredDeviceSelection(Menu backMenu)
        {
            var discoveredDevices = _neewerManager.GetDiscoveredDeviceInfo();
            var entries = new MenuEntry[discoveredDevices.Count + 2];

            for (int i = 0; i < discoveredDevices.Count; i++)
            {
                var deviceInfo = discoveredDevices[i];
                bool isSelected = discoveredDeviceIdSelection.Contains(deviceInfo);
                entries[i] = new MenuEntry(
                    $"[{(isSelected ? 'X' : ' ')}] <color:{(isSelected ? "Green" : "Red")}> - ({deviceInfo.Id})</color>{deviceInfo.DeviceName}",
                    (backMenu) => SetDiscoveredDeviceSelected(deviceInfo, !isSelected, backMenu));
            }

            entries[^2] = new MenuEntry("Select All", (menu) => SelectAllDiscoveredDevices(true, backMenu));
            entries[^1] = new MenuEntry("Select None", (menu) => SelectAllDiscoveredDevices(false, backMenu));

            return new Menu("Discovered Device Selection", null, entries) { BackMenu = backMenu};
        }

        private static Menu SelectAllDiscoveredDevices(bool selected, Menu backMenu)
        {
            if (selected)
            {
                discoveredDeviceIdSelection.UnionWith(_neewerManager.GetDiscoveredDeviceInfo());
            }
            else
            {
                discoveredDeviceIdSelection.Clear();
            }

            return SetDiscoveredDeviceSelection(backMenu);
        }

        private static Menu SetDiscoveredDeviceSelected(DiscoveredNeewerLEDDeviceInfo discoveredDevice, bool selected, Menu backMenu)
        {
            if (selected)
            {
                discoveredDeviceIdSelection.Add(discoveredDevice);
            }
            else
            {
                discoveredDeviceIdSelection.Remove(discoveredDevice);
            }
            
            return SetDiscoveredDeviceSelection(backMenu);
        }

        
        private static Menu ConnectToDiscoveredDevices(Menu backMenu)
        {
            foreach (var deviceInfo in discoveredDeviceIdSelection)
            {
                _neewerManager.ConnectDevice(deviceInfo, new List<NeewerLedDeviceSettings>());    
            }

            knownDeviceIdSelection.UnionWith(_neewerManager.GetKnownDevices());
            var connectingDevices = new HashSet<NeewerLedDevice>(knownDeviceIdSelection);
            
            while (connectingDevices.Count > 0)
            {
                var device = connectingDevices.First();

                if (device == null)
                {
                    connectingDevices.Remove(connectingDevices.First());
                }
                else
                {
                    while (device.Status != NeewerLedDevice.DeviceStatus.Ready)
                    {

                    }

                    connectingDevices.Remove(device);
                    Logger.WriteLine("Connected and ready", $"{device.Name} ({device.Id})");
                }
            }

            return MainMenu;
        }

        private static Menu SetKnownDeviceSelection(Menu backMenu)
        {
            var knownDevices = _neewerManager.GetKnownDevices();
            var entries = new MenuEntry[knownDevices.Count + 2];

            for (int i = 0; i < knownDevices.Count; i++)
            {
                var deviceInfo = knownDevices[i];
                bool isSelected = knownDeviceIdSelection.Contains(deviceInfo);
                entries[i] = new MenuEntry(
                    $"[{(isSelected ? 'X' : ' ')}] <color:{(isSelected ? "Green" : "Red")}> - ({deviceInfo.Id})</color>{deviceInfo.Name}",
                    (backMenu) => SetKnownDeviceSelected(deviceInfo, !isSelected, backMenu));
            }

            entries[^2] = new MenuEntry("Select All", (menu) => SelectAllKnownDevices(true, backMenu));
            entries[^1] = new MenuEntry("Select None", (menu) => SelectAllKnownDevices(false, backMenu));

            return new Menu("Known Device Selection", null, entries) { BackMenu = backMenu};
        }

        private static Menu SelectAllKnownDevices(bool selected, Menu backMenu)
        {
            if (selected)
            {
                knownDeviceIdSelection.UnionWith(_neewerManager.GetKnownDevices());
            }
            else
            {
                knownDeviceIdSelection.Clear();
            }

            return SetKnownDeviceSelection(backMenu);
        }

        private static Menu SetKnownDeviceSelected(NeewerLedDevice knownDevice, bool selected, Menu backMenu)
        {
            if (selected)
            {
                knownDeviceIdSelection.Add(knownDevice);
            }
            else
            {
                knownDeviceIdSelection.Remove(knownDevice);
            }
            
            return SetKnownDeviceSelection(backMenu);
        }

        
        private static Menu SetPowerState(Menu backMenu)
        {
            var selectionString = "-1";
            var selectionInt = -1;
             
            while (!int.TryParse(selectionString, out selectionInt) || selectionInt is < 0 or > 2)
            {
                Logger.WriteLine(LogEntryType.Raw, "<color:white>Select Power State, 0 = off, 1 == on 2 = toggle, b = back</color>");
                
                selectionString = Console.ReadLine().Trim().ToLower();
                
                if (selectionString == "b")
                {
                    return backMenu;
                }
            }

            switch (selectionInt)
            {
                case 0:
                    foreach (var device in knownDeviceIdSelection)
                    {
                        device.Power = false;
                    }

                    break;

                case 1:
                    foreach (var device in knownDeviceIdSelection)
                    {
                        device.Power = true;
                    }

                    break;

                case 2:
                    foreach (var device in knownDeviceIdSelection)
                    {
                        device.Power = !device.Power;
                    }

                    break;
            }


            return backMenu;
        }

        
        private static Menu SetWhite(Menu backMenu)
        {
            var inputString = "-1";
            var temperatureInt = -1;
             
            while (!int.TryParse(inputString, out temperatureInt) || temperatureInt is < 3200 or > 5500)
            {
                Logger.WriteLine(LogEntryType.Raw, "Enter temperature in kelvin usually between 3200 and 5500");
                
                inputString = Console.ReadLine().Trim().ToLower();
            }
            
            inputString = "-1";
            var brightnessInt = -1;
            
            while (!int.TryParse(inputString, out brightnessInt) || brightnessInt is < 0 or > 100)
            {
                Logger.WriteLine(LogEntryType.Raw, "Enter brightness between 0 and 100");
                
                inputString = Console.ReadLine().Trim().ToLower();
            }

            foreach (var device in knownDeviceIdSelection)
            {
                device.SetWhite(temperatureInt, brightnessInt / 100f);
            }

            return backMenu;
        }

        
        private static Menu SetColor(Menu backMenu)
        {
            var inputString = "-1";
            var hueInt = -1;
             
            while (!int.TryParse(inputString, out hueInt) || hueInt is < 0 or > 360)
            {
                Logger.WriteLine(LogEntryType.Raw, "Enter hue between 0 and 360");
                
                inputString = Console.ReadLine().Trim().ToLower();
            }
            
            inputString = "-1";
            var SaturationInt = -1;
            
            while (!int.TryParse(inputString, out SaturationInt) || SaturationInt is < 0 or > 100)
            {
                Logger.WriteLine(LogEntryType.Raw, "Enter saturation between 0 and 100");
                
                inputString = Console.ReadLine().Trim().ToLower();
            }
            
            inputString = "-1";
            var BrightnessInt = -1;
            
            while (!int.TryParse(inputString, out BrightnessInt) || BrightnessInt is < 0 or > 100)
            {
                Logger.WriteLine(LogEntryType.Raw, "Enter brightness between 0 and 100");
                
                inputString = Console.ReadLine().Trim().ToLower();
            }

            foreach (var device in knownDeviceIdSelection)
            {
                device.SetColor(hueInt / 360f, SaturationInt / 100f, BrightnessInt / 100f);
            }

            return backMenu;
        }


        private static Menu SetCustomScene(Menu backMenu)
        {
            throw new NotImplementedException();
        }


        private static Menu DisconnectDevices(Menu backMenu)
        {
            throw new NotImplementedException();
        }



        static volatile bool publikeepRunning = true;

        static Thread discoverBleThread;
        private static NeewerLedDeviceManager _neewerManager;

        static void Main(string[] args)
        {
            Logger.SetLogger(new ConsoleLogger());
            
            //var sctx = SynchronizationContext.Current;
            
            //Run(args).Wait();
            // discoverBleThread = new Thread(DiscoverBleThread);
            // discoverBleThread.Start();
            //
            // while (discoverBleThread.IsAlive)
            // {
            // }
            
            _neewerManager = new NeewerLedDeviceManager();
            _neewerManager.Initialize();

            var currentMenu = MainMenu;

            while (currentMenu != null)
            {
                currentMenu = currentMenu.ExecuteMenu();
            }
            
            /*
            foreach (var discoveredDevice in discovered)
            {
                neewerManager.ConnectDevice(discoveredDevice.Value);
            }
            */

            // DeviceWatcher.StartMonitoring();

            _neewerManager.Cleanup();

            // DeviceWatcher.StopMonitoring();
            //
            // var bleDevices = BleDeviceManager.bleDevices;
            // foreach (var id in foundDevices.Keys)
            // {
            //     bleDevices[id].sendCharacteristic
            //         .WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.PowerSend,
            //             1));
            //     bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.HSVSend, NeewerLightUtil.HsvToBytes(new ColorStruct.HSV(new Random().Next(360) / 360f, 1f,.25f))));
            //
            //     var characteristic = bleDevices[id].sendCharacteristic;
            // }
            //
            // Console.ReadLine();
            //
            // BleDeviceManager.DisconnectAll();
        }
    }
}