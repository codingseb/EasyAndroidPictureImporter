using CodingSeb.Localization;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace EasyAndroidPictureImporter.ViewModel;

public class DirectoryViewModel(MediaDirectoryInfo mediaDirectoryInfo)
    : ViewModelBase
{
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
        CommandManager.InvalidateRequerySuggested();
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
            .Select(fileInfo => new FileViewModel(fileInfo, this))
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

private ICommand toggleIsCheckOfSelectedFilesCommand;
public ICommand ToggleIsCheckOfSelectedFilesCommand => toggleIsCheckOfSelectedFilesCommand ??= new RelayCommand(_ => ToggleIsCheckOfSelectedFiles());

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

private ICommand checkSelectedFilesCommand;
public ICommand CheckSelectedFilesCommand => checkSelectedFilesCommand ??= new RelayCommand(_ => CheckOfSelectedFiles());

private void CheckOfSelectedFiles()
{
    if (files?.Count > 0)
    {
        foreach (var fileViewModel in files)
        {
            if (fileViewModel.IsSelected)
                fileViewModel.IsChecked = true;
        }
    }
}

private ICommand uncheckSelectedFilesCommand;
public ICommand UncheckSelectedFilesCommand => uncheckSelectedFilesCommand ??= new RelayCommand(_ => UnCheckOfSelectedFiles());

private void UnCheckOfSelectedFiles()
{
    if (files?.Count > 0)
    {
        foreach (var fileViewModel in files)
        {
            if (fileViewModel.IsSelected)
                fileViewModel.IsChecked = false;
        }
    }
}

private ICommand openSelectedFileCommand;
public ICommand OpenSelectedFileCommand => openSelectedFileCommand ??= new RelayCommand(_ => OpenSelectedFile());

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