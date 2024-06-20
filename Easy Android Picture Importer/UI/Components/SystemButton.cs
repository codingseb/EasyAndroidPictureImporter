using System.Windows;
using System.Windows.Controls;

namespace EasyAndroidPictureImporter.UI.Components;
public class SystemButton : Button
{
    public bool SpecialIsHover
    {
        get { return (bool)GetValue(SpecialIsHoverProperty); }
        set { SetValue(SpecialIsHoverProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SpecialIsHover.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SpecialIsHoverProperty =
        DependencyProperty.Register("SpecialIsHover", typeof(bool), typeof(SystemButton), new PropertyMetadata(false));
}
