using CodingSeb.Localization;
using EasyAndroidPictureImporter.Utils;
using MediaDevices;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EasyAndroidPictureImporter.ViewModel;

public class FileViewModel(MediaFileInfo fileInfo, DirectoryViewModel directory, Configuration configuration)
    : ViewModelBase
{
    private readonly Configuration _configuration = configuration;

    public DirectoryViewModel Directory { get; } = directory;

    public MediaFileInfo FileInfo { get; } = fileInfo;

    [Localize]
    public string SizeInKB => string.Format(Loc.Tr("SizeInKB"), FileInfo.Length / 1024);

    [Localize]
    public string Modification => FileInfo.LastWriteTime?.ToString(Loc.Tr("DateTimeFormat"), CultureInfo.InvariantCulture);

    public bool IsChecked { get; set; }

    public bool IsSelected { get; set; }

    public void OnIsSelectedChanged()
    {
        Directory.NotifyPropertyChanged(nameof(DirectoryViewModel.IsSelectedFilesCount));
        Directory.NotifyPropertyChanged(nameof(DirectoryViewModel.SelectedFilesSizeSum));
    }

    public void OnIsCheckedChanged()
    {
        Directory?.UpdateIsCheckedCount();
    }

    public ImageSource Icon { get; set; }
    public ImageSource Thumbnail { get; set; }

    public ImageSource ThumbnailOrIcon
    {
        get
        {
            bool isThumbnail = _configuration.ShowThumbnailsInPlaceOfIconInGrid;

            Icon ??= IconManager.FindIconForFilename(FileInfo.Name, false);

            if (Thumbnail == null && isThumbnail)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ScanForThumbnailAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return isThumbnail
                ? Thumbnail ?? Icon
                : Icon;
        }
    }

    public async Task ScanForThumbnailAsync()
    {
        await Task.Run(async () =>
        {
            System.IO.Directory.CreateDirectory(PathUtils.TempPath);

            string thumbnailPath = Path.Combine(PathUtils.TempPath, $"Thumbnail_{FileInfo.Name}");

            for (int retry = 3; retry > 0; retry--)
            {
                try
                {
                    await Task.Delay(1);

                    if (!File.Exists(thumbnailPath))
                        FileInfo.CopyThumbnail(thumbnailPath, true);

                    retry=0;
                }
                catch
                {
                    retry--;
                    await Task.Delay(10);
                }
            }

            await Task.Delay(1);

            if (new FileInfo(thumbnailPath).Length > 0)
            {
                var bmp = new BitmapImage();

                try
                {
                    using (var stream = new FileStream(thumbnailPath, FileMode.Open))
                    {
                        bmp.BeginInit();
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.StreamSource = stream;
                        bmp.EndInit();
                        bmp.Freeze();
                    }
                    await Task.Delay(1);

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                    Thumbnail = bmp;
                }
                catch{}
            }

            await Task.Delay(1);
        });
    }
}