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

        private string DeviceName => _targetDevice.Settings.DeviceName;
        private string Id => _targetDevice.Id;
        private string Status => _targetDevice.Status.ToString();

        private bool Power
        {
            get => _targetDevice.Power;
            set => _targetDevice.Power = value;
        }

        private int TemperatureMin
        {
            get => _targetDevice.Settings.Capabilities.minTemperature;
        }
        
        private int TemperatureMax
        {
            get => _targetDevice.Settings.Capabilities.maxTemperature;
        }

        private int Temperature
        {
            get => _targetDevice.Temperature;
            set => _targetDevice.Temperature = value;
        }

        private int Hue
        {
            get => (int)(_targetDevice.Hue * 359);
            set => _targetDevice.Hue = value / 359f;
        }

        private int Saturation
        {
            get => (int)(_targetDevice.Saturation * 100);
            set => _targetDevice.Saturation = value / 100f;
        }

        private NeewerSceneId SceneId
        {
            get => _targetDevice.CustomSceneId;
            set => _targetDevice.CustomSceneId = value;
        }
        
        private int Brightness
        {
            get => (int)(_targetDevice.Brightness * 100);
            set => _targetDevice.Brightness = value / 100f;
        }


        public NeewerLedPanelSettings(NeewerLedDevice targetDevice)
        {
            _targetDevice = targetDevice;
            InitializeComponent();
            sceneComboBox.Items.AddRange(Enum.GetValues<NeewerSceneId>().Select((v) => (object)v).ToArray());
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
            deviceNameLabel.Text = DeviceName;
            idLabel.Text = Id;
            statusLabel.Text = Status;
            tColorBar.Minimum = TemperatureMin;
            tColorBar.Maximum = TemperatureMax;
            tColorBar.Value = Temperature;
            tNumericUpDown.Minimum = TemperatureMin;
            tNumericUpDown.Maximum = TemperatureMax;
            tNumericUpDown.Value = Temperature;
            tNumericUpDown.Increment = 100;
            hColorBar.Value = Hue;
            hNumericUpDown.Value = Hue;
            sColorBar.Value = Saturation;
            sNumericUpDown.Value = Saturation;
            lColorBar.Value = Brightness;
            lNumericUpDown.Value = Brightness;
            
            sceneComboBox.SelectedIndex = sceneComboBox.Items.IndexOf(SceneId);

        }

        private void NeewerLEDPanelSettings_Load(object sender, EventArgs e)
        {

        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }

            TargetName = nameTextBox.Text;
        }

        private void tColorBar_ValueChanged(object sender, EventArgs e)
        {
            Temperature = (int)tColorBar.Value;
        }

        private void tNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Temperature = (int)tNumericUpDown.Value;
        }

        private void hColorBar_ValueChanged(object sender, EventArgs e)
        {
            Hue = (int)hColorBar.Value;
        }

        private void hNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Hue = (int)hNumericUpDown.Value;
        }

        private void sColorBar_ValueChanged(object sender, EventArgs e)
        {
            Saturation = (int)sColorBar.Value;
        }

        private void sNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Saturation = (int)sNumericUpDown.Value;
        }

        private void lColorBar_ValueChanged(object sender, EventArgs e)
        {
            Brightness = (int)lColorBar.Value;
        }

        private void lNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Brightness = (int)lNumericUpDown.Value;
        }

        private void sceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SceneId = (NeewerSceneId)sceneComboBox.Items[sceneComboBox.SelectedIndex];
        }

        private void whiteModeButton_Click(object sender, EventArgs e)
        {
            _targetDevice.SetWhite(_targetDevice.Temperature, _targetDevice.Brightness);
        }

        private void colorModeButton_Click(object sender, EventArgs e)
        {
            _targetDevice.SetColor(_targetDevice.Hue, _targetDevice.Saturation, _targetDevice.Brightness);
        }

        private void sceneModeButton_Click(object sender, EventArgs e)
        {
            _targetDevice.SetCustomScene(_targetDevice.CustomSceneId, _targetDevice.Brightness);
        }

        private void powerOnButton_Click(object sender, EventArgs e)
        {
            Power = true;
        }

        private void togglePower_Click(object sender, EventArgs e)
        {
            Power = !Power;
        }

        private void powerOffButton_Click(object sender, EventArgs e)
        {
            Power = false;
        }
    }
}
