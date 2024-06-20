using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyAndroidPictureImporter.UI.Components;
/// <summary>
/// Interaction logic for MainWindowCustomTitleBar.xaml
/// </summary>
public partial class MainWindowCustomTitleBar : UserControl
{
    public MainWindowCustomTitleBar()
    {
        InitializeComponent();
    }

    public Window OwnerWindow => App.Current.MainWindow;

    private void MaximizeWindowExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (OwnerWindow.WindowState == WindowState.Maximized)
        {
            SystemCommands.RestoreWindow(OwnerWindow);
        }
        else
        {
            SystemCommands.MaximizeWindow(OwnerWindow);
        }
    }

    private void MinimizeWindowExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        SystemCommands.MinimizeWindow(OwnerWindow);
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var dpi = VisualTreeHelper.GetDpi(this);
        Point position = PointToScreen(Mouse.GetPosition(OwnerWindow));
        position.X /= dpi.DpiScaleX;
        position.Y /= dpi.DpiScaleY;
        SystemCommands.ShowSystemMenu(OwnerWindow, position);
    }
}
