using CodingSeb.Mvvm;
using MediaDevices;

namespace EasyAndroidPictureImporter.ViewModel;

public class FileViewModel(MediaFileInfo fileInfo)
    : NotifyPropertyChangedBaseClass
{
    public MediaFileInfo FileInfo { get; } = fileInfo;

    public bool IsChecked { get; set; }
}