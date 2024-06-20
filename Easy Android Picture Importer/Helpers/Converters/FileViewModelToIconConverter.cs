using EasyAndroidPictureImporter.Utils;
using EasyAndroidPictureImporter.ViewModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace EasyAndroidPictureImporter.Helpers.Converters;

/// <summary>
/// Convert a FileViewModel to the corresponding downloaded preview
/// </summary>
public class FileViewModelToIconConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is FileViewModel fileViewModel)
        {
            return IconManager.FindIconForFilename(fileViewModel.FileInfo.Name, false);
        }
        else
        {
            throw new ArgumentException($"Should be a {nameof(FileViewModel)}", nameof(value));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}