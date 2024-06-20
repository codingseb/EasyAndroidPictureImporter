using CodingSeb.Mvvm;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.IO;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// Map a Media Device for the view
/// </summary>
public class DeviceViewModel : NotifyPropertyChangedBaseClass
{
    public DeviceViewModel(MediaDevice device)
    {
        device.Connect();
        Device = device;
    }

    /// <summary>
    /// The mapped MediaDevice
    /// </summary>
    public MediaDevice Device { get; set; }

    public string Name => Device.FriendlyName;

    public string Description => Device.Description;

    public int BatteryLevel => Device.PowerLevel;

    public List<MediaDirectoryInfo> Directories
    {
        get
        {
            List<MediaDirectoryInfo> list = [];

            var root = Device.GetRootDirectory()?.EnumerateDirectories().FirstOrDefault();

            MediaDirectoryInfo dcim = root?.GetDirectoryIfExists("dcim");

            if(dcim != null)
                list.AddRange(dcim.EnumerateDirectories().Where(directory => !directory.Name.StartsWith('.')));

            MediaDirectoryInfo whatsapp = root?.GetDirectoryIfExists(@"Android\media\com.whatsapp\WhatsApp\Media");

            if (whatsapp != null)
            {
                list.AddRange(whatsapp.EnumerateDirectories()
                    .Where(directory => directory.Name.ContainsOneOf(["audio", "images", "video", "documents", "voice"])
                    && !directory.Name.StartsWith('.')));
            }

            return list;
        }
    }

}