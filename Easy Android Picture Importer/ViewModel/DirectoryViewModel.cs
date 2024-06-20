using CodingSeb.Localization;
using CommunityToolkit.Mvvm.Input;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.Diagnostics;
using System.IO;

namespace EasyAndroidPictureImporter.ViewModel;

public partial class DirectoryViewModel(MediaDirectoryInfo mediaDirectoryInfo, Configuration configuration)
    : ViewModelBase
{
    private readonly Configuration _configuration = configuration;

    public MediaDirectoryInfo DirectoryInfo { get; } = mediaDirectoryInfo;

    private List<FileViewModel> files;

    public List<FileViewModel> Files
    {
        get
        {
            if (files == null)
                ScanForFile();

            return files;
        }
    }

    public int FilesCount => Files?.Count ?? 0;

    public int IsCheckedFilesCount => Files?.Count(f => f.IsChecked) ?? 0;

    public int IsSelectedFilesCount => SelectedFiles?.Count(f => f.IsSelected) ?? 0;

    public List<FileViewModel> SelectedFiles { get; set; }

    [Localize]
    public string SelectedFilesSizeSum
    {
        get
        {
            string label = "SizeInBytes";
            decimal decimalSize= 0m;

            Int64 size = SelectedFiles?.Sum(f => (Int64)f.FileInfo.Length) ?? 0;

            if(size > 1024)
            {
                label = "SizeInKB";
                decimalSize = size / 1024m;
                size /= 1024;
            }

            if(size > 1024)
            {
                label = "SizeInMB";
                decimalSize = size / 1024m;
                size /= 1024;
            }

            if(size > 1024)
            {
                label = "SizeInGB";
                decimalSize = size / 1024m;
                size /= 1024;
            }

            if(size > 1024)
            {
                label = "SizeInTB";
                decimalSize = size / 1024m;
            }

            return string.Format(Loc.Tr(label), decimalSize.ToString("0.##"));
        }
    }

    bool notifyIsChecked = true;
    public void UpdateIsCheckedCount()
    {
        if (notifyIsChecked)
        {
            NotifyPropertyChanged(nameof(IsCheckedFilesCount));
        }
    }

    public bool IsScanning { get; set; }
    public async void ScanForFile()
    {
        if (IsScanning) return;

        IsScanning = true;

        await Task.Run(async () =>
        {
            files = DirectoryInfo
                .EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                .Where(file => file.Name?.StartsWith('.') == false)
                .OrderByDescending(file => file.LastWriteTime)
                .Select(fileInfo => new FileViewModel(fileInfo, this, _configuration))
                .ToList();

            await Task.Delay(10);

            NotifyPropertyChanged(nameof(Files));
            NotifyPropertyChanged(nameof(FilesCount));
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
            notifyIsChecked = false;
            files.ForEach(fileViewModel => fileViewModel.IsChecked = value);
            notifyIsChecked = true;
            NotifyPropertyChanged(nameof(IsCheckedFilesCount));
        }
    }

    public FileViewModel SelectedFile { get; set; }

    [RelayCommand]
    private void ToggleIsCheckOfSelectedFiles()
    {
        if (files?.Count > 0)
        {
            foreach (var fileViewModel in files)
            {
                if (fileViewModel.IsSelected)
                    fileViewModel.IsChecked = !fileViewModel.IsChecked;
            }
        }
    }

    [RelayCommand]
    private void CheckSelectedFiles()
    {
        if (files?.Count > 0)
        {
            foreach (var fileViewModel in SelectedFiles)
            {
                fileViewModel.IsChecked = true;
            }
        }
    }

    [RelayCommand]
    private void UncheckSelectedFiles()
    {
        if (files?.Count > 0)
        {
            foreach (var fileViewModel in SelectedFiles)
            {
                fileViewModel.IsChecked = false;
            }
        }
    }

    [RelayCommand]
    public void OpenSelectedFile()
    {
        if (SelectedFile == null)
            return;

        Directory.CreateDirectory(PathUtils.TempPath);

        string path = Path.Combine(PathUtils.TempPath, SelectedFile.FileInfo.Name);

        try
        {
            SelectedFile.FileInfo.CopyTo(path, true);
        }
        catch { }
        finally
        {
            if (File.Exists(path))
            {
                using Process fileopener = new();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }
}