using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            FilesListBox.SelectedValue = frameworkElement.DataContext;
        }
    }

    private void CloseWindowExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}