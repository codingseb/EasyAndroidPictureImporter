using Autofac.Core;
using CodingSeb.Mvvm;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.IO;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// Map a Media Device for the view
/// </summary>
public class DeviceViewModel
    : NotifyPropertyChangedBaseClass
{
    private readonly MainViewModel _container;

    public DeviceViewModel(MediaDevice device, MainViewModel container)
    {
        _container = container;
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

    public List<DirectoryViewModel> Directories
    {
        get
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

            MediaDirectoryInfo whatsapp = root?.GetDirectoryIfExists(@"Android\media\com.whatsapp\WhatsApp\Media");

            if (whatsapp != null)
            {
                list.AddRange(whatsapp.EnumerateDirectories()
                    .Where(directory => directory.Name.ContainsOneOf(["audio", "images", "video", "documents", "voice"])
                    && !directory.Name.StartsWith('.')));
            }

            var result = list
                .OrderBy(directory => directory.Name)
                .Select(directory => new DirectoryViewModel(directory))
                .ToList();

            SelectedDirectory = result.FirstOrDefault();

            return result;
        }
    }

    public DirectoryViewModel SelectedDirectory { get; set; }

    private void Device_DeviceRemoved(object sender, MediaDeviceEventArgs e)
    {
        Device.DeviceRemoved -= Device_DeviceRemoved;
        _container.Devices.Remove(this);
    }
}