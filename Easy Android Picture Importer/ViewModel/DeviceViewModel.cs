using EasyAndroidPictureImporter.Utils;
using MediaDevices;

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

    public void ResetDirectories()
    {
        directories = null;
    }

    public async void ScanForDirectories()
    {
        await Task.Run(async () =>
        {
            try
            {
                List<MediaDirectoryInfo> list = [];

                var root = Device.GetRootDirectory()?.EnumerateDirectories().FirstOrDefault();

                foreach(var favoriteDirectory in _configuration.FavoriteDirectories)
                {
                    if (favoriteDirectory.Show)
                    {
                        MediaDirectoryInfo mediaDirectoryInfo = root?.GetDirectoryIfExists(favoriteDirectory.RootPath);

                        if (mediaDirectoryInfo != null)
                        {
                            if (favoriteDirectory.AddEachChildAsFavorite)
                            {
                                list.AddRange(mediaDirectoryInfo.EnumerateDirectories()
                                    .Where(directory => !directory.Name.StartsWith('.')
                                        && directory.EnumerateFileSystemInfos().Any()));
                            }
                            else if (mediaDirectoryInfo.EnumerateFileSystemInfos().Any())
                            {
                                list.Add(mediaDirectoryInfo);
                            }
                        }
                    }
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