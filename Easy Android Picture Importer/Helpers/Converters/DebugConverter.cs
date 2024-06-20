using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EasyAndroidPictureImporter.Helpers.Converters;

/// <summary>
/// Only used in dev to debug a binding
/// Put a break point int the method to debug
/// </summary>
public class DebugConverter : MarkupExtension, IValueConverter, IMultiValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    /// <inheritdoc/>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    /// <inheritdoc/>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return [value];
    }

    /// <inheritdoc/>
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}