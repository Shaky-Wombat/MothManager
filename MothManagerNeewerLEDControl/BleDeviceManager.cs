using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InTheHand.Bluetooth;
using MothManagerNeewerLEDControl.Logger;

namespace MothManagerNeewerLEDControl
{
    public class BleDeviceManager
    {
        static BluetoothUuid mainServiceUuid = BluetoothUuid.FromGuid(new Guid("02FE4875-5056-48B5-AD15-36E30665D9B4"));
        static BluetoothUuid mainCommandUuid = BluetoothUuid.FromGuid(new Guid("220154BF-1DCE-4F03-85F0-7BA905D2D6B0"));

        static BluetoothUuid mainAuthenticateUuid =
            BluetoothUuid.FromGuid(new Guid("4C75BB42-5365-458D-A3EA-2B91339646B7"));

        static BluetoothUuid mainPlayUuid = BluetoothUuid.FromGuid(new Guid("3B140EF5-0A72-4891-AD38-83B5A2595622"));
        static BluetoothUuid mainStatusUuid = BluetoothUuid.FromGuid(new Guid("D01C9106-91BD-4998-9554-85264D33ACB2"));

        public static string MonitorLog = "";

        public static ConcurrentDictionary<string, BleDevice> bleDevices =
            new ConcurrentDictionary<string, BleDevice>();

        public static List<string> connectionList = new List<string>();
        
        public static void StartMonitoring()
        {
            //connectionList = bleDeviceConnectionList;

            foreach (string str in connectionList)
            {
                bleDevices[str] = new BleDevice();
                bleDevices[str].monitorThread = new Thread(MonitorDevice); // new ThreadStart(ListenToConnected));
                bleDevices[str].monitorThread.Start("" + str);
            }

            MonitorLog = "";
        } 

        public static async void QueryGATT(string bdid)
        {
            BluetoothDevice bd = null;
            while (bd == null)
            {
                var result = Task.Run(() => BluetoothDevice.FromIdAsync(bdid));
                bd = result.Result;
            }

            BleDevice device = bleDevices[bd.Id];
            device.status = BleDevice.DeviceStatus.QueryingService;
            device.name = bd.Name;
            device.id = bd.Id;
            RemoteGattServer rgs = bd.Gatt;
            Logger.Logger.WriteLine($"{bd.Name} = {bd.Id}", "Querying GATT");

            try
            {
                var attemptsRemaining = 4;

                while (attemptsRemaining > 0 && device.lightControlService == null)
                {
                    Logger.Logger.WriteLine($"Query Service (Attempts remaining: {attemptsRemaining}", $"{bd.Name} = {bd.Id}");
                    
                    device.lightControlService = await rgs.GetPrimaryServiceAsync(NeewerLightUtil.LightControlServiceId);
                    attemptsRemaining--;
                }
                
                if (device.lightControlService == null)
                {
                    Logger.Logger.WriteLine(LogEntryType.Error, $"FAILED TO GET LIGHT CONTROL SERVICE!", $"{bd.Name} = {bd.Id}");
                    
                    device.status = BleDevice.DeviceStatus.MissingService;
                    return;
                }

                device.status = BleDevice.DeviceStatus.QueryingReceive;
                attemptsRemaining = 4;
                
                while (attemptsRemaining > 0 && device.receiveCharacteristic == null)
                {
                    Logger.Logger.WriteLine($"Query Receive (Attempts remaining: {attemptsRemaining}", $"{bd.Name} = {bd.Id}");
                    
                    device.receiveCharacteristic = await device.lightControlService.GetCharacteristicAsync(NeewerLightUtil.ReceiveCharacteristicId);
                    attemptsRemaining--;
                }
                
                
                if (device.receiveCharacteristic == null)
                {
                    Logger.Logger.WriteLine(LogEntryType.Error, "FAILED TO GET LIGHT DATA RECEIVE CHARACTERISTIC\n", $"{bd.Name} = {bd.Id}");
                    device.status = BleDevice.DeviceStatus.MissingReceive;
                    return;
                }
                
                device.status = BleDevice.DeviceStatus.QueryingSend;
                attemptsRemaining = 4;
                
                while (attemptsRemaining > 0 && device.sendCharacteristic == null)
                {
                    Logger.Logger.WriteLine("Query Send (Attempts remaining: {attemptsRemaining}", $"{bd.Name} = {bd.Id}");
                    Console.ResetColor();
                    device.sendCharacteristic = await device.lightControlService.GetCharacteristicAsync(NeewerLightUtil.SendCharacteristicId);
                    attemptsRemaining--;
                }
                
                if (device.sendCharacteristic == null)
                {
                    Logger.Logger.WriteLine(LogEntryType.Error, "FAILED TO GET LIGHT DATA SEND CHARACTERISTIC\n", $"{bd.Name} = {bd.Id}");
                    device.status = BleDevice.DeviceStatus.MissingSend;
                    return;
                }

                //device.log = bd.Name + " BLE thread(" + device.monitorThread.ManagedThreadId.ToString() + ")\n";
                // device.log = bd.Name + "(t" + device.monitorThread.ManagedThreadId.ToString() + "), g=" + gattServices.Count + " " + device.id + " OK";
                device.bluetoothDevice = bd;
                //bleDevices[device.id] = device;
                device.status = BleDevice.DeviceStatus.Ready;
            }
            catch (Exception ex)
            {
                device.log = "GATT Exception\n" + ex.ToString();
                Logger.Logger.WriteLine(LogEntryType.Error, $"GATT Exception\n{ex}", $"{bd.Name} = {bd.Id}");
            }
        }

