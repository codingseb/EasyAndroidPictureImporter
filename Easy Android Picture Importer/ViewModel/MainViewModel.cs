using CodingSeb.Mvvm;
using EasyAndroidPictureImporter.DependencyInjection;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.Collections.ObjectModel;
using System.IO;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// The Main ViewModel of the application
/// </summary>
public class MainViewModel(MediaDeviceComparer mediaDeviceComparer)
    : NotifyPropertyChangedBaseClass, IInitializable
{
    private IEnumerable<MediaDevice> _devices = MediaDevice.GetDevices();
    private readonly MediaDeviceComparer _mediaDeviceComparer = mediaDeviceComparer;

    public void Init()
    {
        Devices = new(_devices.Select(device => new DeviceViewModel(device, this)));

        ScanForNewDevices();
    }

    /// <summary>
    /// The list of all connected devices
    /// </summary>
    public ObservableCollection<DeviceViewModel> Devices { get; set; }

    /// <summary>
    /// The selected device
    /// </summary>
    public DeviceViewModel SelectedDevice { get; set; }

    private async void ScanForNewDevices()
    {
        while (true)
        {
            await Task.Delay(1000);
            var newDevicesCollection = MediaDevice.GetDevices();

            if (!newDevicesCollection.SequenceEqual(_devices, _mediaDeviceComparer))
            {
                foreach (var device in newDevicesCollection.Except(_devices, _mediaDeviceComparer))
                {
                    Devices.Add(new DeviceViewModel(device, this));
                }

                foreach (var device in _devices.Except(newDevicesCollection, _mediaDeviceComparer))
                {
                    Devices.Remove(Devices.FirstOrDefault(vm => vm.Device.DeviceId.Equals(device.DeviceId)));
                }

                _devices = newDevicesCollection;
            }
        }
    }
}