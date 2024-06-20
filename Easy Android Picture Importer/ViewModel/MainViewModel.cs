using CodingSeb.Mvvm;
using MediaDevices;
using System.Collections.ObjectModel;
using System.IO;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// The Main ViewModel of the application
/// </summary>
public class MainViewModel : NotifyPropertyChangedBaseClass
{
    public MainViewModel()
    {
        AndroidDevices = MediaDevice
            .GetDevices()
            .Select(device => new DeviceViewModel(device))
            .ToList();
    }

    /// <summary>
    /// The list of all connected devices
    /// </summary>
    public List<DeviceViewModel> AndroidDevices { get; set; }

    /// <summary>
    /// The selected device
    /// </summary>
    public DeviceViewModel SelectedDevice { get; set; }
}