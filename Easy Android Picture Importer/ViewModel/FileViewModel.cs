using CodingSeb.Localization;
using CodingSeb.Mvvm;
using MediaDevices;
using System.Globalization;

namespace EasyAndroidPictureImporter.ViewModel;

public class FileViewModel(MediaFileInfo fileInfo, DirectoryViewModel directory) : ViewModelBase
{
    public DirectoryViewModel Directory { get; } = directory;

    public MediaFileInfo FileInfo { get; } = fileInfo;

    [Localize]
    public string SizeInKB => string.Format(Loc.Tr("SizeInKB"), FileInfo.Length / 1024);

    [Localize]
    public string Modification => FileInfo.LastWriteTime?.ToString(Loc.Tr("DateTimeFormat"), CultureInfo.InvariantCulture);

    public bool IsChecked { get; set; }

    public bool IsSelected { get; set; }

    public void OnIsCheckedChanged()
    {
        Directory?.UpdateIsCheckedCount();
    }
}