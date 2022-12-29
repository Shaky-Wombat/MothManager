using System.Diagnostics;
using MothManager.Core.Logger;
using MothManager.NeewerLEDControl;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace MothManagerTrayApp;

static class Program
{
    

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MothManagerApplicationContext());

    }
}

class MothManagerApplicationContext : ApplicationContext
{
    private static NeewerLedDeviceManager _neewerManager;
    private static DeviceUserSettings? _userSettings;
    private FileStream _userData;

    private static readonly string FileName = Application.UserAppDataPath + "\\devices.json";
    private readonly Form1 _form;

    public MothManagerApplicationContext()
    {
        AllocConsole();
        Logger.SetLogger(new ConsoleLogger());
        
        _neewerManager = new NeewerLedDeviceManager();

        try
        {
            _neewerManager.Initialize();
            _form = new Form1(_neewerManager);
            _form.OnLoadSettings += LoadSettings;
            _form.OnSaveSettings += SaveSettings;
            
            LoadSettings();
            _form.Startup();

            Application.ApplicationExit += OnAppExit;
        }
        catch(Exception e)
        {
            _neewerManager?.Cleanup();
            MessageBox.Show($"EXCEPTION!\n\n{e}");
            
            throw;
        }
    }
    
    
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

    private void LoadSettings()
    {
        
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        var serializer = JsonSerializer.Create(settings);

        using var sw = new StreamReader(FileName);
        using var reader = new JsonTextReader(sw);
        _form.SetDeviceUserSettings(serializer.Deserialize<DeviceUserSettings>(reader) ?? new DeviceUserSettings());
    }

    private void SaveSettings()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        var serializer = JsonSerializer.Create(settings);

        using var sw = new StreamWriter(FileName);
        using JsonWriter writer = new JsonTextWriter(sw);
        var deviceUserSettings = _form.GetDeviceUserSettings();
        serializer.Serialize(writer, deviceUserSettings);
        
        Logger.WriteLine(deviceUserSettings.DeviceSettings.Count + " DEVICE SETTINGS!");
    }

    private void OnAppExit(object? sender, EventArgs e)
    {
        _neewerManager?.Cleanup();
    }
}