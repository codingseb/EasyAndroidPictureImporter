using EasyAndroidPictureImporter.Interop;
using EasyAndroidPictureImporter.Utils;
using EasyAndroidPictureImporter.ViewModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace EasyAndroidPictureImporter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();

        Title += " - " + version;
    }

    private void DockPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if(sender is FrameworkElement frameworkElement)
        {
            FilesDataGrid.SelectedValue = frameworkElement.DataContext;
        }
    }

    private void CloseWindowExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        ThumbnailImage.Source = null;

        try
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        catch { }

        foreach (string oldFiles in Directory.GetFiles(PathUtils.TempPath))
        {
            try
            {
                File.Delete(oldFiles);
            }
            catch { }
        }

        try
        {
            Directory.Delete(PathUtils.TempPath, true);
        }
        catch { }
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter
            && e.KeyboardDevice.Modifiers == ModifierKeys.Control
            && DataContext is MainViewModel viewModel)
        {
            if (viewModel.ImportCheckedFilesCommand.CanExecute(this))
                viewModel.ImportCheckedFilesCommand.Execute(this);

            e.Handled = true;
        }
    }
}