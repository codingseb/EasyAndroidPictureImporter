using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.Windows;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// Map a Media Device for the view
/// </summary>
public class DeviceViewModel
    : ViewModelBase
{
    private readonly MainViewModel _container;
    private readonly Configuration _configuration;

    public DeviceViewModel(MediaDevice device, MainViewModel container, Configuration configuration)
    {
        _container = container;
        _configuration = configuration;
        device.Connect();
        Device = device;
        Device.DeviceRemoved += Device_DeviceRemoved;
    }

    /// <summary>
    /// The mapped MediaDevice
    /// </summary>
    public MediaDevice Device { get; set; }

    public string Name => Device.FriendlyName;

    public string Description => Device.Description;

    public int BatteryLevel => Device.PowerLevel;

    private List<DirectoryViewModel> directories;
    public List<DirectoryViewModel> Directories
    {
        get
        {
            if(directories == null)
                ScanForDirectories();

            return directories;
        }
    }

    public async void ScanForDirectories()
    {
        await Task.Run(async () =>
        {
            try
            {
                List<MediaDirectoryInfo> list = [];

                var root = Device.GetRootDirectory()?.EnumerateDirectories().FirstOrDefault();

                MediaDirectoryInfo dcim = root?.GetDirectoryIfExists("dcim");

                if (dcim != null)
                {
                    list.AddRange(dcim.EnumerateDirectories()
                        .Where(directory => !directory.Name.StartsWith('.')
                        && directory.EnumerateFileSystemInfos().Any()));
                }

                MediaDirectoryInfo download = root?.GetDirectoryIfExists("download");

                if (download?.EnumerateFileSystemInfos().Any() == true)
                {
                    list.Add(download);
                }

                MediaDirectoryInfo whatsapp = root?.GetDirectoryIfExists(@"Android\media\com.whatsapp\WhatsApp\Media");

                if (whatsapp != null)
                {
                    list.AddRange(whatsapp.EnumerateDirectories()
                        .Where(directory => directory.Name.ContainsOneOf(["audio", "images", "video", "documents", "voice"])
                        && !directory.Name.StartsWith('.')));
                }

                directories = list
                    .OrderBy(directory => directory.Name)
                    .Select(directory => new DirectoryViewModel(directory, _configuration))
                    .ToList();
            }
            catch {}
            finally
            {
                await Task.Delay(10);
                if(directories != null)
                    NotifyPropertyChanged(nameof(Directories));
                SelectedDirectory = Directories?.FirstOrDefault();
            }
        });
    }

    public DirectoryViewModel SelectedDirectory { get; set; }

    private void Device_DeviceRemoved(object sender, MediaDeviceEventArgs e)
    {
        Device.DeviceRemoved -= Device_DeviceRemoved;

        App.Current.Dispatcher.Invoke(() => _container.Devices.Remove(this));
    }
}