using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EasyAndroidPictureImporter.Helpers.Converters;

public class DebugConverter : MarkupExtension, IValueConverter, IMultiValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return [value];
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}