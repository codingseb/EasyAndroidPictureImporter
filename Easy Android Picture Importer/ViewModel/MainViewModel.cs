using CodingSeb.Localization;
using CodingSeb.Mvvm;
using CommunityToolkit.Mvvm.Input;
using EasyAndroidPictureImporter.DependencyInjection;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// The Main ViewModel of the application
/// </summary>
public partial class MainViewModel(MediaDeviceComparer mediaDeviceComparer, Configuration configuration)
    : NotifyPropertyChangedBaseClass, IInitializable
{
    private IEnumerable<MediaDevice> _devices = MediaDevice.GetDevices();
    private readonly MediaDeviceComparer _mediaDeviceComparer = mediaDeviceComparer;
    private readonly Configuration _configuration = configuration;

    /// <inheritdoc/>
    public void Init()
    {
        Devices = new(_devices.Select(device => new DeviceViewModel(device, this, _configuration)));

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

    private bool selectAtTheEnd;
    private async void ScanForNewDevices()
    {
        while (true)
        {
            selectAtTheEnd = SelectedDevice == null;
            await Task.Delay(1000);
            var newDevicesCollection = MediaDevice.GetDevices();

            if (!newDevicesCollection.SequenceEqual(_devices, _mediaDeviceComparer))
            {
                foreach (var device in newDevicesCollection.Except(_devices, _mediaDeviceComparer))
                {
                    Devices.Add(new DeviceViewModel(device, this, _configuration));
                }

                foreach (var device in _devices.Except(newDevicesCollection, _mediaDeviceComparer))
                {
                    Devices.Remove(Devices.FirstOrDefault(vm => vm.Device.DeviceId.Equals(device.DeviceId)));
                }

                _devices = newDevicesCollection;

                if(selectAtTheEnd && Devices.Any())
                    SelectedDevice = Devices.First();
                else if(!Devices.Contains(SelectedDevice))
                    SelectedDevice = null;
            }
        }
    }

    public bool IsImporting { get; set; }

    public string ImportState { get; set; }

    public int ImportProgress { get; set; }
    public int ImportMax { get; set; }

    private bool stopImporting = false;

    [RelayCommand]
    public void ReloadDirectories()
    {
        foreach( var device in Devices)
        {
            device.ResetDirectories();
        }

        SelectedDevice.NotifyPropertyChanged(nameof(DeviceViewModel.Directories));
    }

    [RelayCommand]
    public void CancelImport()
    {
        stopImporting = true;
    }

    [RelayCommand]
    public async void ImportCheckedFiles()
    {
        IsImporting = true;
        stopImporting = false;
        try
        {
            var files = SelectedDevice?.SelectedDirectory?.Files;

            if (files?.Count > 0)
            {
                var filesToCopy = files.FindAll(f => f.IsChecked);
                ImportProgress = 0;
                ImportMax = filesToCopy.Count;
                ImportState = string.Format(Loc.Tr("ImportState"), 0, filesToCopy.Count);

                var folderDialog = new OpenFolderDialog();

                if (folderDialog.ShowDialog(App.Current.MainWindow) == true)
                {
                    var folderName = folderDialog.FolderName;

                    await Task.Run(async () =>
                    {
                        for (int i = 0; i < filesToCopy.Count; i++)
                        {
                            var fileViewModel = filesToCopy[i];

                            fileViewModel.FileInfo.CopyTo(Path.Combine(folderName, fileViewModel.FileInfo.Name), true);

                            if (stopImporting)
                                return;

                            ImportProgress = i + 1;
                            ImportState = string.Format(Loc.Tr("ImportState"), ImportProgress, ImportMax);
                            await Task.Delay(1);
                        }
                    });
                }
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, Loc.Tr("ImportErrorMessage"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsImporting = false;
        }
    }
}