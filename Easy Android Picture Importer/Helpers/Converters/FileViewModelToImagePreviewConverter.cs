using EasyAndroidPictureImporter.Utils;
using EasyAndroidPictureImporter.ViewModel;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EasyAndroidPictureImporter.Helpers.Converters;

/// <summary>
/// Convert a FileViewModel to the corresponding downloaded preview
/// </summary>
public class FileViewModelToImagePreviewConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is FileViewModel fileViewModel)
        {
            Directory.CreateDirectory(PathUtils.TempPath);

            string thumbnailPath = Path.Combine(PathUtils.TempPath, $"Thumbnail_{fileViewModel.FileInfo.Name}");

            try
            {
                if(!File.Exists(thumbnailPath))
                    fileViewModel.FileInfo.CopyThumbnail(thumbnailPath, true);
            }
            catch { }

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
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                }
                catch
                {
                    return thumbnailPath;
                }

                return bmp;
            }
            else
            {
                File.Delete(thumbnailPath);
                return IconManager.FindIconForFilename(fileViewModel.FileInfo.Name, true);
            }
        }
        else
        {
            throw new ArgumentException($"Should be a {nameof(FileViewModel)}", nameof(value));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}