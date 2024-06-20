using EasyAndroidPictureImporter.Interop;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

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

    private const int HTMAXBUTTON = 9;
    private const int HTMINBUTTON = 8;
    private const int HTCLOSE = 20;
    private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        var message = (User32.WM)msg;

        switch (message)
        {
            case User32.WM.NCHITTEST:
                try
                {
                    int x = lparam.ToInt32() & 0xffff;
                    int y = lparam.ToInt32() >> 16;
                    
                    var rect = new Rect(MaximizeWindowButton.PointToScreen(
                        new Point()),
                        new Size(MaximizeWindowButton.ActualWidth, MaximizeWindowButton.ActualWidth));

                    if (rect.Contains(new Point(x, y)))
                    {
                        MaximizeWindowButton.SpecialIsHover = true;
                        handled = true;
                        return new IntPtr(HTMAXBUTTON);
                    }
                    else
                    {
                        MaximizeWindowButton.SpecialIsHover = false;
                    }

                    rect = new Rect(MinimizeButton.PointToScreen(
                        new Point()),
                        new Size(MinimizeButton.ActualWidth, MinimizeButton.ActualWidth));

                    if (rect.Contains(new Point(x, y)))
                    {
                        MinimizeButton.SpecialIsHover = true;
                        handled = true;
                        return new IntPtr(HTMINBUTTON);
                    }
                    else
                    {
                        MinimizeButton.SpecialIsHover = false;
                    }

                    rect = new Rect(ExitButton.PointToScreen(
                        new Point()),
                        new Size(ExitButton.ActualWidth, ExitButton.ActualWidth));

                    if (rect.Contains(new Point(x, y)))
                    {
                        ExitButton.SpecialIsHover = true;
                        handled = true;
                        return new IntPtr(HTCLOSE);
                    }
                    else
                    {
                        ExitButton.SpecialIsHover = false;
                    }
                }
                catch (OverflowException)
                {
                    handled = true;
                }
                break;
        }
        return IntPtr.Zero;
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

    bool firstLoad = true;

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if(firstLoad && !DesignerProperties.GetIsInDesignMode(this))
        {
            var handle = new WindowInteropHelper(OwnerWindow).Handle;
            var source = HwndSource.FromHwnd(handle);
            source?.AddHook(new HwndSourceHook(HwndSourceHook));
            firstLoad = false;
        }
    }
}
