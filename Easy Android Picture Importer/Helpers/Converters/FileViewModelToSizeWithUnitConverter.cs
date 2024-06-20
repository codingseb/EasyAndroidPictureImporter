using EasyAndroidPictureImporter.ViewModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace EasyAndroidPictureImporter.Helpers.Converters;

/// <summary>
/// Convert a FileViewModel to the corresponding downloaded icon
/// </summary>
public class FileViewModelToSizeWithUnitConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is FileViewModel fileViewModel)
        {
            if (fileViewModel.FileInfo.Length == 0)
                return "0 Kb";
            else
                return $"{Math.Max(1, fileViewModel.FileInfo.Length / 1024)} Kb";
        }
        else
        {
            throw new ArgumentException($"Should be a {nameof(FileViewModel)}", nameof(value));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}