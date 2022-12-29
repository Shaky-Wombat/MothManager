using System.ComponentModel;
using MothManager.NeewerLEDControl;

namespace MothManagerTrayApp.Controls
{
    public partial class NeewerLedPanelSettings : UserControl
    {
        private readonly NeewerLedDevice _targetDevice;
        private bool initialized = false;

        private string TargetName
        {
            get => _targetDevice.Settings.Name;
            set => _targetDevice.Settings.Name = value;
        }
    

        public NeewerLedPanelSettings(NeewerLedDevice targetDevice)
        {
            _targetDevice = targetDevice;
            InitializeComponent();
            _targetDevice.PropertyChanged += DevicePropertyChanged;
            _targetDevice.CurrentState.PropertyChanged += DevicePropertyChanged;
            
            initialized = true;
            RefreshFromDevice();
        }

        private void DevicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.InvokeOnUIThread(RefreshFromDevice);
        }

        private void RefreshFromDevice()
        {
            if (!initialized)
            {
                return;
            }
            
            nameTextBox.Text = TargetName;
            deviceNameLabel.Text = _targetDevice.Settings.DeviceName;
            idLabel.Text = _targetDevice.Id;
            statusLabel.Text = _targetDevice.Status.ToString();
        }

        private void NeewerLEDPanelSettings_Load(object sender, EventArgs e)
        {

        }

        private void togglePower_Click(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }
            
            _targetDevice.Power = !_targetDevice.Power;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }

            TargetName = nameTextBox.Text;
        }
    }
}
