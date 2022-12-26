using System.Collections.Concurrent;
using System.Threading;
using InTheHand.Bluetooth;

namespace MothManagerNeewerLEDControl
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
    
    public class BleDevice
    {
        public enum DeviceStatus
        {
            Unititalized,
            QueryingService,
            QueryingReceive,
            QueryingSend,
            Ready,
            
            MissingService,
            MissingReceive,
            MissingSend,
        }
        
        public string id;
        public string name;
        public BluetoothDevice bluetoothDevice;
        public GattService? lightControlService;
        public GattCharacteristic? receiveCharacteristic;
        public GattCharacteristic? sendCharacteristic;

        // public ConcurrentDictionary<BluetoothUuid, BleCacheService> serviceCache =
        //     new ConcurrentDictionary<BluetoothUuid, BleCacheService>();

        public string log = "";
        public string previousLog = "";
        public Thread monitorThread;
        public bool stayActive = true;

        public DeviceStatus status = DeviceStatus.Unititalized;
        
        public ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
    }
}