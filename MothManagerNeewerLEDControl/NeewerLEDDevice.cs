using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using InTheHand.Bluetooth;
using MothManager.Core;
using MothManager.Core.DeviceControl;
using MothManager.Core.Logger;

namespace MothManager.NeewerLEDControl
{
    /*public class BleCacheCharacteristic
    {
        public BluetoothUuid uuid;
        public GattCharacteristic gattCharacteristic;
    }

    public class BleCacheService
    {
        public BluetoothUuid uuid;
        public GattService gattService;

        public ConcurrentDictionary<BluetoothUuid, BleCacheCharacteristic> characteristics =
            new ConcurrentDictionary<BluetoothUuid, BleCacheCharacteristic>();
    }*/

    [Serializable]
    public class NeewerLedDeviceSettings : DeviceSettingsBase
    {
        public DeviceCapabilities Capabilities { get; set; }

        public NeewerLedDeviceSettings() : base(new NeewerLEDDeviceState())
        {
        }

        public NeewerLedDeviceSettings(DiscoveredNeewerLEDDeviceInfo info) : base(info, new NeewerLEDDeviceState())
        {
            Capabilities = info.Capabilities;
            State.SetWhite((Capabilities.minTemperature + Capabilities.maxTemperature) / 2, 0.5f);
        }

        public override void CopyFrom(DeviceSettingsBase settings, bool overwriteId = false)
        {
            base.CopyFrom(settings, overwriteId);

            if (settings is NeewerLedDeviceSettings neewerSettings)
            {
                Capabilities = neewerSettings.Capabilities;

                if (settings.State is NeewerLEDDeviceState)
                {
                    State = settings.State;
                }
                else
                {
                    State.CopyFrom(settings.State);
                }
                
            }
        }
    }

    public class NeewerLEDDeviceState : DeviceStateBase
    {
        public override Type CustomModeIdEnumType => typeof(NeewerLedDevice.SceneId);

        public NeewerLEDDeviceState() : base()
        {
        }
        
        public NeewerLEDDeviceState(NeewerLEDDeviceState state) : base(state)
        {
        }
        
        public override DeviceStateBase Clone()
        {
            return new NeewerLEDDeviceState(this);
        }
    }
    
    public class NeewerLedDevice : DeviceBase
    {
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


        private const byte CommandPrefix = 120;
        private static readonly BluetoothUuid LightControlServiceId = new Guid("69400001-b5a3-f393-e0a9-e50e24dcca99");
        private static readonly BluetoothUuid SendCharacteristicId = new Guid("69400002-b5a3-f393-e0a9-e50e24dcca99");
        private static readonly BluetoothUuid ReceiveCharacteristicId = new Guid("69400003-b5a3-f393-e0a9-e50e24dcca99");
       
        
        public enum DeviceStatus
        {
            Uninitialized,
            QueryingService,
            QueryingReceive,
            QueryingSend,
            GattInitialized,
            Ready,
            
            MissingService,
            MissingReceive,
            MissingSend,
            
            TryReconnect,
        }
        
        public BluetoothDevice bluetoothDevice;
        private GattService? lightControlService;
        private GattCharacteristic? receiveCharacteristic;
        private GattCharacteristic? sendCharacteristic;

        private Thread? monitorThread = null;
        private bool stayActive = true;

        public DeviceStatus Status
        {
            get => _status;
            private set => SetField(ref _status, value);
        }

        public ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
        private bool _gattCharacteristicsLogged;
        private DeviceStatus _status = DeviceStatus.Uninitialized;

        public override bool Connected => Status == DeviceStatus.Ready && bluetoothDevice.Gatt.IsConnected;

        public NeewerLedDevice(NeewerLedDeviceSettings settings) : base(settings)
        {
        }

        public override void Connect(int attemptsAllowed)
        {
            Logger.WriteLine($"<color:cyan>Connect [{Id}] {Name}</color>");

            monitorThread = new Thread(MonitorDevice);
            monitorThread.Start();
        }
        
