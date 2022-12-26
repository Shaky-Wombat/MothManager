// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Threading;
using MothManagerNeewerLEDControl;
using MothManagerNeewerLEDControl.Logger;

namespace MothManagerConsoleTestApp
{


    internal class Program
    {
        static volatile bool keepRunning = true;

        static Thread discoverBleThread;
        static Dictionary<string, string> foundDevices = null;

        static void Main(string[] args)
        {
            Logger.SetLogger(new ConsoleLogger());
            
            //var sctx = SynchronizationContext.Current;
            
            //Run(args).Wait();
            discoverBleThread = new Thread(DiscoverBleThread);
            discoverBleThread.Start();

            while (discoverBleThread.IsAlive)
            {
            }

            DeviceWatcher.StartMonitoring();
            
            Console.ReadLine();
            
            DeviceWatcher.StopMonitoring();
            
            // bleDevices[id].sendQueue.Enqueue(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.PowerRequest));
            // bleDevices[id].receiveCharacteristic.StartNotificationsAsync().Wait();
            // characteristic = bleDevices[id].receiveCharacteristic;
            var bleDevices = BleDeviceManager.bleDevices;
            // Console.WriteLine($"\t\tReceive Characteristic : {characteristic.Uuid} - {characteristic.UserDescription}, {characteristic.Properties} = {BitConverter.ToString(characteristic.Value)}");
            //bleDevices[id].sendQueue.Enqueue(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.SceneSend, 50, (byte)NeewerLightUtil.SceneId.Fireworks));
            foreach (var id in foundDevices.Keys)
            {
                bleDevices[id].sendCharacteristic
                    .WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.PowerSend,
                        1));
                bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.HSVSend, NeewerLightUtil.HsvToBytes(new ColorStruct.HSV(new Random().Next(360) / 360f, 1f,.25f))));

                //bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.CCTSend, 44, 50));
                var characteristic = bleDevices[id].sendCharacteristic;
            }

            Console.ReadLine();

            BleDeviceManager.DisconnectAll();
        }

        private static void DiscoverBleThread(object inName)
        {
            foundDevices = DeviceSearchBLE.DiscoverDevicesAsync().Result;

            if (foundDevices != null)
            {
                var bleScanned = "Found: " + foundDevices.Count + "\n";
                foreach (var kvp in foundDevices)
                {
                    var specs = NeewerLightUtil.GetDeviceSpec(kvp.Value);

                    bleScanned +=
                        $"{kvp.Value} : {kvp.Key} - {specs.minTemperature}K-{specs.maxTemperature}K - White:{specs.cctOnly}\n";
                    BleDeviceManager.connectionList.Add(kvp.Key);
                }

                Logger.WriteLine($" * <color:blue>{bleScanned}</color>");
            }

            //Console.WriteLine(BleDeviceManager.MonitorLog);
            //Console.WriteLine("Done.");
        }
    }
}