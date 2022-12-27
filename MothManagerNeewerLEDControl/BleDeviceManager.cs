// using System;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
// using InTheHand.Bluetooth;
// using MothManager.Core.DeviceControl;
// using MothManager.Core.Logger;
//
// namespace MothManager.NeewerLEDControl
// {
//     public class BleDeviceManager
//     {
//         static BluetoothUuid mainServiceUuid = BluetoothUuid.FromGuid(new Guid("02FE4875-5056-48B5-AD15-36E30665D9B4"));
//         static BluetoothUuid mainCommandUuid = BluetoothUuid.FromGuid(new Guid("220154BF-1DCE-4F03-85F0-7BA905D2D6B0"));
//
//         static BluetoothUuid mainAuthenticateUuid =
//             BluetoothUuid.FromGuid(new Guid("4C75BB42-5365-458D-A3EA-2B91339646B7"));
//
//         static BluetoothUuid mainPlayUuid = BluetoothUuid.FromGuid(new Guid("3B140EF5-0A72-4891-AD38-83B5A2595622"));
//         static BluetoothUuid mainStatusUuid = BluetoothUuid.FromGuid(new Guid("D01C9106-91BD-4998-9554-85264D33ACB2"));
//
//         public static string MonitorLog = "";
//
//         // public static ConcurrentDictionary<string, NeewerLEDDeviceConnection> bleDevices =
//         //     new ConcurrentDictionary<string, NeewerLEDDeviceConnection>();
//
//         public static List<string> connectionList = new List<string>();
//         
//         // public static void StartMonitoring()
//         // {
//         //     //connectionList = bleDeviceConnectionList;
//         //
//         //     foreach (string str in connectionList)
//         //     {
//         //         bleDevices[str] = new NeewerLEDDeviceConnection();
//         //         bleDevices[str]. // new ThreadStart(ListenToConnected));
//         //         bleDevices[str].;
//         //     }
//         //
//         //     MonitorLog = "";
//         // } 
//
//         
//
//         
//
//         public static void Disconnect(NeewerLEDDeviceConnection device)
//         {
//  
//         }
//
//         // public static void DisconnectAll()
//         // {
//         //     foreach (KeyValuePair<string, NeewerLEDDeviceConnection> kvp in bleDevices)
//         //     {
//         //         kvp.Value.stayActive = false;
//         //     }
//         //
//         //     foreach (KeyValuePair<string, NeewerLEDDeviceConnection> kvp in bleDevices)
//         //     {
//         //         NeewerLEDDeviceConnection bd = kvp.Value;
//         //         kvp.Value.stayActive = false;
//         //         if (bd.bluetoothDevice != null)
//         //             bd.bluetoothDevice.Gatt.Disconnect();
//         //     }
//         //     //bleDevices = new ConcurrentDictionary<string, BleDevice>();
//         // }
//     }
// }