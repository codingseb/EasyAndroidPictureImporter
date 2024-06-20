using CodingSeb.Mvvm;
using MediaDevices;

namespace EasyAndroidPictureImporter.ViewModel;

public class DirectoryViewModel(MediaDirectoryInfo mediaDirectoryInfo)
    : NotifyPropertyChangedBaseClass
{
    public MediaDirectoryInfo DirectoryInfo { get; } = mediaDirectoryInfo;

    public List<FileViewModel> Files
    {
        get
        {
            return DirectoryInfo
                .EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                .Where(file => !file.Name.StartsWith('.'))
                .Select(fileInfo => new FileViewModel(fileInfo))
                .ToList();
        }
    }

    public FileViewModel SelectedFile { get; set; }
}