        private static void MonitorDevice(object in_name)
        {
            string id = (string)in_name;

            var initializedDevices = new HashSet<string>();
            
            while (!bleDevices.ContainsKey(id) || bleDevices[id].stayActive)
            {
                if (bleDevices.ContainsKey(id) && bleDevices[id].status != BleDevice.DeviceStatus.Ready)
                {
                    bleDevices[id].log = "Waiting : " + id;
                    try
                    {
                        if (bleDevices[id].status == BleDevice.DeviceStatus.Unititalized)
                        {
                            //bleDevices[id].log = id + " offline" + (bleDevices[id].queryingGatt ? "*" : "");
                            Task.Run(() => QueryGATT(id)).Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        bleDevices[id].log = ex.ToString();
                    }
                }
                else
                {
                    if (bleDevices.ContainsKey(id)
                        && bleDevices[id].bluetoothDevice != null
                        && !bleDevices[id].bluetoothDevice.Gatt.IsConnected
                        && bleDevices[id].status == BleDevice.DeviceStatus.Ready)
                    {
                        bleDevices[id].log = id + " DC";
                        bleDevices[id].status = BleDevice.DeviceStatus.Unititalized;
                    }
                    else
                    {
                        BleDevice device = bleDevices[id];
                        device.log = $"{device.name}(t{device.monitorThread.ManagedThreadId}), {device.id} OK";
                    }


                    while (bleDevices[id].sendQueue.Count > 0)
                    {
                        byte[] message;
                        if (bleDevices[id].status == BleDevice.DeviceStatus.Ready && bleDevices[id].sendCharacteristic != null)
                        {
                            bleDevices[id].sendQueue.TryDequeue(out message);
                            byte[] bytesCommand = Encoding.ASCII.GetBytes(message + "\r\n");
                            bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(bytesCommand).Wait();
                            // Console.WriteLine($"{id} Send -> {(NeewerLightUtil.CommandType)message[1]} ");
                        }
                    }
                }

/*               }
                else
                {
                    bleDevices[id].log = id + " DC";
                }*/

                if (!string.IsNullOrWhiteSpace(bleDevices[id].log) && bleDevices[id].log != bleDevices[id].previousLog)
                {
                    Console.WriteLine(bleDevices[id].log);
                    bleDevices[id].previousLog = bleDevices[id].log;
                    bleDevices[id].log = "";
                }
                
                if (bleDevices[id].status == BleDevice.DeviceStatus.Ready && !initializedDevices.Contains(id))
                {
                    string logMessage  = "<color:green>Querying GATT Finished :</color>\n"
                        + $"\t<color:white,blue>Control Service : {bleDevices[id].lightControlService.Uuid}</color>\n"
                        + $"\t\t<color:white,darkred>Receive Characteristic : {bleDevices[id].receiveCharacteristic.Uuid} - {bleDevices[id].receiveCharacteristic.Properties} = {BitConverter.ToString(bleDevices[id].receiveCharacteristic.Value)}</color>\n"
                        + $"\t\t<color:white,darkgreen>Send Characteristic : {bleDevices[id].receiveCharacteristic.Uuid} - {bleDevices[id].receiveCharacteristic.Properties} = {BitConverter.ToString(bleDevices[id].sendCharacteristic.Value)}</color>\n";
                    
                    Logger.Logger.WriteLine(logMessage);
                    // Console.ForegroundColor = ConsoleColor.Yellow;
                    // var characteristic = bleDevices[id].receiveCharacteristic;
                    // Console.WriteLine($"\t\tReceive Characteristic : {characteristic.Uuid} - {characteristic.UserDescription}, {characteristic.Properties} = {BitConverter.ToString(characteristic.Value)}");
                    // characteristic = bleDevices[id].sendCharacteristic;
                    // Console.WriteLine($"\t\tSend Characteristic : {characteristic.Uuid} - {characteristic.UserDescription}, {characteristic.Properties} = {BitConverter.ToString(characteristic.Value)}");
                    // Console.ResetColor();
                    initializedDevices.Add(id);

                    // // bleDevices[id].sendQueue.Enqueue(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.PowerRequest));
                    // // bleDevices[id].receiveCharacteristic.StartNotificationsAsync().Wait();
                    // Console.ForegroundColor = ConsoleColor.Blue;
                    // // characteristic = bleDevices[id].receiveCharacteristic;
                    // // Console.WriteLine($"\t\tReceive Characteristic : {characteristic.Uuid} - {characteristic.UserDescription}, {characteristic.Properties} = {BitConverter.ToString(characteristic.Value)}");
                    // //bleDevices[id].sendQueue.Enqueue(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.SceneSend, 50, (byte)NeewerLightUtil.SceneId.Fireworks));
                    // bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.PowerSend, 1));
                    // // bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.HSVSend, NeewerLightUtil.HsvToBytes(new ColorStruct.HSV(new Random().Next(360) / 360f, 1f,1f))));
                    //
                    // bleDevices[id].sendCharacteristic.WriteValueWithResponseAsync(NeewerLightUtil.GetCommandBytes(NeewerLightUtil.CommandType.CCTSend, 44, 50 ));
                    // characteristic = bleDevices[id].sendCharacteristic;
                    // Console.WriteLine($"\t\tSend Characteristic : {characteristic.Uuid} - {characteristic.UserDescription}, {characteristic.Properties} = {BitConverter.ToString(characteristic.Value)}");
                    //
                    // Console.ResetColor();
                }
            }
        }


        public static void DisconnectAll()
        {
            foreach (KeyValuePair<string, BleDevice> kvp in bleDevices)
            {
                kvp.Value.stayActive = false;
            }

            foreach (KeyValuePair<string, BleDevice> kvp in bleDevices)
            {
                BleDevice bd = kvp.Value;
                kvp.Value.stayActive = false;
                if (bd.bluetoothDevice != null)
                    bd.bluetoothDevice.Gatt.Disconnect();
            }
            //bleDevices = new ConcurrentDictionary<string, BleDevice>();
        }
    }
}