        private async void MonitorDevice()
        {
            Logger.WriteLine($"<color:yellow>MonitorDevice Start [{Id}] {Name}</color>");
            
            stayActive = true;
            while (stayActive)
            {
                if (Status != DeviceStatus.Ready)
                {
                    try
                    {
                        if (Status == DeviceStatus.Uninitialized || Status == DeviceStatus.TryReconnect)
                        {
                            QueryGATT(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(LogEntryType.Error, ex.ToString());
                    }
                }
                else
                {
                    if (bluetoothDevice != null
                        && !bluetoothDevice.Gatt.IsConnected
                        && Status == DeviceStatus.Ready)
                    {
                        Logger.WriteLine(LogEntryType.Warning,  Id + " DC");
                        Status = DeviceStatus.TryReconnect;
                    }


                    while (sendQueue.Count > 0 && Status == DeviceStatus.Ready && sendCharacteristic != null)
                    {
                        sendQueue.TryDequeue(out var message);
                        
                        Logger.WriteLine($"<color:white:darkgreen>Sending {(CommandType)message[1]} to device</color>");
                        
                        
                        //byte[] bytesCommand = Encoding.ASCII.GetBytes(message + "\r\n");
                        sendCharacteristic.WriteValueWithResponseAsync(message).Wait();
                    }
                }
            }
            
            Logger.WriteLine($"<color:yellow>MonitorDevice End [{Id}] {Name}</color>");
        }
        
        public async void QueryGATT(bool loadSettingsOnFound)
        {
            Status = DeviceStatus.QueryingService;
            bluetoothDevice = null;
            while (bluetoothDevice == null)
            {
                var result = Task.Run(() => BluetoothDevice.FromIdAsync(Id));
                bluetoothDevice = result.Result;
            }

            RemoteGattServer rgs = bluetoothDevice.Gatt;
            Logger.WriteLine($"{Name} = {Id}",$"<color:Black:white>Querying GATT - Thread ID{Thread.CurrentThread.ManagedThreadId}</color>");

            try
            {
                var attemptsRemaining = 4;

                while (attemptsRemaining > 0 && lightControlService == null)
                {
                    Logger.WriteLine($"Query Service (Attempts remaining: {attemptsRemaining} - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");
                    
                    lightControlService = await rgs.GetPrimaryServiceAsync(LightControlServiceId);
                    attemptsRemaining--;
                }
                
                if (lightControlService == null)
                {
                    Logger.WriteLine(LogEntryType.Error, $"FAILED TO GET LIGHT CONTROL SERVICE!", $"{Name} = {Id}");
                    
                    Status = DeviceStatus.MissingService;
                    return;
                }
                
                Logger.WriteLine($"<Color:green>Service Found!</Color> - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");

                Status = DeviceStatus.QueryingReceive;
                attemptsRemaining = 4;
                
                while (attemptsRemaining > 0 && receiveCharacteristic == null)
                {
                    Logger.WriteLine($"Query Receive (Attempts remaining: {attemptsRemaining} - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");
                    
                    receiveCharacteristic = await lightControlService.GetCharacteristicAsync(ReceiveCharacteristicId);
                    attemptsRemaining--;
                }
                
                
                if (receiveCharacteristic == null)
                {
                    Logger.WriteLine(LogEntryType.Error, "FAILED TO GET LIGHT DATA RECEIVE CHARACTERISTIC\n", $"{Name} = {Id}");
                    Status = DeviceStatus.MissingReceive;
                    return;
                }
                
                Logger.WriteLine($"<Color:green>Receive Characteristic Found!</Color> - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");
                
                Status = DeviceStatus.QueryingSend;
                attemptsRemaining = 4;
                
                while (attemptsRemaining > 0 && sendCharacteristic == null)
                {
                    Logger.WriteLine($"Query Send (Attempts remaining: {attemptsRemaining} - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");
                    sendCharacteristic = await lightControlService.GetCharacteristicAsync(SendCharacteristicId);
                    attemptsRemaining--;
                }
                
                if (sendCharacteristic == null)
                {
                    Logger.WriteLine(LogEntryType.Error, "FAILED TO GET LIGHT DATA SEND CHARACTERISTIC\n", $"{Name} = {Id}");
                    Status = DeviceStatus.MissingSend;
                    return;
                }

                Logger.WriteLine($"<Color:green>Send Characteristic Found!</Color> - Thread ID{Thread.CurrentThread.ManagedThreadId}", $"{Name} = {Id}");

                Status = DeviceStatus.GattInitialized;

                if (loadSettingsOnFound)
                {
                    LoadStateFromSettings();
                    Status = DeviceStatus.Ready;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(LogEntryType.Error, $"GATT Exception\n{ex}", $"{Name} = {Id}");
            }
        }

        public override void Disconnect()
        {
            stayActive = false;
            
            if (bluetoothDevice != null)
            {
                bluetoothDevice.Gatt.Disconnect();
            }
        }

        protected override void SetCurrentState(DeviceStateBase value)
        {
            State.CopyFrom(value);
            
            SetPower(value.Power);
            SendModeToDevice();
        }

        protected override void SetPower(bool value)
        {
            State.Power = value;
            sendQueue.Enqueue(GetCommandBytes(CommandType.PowerSend, (byte)(value ? 1 : 2)));
        }

        protected override void SetMode(DeviceMode value)
        {
            State.Mode = value;
            SendModeToDevice();
        }

        protected override void SetTemperature(int value)
        {
            State.Temperature = value;

            //TODO: Handle CCT Only lights (Separate commands for temperature and brightness)
            
            if (State.Mode == DeviceMode.White)
            {
                SendWhiteToDevice();        
            }
        }

        protected override void SetHue(float value)
        {
            State.Hue = value;

            if (State.Mode == DeviceMode.Color)
            {
                SendColorToDevice();
            }
        }

        protected override void SetSaturation(float value)
        {
            State.Saturation = value;

            if (State.Mode == DeviceMode.Color)
            {
                SendColorToDevice();
            }
        }

        protected override void SetBrightness(float value)
        {
            State.Brightness = value;
            
            //TODO: Handle CCT Only lights (Separate commands for temperature and brightness)
            
            SendModeToDevice();
        }

        protected override void SetCustomSceneId(int value)
        {
            State.CustomSceneId = value;
            SendCustomSceneToDevice();
        }

        public override void SetWhite(int temperature, float brightness)
        {
            State.SetWhite(temperature, brightness);
            SendWhiteToDevice();
        }

        public override void SetColor(float hue, float saturation, float brightness)
        {
            State.SetColor(hue, saturation, brightness);
            SendColorToDevice();
        }

        public override void SetCustomScene(int customSceneId, float brightness)
        {
            State.SetCustomScene(customSceneId, brightness);
            SendCustomSceneToDevice();
        }

        private void SendModeToDevice()
        {
            switch (State.Mode)
            {
                case DeviceMode.White:
                    SendWhiteToDevice();
                    break;
                case DeviceMode.Color:
                    SendColorToDevice();
                    break;
                case DeviceMode.Custom:
                    SendCustomSceneToDevice();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SendWhiteToDevice()
        {
            //TODO: Handle CCT only devices.
            sendQueue.Enqueue(GetCommandBytes(CommandType.CCTSend, (byte)(State.Brightness * 100), (byte)(State.Temperature / 100)));
        }
        
        private void SendColorToDevice()
        {
            var hueInt = (int)(State.Hue * 360f) % 360;
            sendQueue.Enqueue(GetCommandBytes(CommandType.HSVSend, (byte)(hueInt & 255), (byte)((hueInt & 65280) >> 8), (byte)(State.Saturation * 100), (byte)(State.Brightness * 100)));
        }
        
        private void SendCustomSceneToDevice()
        {
            sendQueue.Enqueue(GetCommandBytes(CommandType.SceneSend, (byte)State.CustomSceneId, (byte)(State.Brightness * 100)));
        }

        public byte[] GetCommandBytes(CommandType commandType, params byte[] values)
        {
            var retVal = new byte[values.Length + 6];

            retVal[0] = CommandPrefix;
            retVal[1] = (byte)commandType;
            retVal[2] = (byte)values.Length;

            var checkSum = retVal[0] + retVal[1] + retVal[2];

            for (var i = 0; i < values.Length; i++)
            {
                retVal[i + 3] = values[i];
                checkSum += values[i];
            }

            retVal[^3] = unchecked((byte)checkSum);

            Logger.WriteLine($"<color:black,white>{commandType}</color> - {BitConverter.ToString(retVal)}", $"{Name} ({Id})");


            return retVal;
        }
    }
}