using System.ComponentModel;
using System.Runtime.InteropServices;
using MothManager.Core.DeviceControl;
using MothManager.Core.Logger;
using MothManager.NeewerLEDControl;
using MothManagerTrayApp.Controls;

namespace MothManagerTrayApp;

public partial class Form1 : Form
{
    private BackgroundWorker _scanBackgroundWorker;
    private readonly NeewerLedDeviceManager _neewerLedDeviceManager;
    private Dictionary<string, DiscoveredDeviceInfoBase> _discoveredDevices;
    private DeviceUserSettings _deviceUserSettings;

    public event Action OnSaveSettings;
    public event Action OnLoadSettings;
    
    public Form1(NeewerLedDeviceManager neewerLedDeviceManager)
    {
        _deviceUserSettings = new DeviceUserSettings();
        _neewerLedDeviceManager = neewerLedDeviceManager;
        
        InitializeComponent();
        _neewerLedDeviceManager.OnDeviceAdded += OnDeviceAdded;
        showToolStripMenuItem.Click += showToolStripMenuItem_Click;
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
    }

    private bool allowVisible;     // ContextMenu's Show command used
    private bool allowClose;       // ContextMenu's Exit command used

    private void Form1_Load(object sender, EventArgs e)
    {

    }



    protected override void SetVisibleCore(bool value)
    {
        if (!allowVisible)
        {
            value = false;
            if (!this.IsHandleCreated) CreateHandle();
        }
        base.SetVisibleCore(value);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!allowClose)
        {
            this.Hide();
            OnSaveSettings.Invoke();
            e.Cancel = true;
        }
        base.OnFormClosing(e);
    }

    private void showToolStripMenuItem_Click(object sender, EventArgs e)
    {
        allowVisible = true;
        Show();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        allowClose = true;
        Application.Exit();
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        allowVisible = true;
        Show();
    }

    private void scanButton_Click(object sender, EventArgs e)
    {
        scanButton.Enabled = false;
        addDiscoveredButton.Enabled = false;

        if (_scanBackgroundWorker != null)
        {
            return;
        }

        _scanBackgroundWorker = new BackgroundWorker();
        
        _scanBackgroundWorker.DoWork += ScanBackgroundWorker_DoWork;
        _scanBackgroundWorker.RunWorkerCompleted += ScanBackgroundWorker_Complete;
        
        _scanBackgroundWorker.RunWorkerAsync();
    }

    private void ScanBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        _discoveredDevices = _neewerLedDeviceManager.DiscoverDevices();
    }

    private void ScanBackgroundWorker_Complete(object? sender, RunWorkerCompletedEventArgs e)
    {
        scanButton.Enabled = true;
        addDiscoveredButton.Enabled = true;
        
        discoveredDeviceListBox.Items.Clear();
        discoveredDeviceListBox.Items.AddRange(_discoveredDevices.Values.ToArray());
    }

    private void addDiscoveredButton_Click(object sender, EventArgs e)
    {
        foreach (DiscoveredDeviceInfoBase deviceInfo in discoveredDeviceListBox.SelectedItems)
        {
            _neewerLedDeviceManager.ConnectDevice(deviceInfo, _deviceUserSettings.DeviceSettings);
        }
    }

    private void discoveredDeviceListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        addDiscoveredButton.Enabled = (discoveredDeviceListBox.SelectedIndices.Count > 0);
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {

    }

    public void SetDeviceUserSettings(DeviceUserSettings settings)
    {
        _deviceUserSettings = settings;
        _neewerLedDeviceManager.UpdateKnownDeviceSettings(settings.DeviceSettings, _deviceUserSettings.AutoConnectOnLoad);
    }
    
    public DeviceUserSettings GetDeviceUserSettings()
    {
        _deviceUserSettings.DeviceSettings = _neewerLedDeviceManager.GetKnownDeviceSettings().ToList();
        return _deviceUserSettings;
    }

    private void loadSettingsButton_Click(object sender, EventArgs e)
    {
        OnLoadSettings.Invoke();
    }

    private void saveSettingsButton_Click(object sender, EventArgs e)
    {
        OnSaveSettings.Invoke();
    }

    private void OnDeviceAdded(DeviceBase device)
    {
        this.InvokeOnUIThread(() => AddDeviceUI(device));
    }

    private void AddDeviceUI(DeviceBase device)
    {
        if (device is NeewerLedDevice neewerLedDevice)
        {
            knownDevicePanel.Controls.Add(new NeewerLedPanelSettings(neewerLedDevice));
        }
    }

    public void Startup()
    {
        if (_deviceUserSettings.AutoConnectOnLoad)
        {
            foreach (var device in _neewerLedDeviceManager.GetKnownDevices())
            {
                device.Connect(_deviceUserSettings.ConnectAttemptsAllowed);
            }
        }
    }
}

public static class WinFormsExtensions
{
    /// <summary>
    /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="code"></param>
    public static void InvokeOnUIThread(this Control @this, Action code)
    {
        if (@this.InvokeRequired)
        {
            @this.BeginInvoke(code);
        }
        else
        {
            code.Invoke();
        }
    }
}