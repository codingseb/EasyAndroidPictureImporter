using System.Globalization;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;

namespace EasyAndroidPictureImporter.Helpers.Converters;

public class Pixel2ScreenConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double pixels = (double)value;
        bool horizontal = Equals(parameter, true);

        double points = 0d;

        // NOTE: Ideally, we would get the source from a visual:
        // source = PresentationSource.FromVisual(visual);
        //
        using (var source = new HwndSource(new HwndSourceParameters()))
        {
            var matrix = source.CompositionTarget?.TransformToDevice;
            if (matrix.HasValue)
            {
                points = pixels * (horizontal ? matrix.Value.M11 : matrix.Value.M22);
            }
        }

        return points;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
