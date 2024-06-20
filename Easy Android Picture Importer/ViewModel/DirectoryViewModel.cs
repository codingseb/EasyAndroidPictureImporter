using CodingSeb.Mvvm;
using MediaDevices;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace EasyAndroidPictureImporter.ViewModel;

public class DirectoryViewModel(MediaDirectoryInfo mediaDirectoryInfo)
    : NotifyPropertyChangedBaseClass
{
    public MediaDirectoryInfo DirectoryInfo { get; } = mediaDirectoryInfo;

    private List<FileViewModel> files;

    public List<FileViewModel> Files
    {
        get
        {
            if(files == null)
                ScanForFile();

            return files;
        }
    }

    public async void ScanForFile()
    {
        await Task.Run(async () =>
        {
            files = DirectoryInfo
                .EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                .Where(file => !file.Name.StartsWith('.'))
                .OrderByDescending(file => file.LastWriteTime)
                .Select(fileInfo => new FileViewModel(fileInfo))
                .ToList();

            NotifyPropertyChanged(nameof(Files));
        });
    }

    private bool checkOrUnCheckAllfiles;
    public bool CheckOrUnCheckAllfiles
    {
        get
        {
            return checkOrUnCheckAllfiles;
        }

        set
        {
            checkOrUnCheckAllfiles=value;
            files.ForEach(fileViewModel => fileViewModel.IsChecked = value);
        }
    }

    public FileViewModel SelectedFile { get; set; }

    private ICommand openSelectedFileCommand;
    public ICommand OpenSelectedFileCommand => openSelectedFileCommand ??= new RelayCommand(_ => OpenSelectedFile());

    public void OpenSelectedFile()
    {
        if (SelectedFile == null)
            return;

        string path = Path.Combine(Path.GetTempPath(), SelectedFile.FileInfo.Name);

        try
        {
            SelectedFile.FileInfo.CopyTo(path, true);
        }
        catch { }
        finally
        {
            if(File.Exists(path))
            {
                using Process fileopener = new();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }
}