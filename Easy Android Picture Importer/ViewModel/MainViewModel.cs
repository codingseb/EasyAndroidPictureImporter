﻿using CodingSeb.Localization;
using CodingSeb.Mvvm;
using EasyAndroidPictureImporter.DependencyInjection;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

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
                    Devices.Add(new DeviceViewModel(device, this));
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

    private bool stopImporting = false;
    private ICommand importCheckedFilesCommand;
    public ICommand ImportCheckedFilesCommand => importCheckedFilesCommand ??= new RelayCommand(_ => ImportCheckedFiles());

    public ICommand CancelImportCommand => new RelayCommand(_ => this.stopImporting = true);

    private async void ImportCheckedFiles()
    {
        IsImporting = true;
        stopImporting = false;
        try
        {
            var files = SelectedDevice?.SelectedDirectory?.Files;

            if (files?.Count > 0)
            {
                var filesToCopy = files.FindAll(f => f.IsChecked);
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

                            ImportState = string.Format(Loc.Tr("ImportState"), i + 1, filesToCopy.Count);
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