using EasyAndroidPictureImporter.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyAndroidPictureImporter.UI.Components;
/// <summary>
/// Interaction logic for FileSelectionPanel.xaml
/// </summary>
public partial class FileSelectionPanel : UserControl
{
    public FileSelectionPanel()
    {
        InitializeComponent();
    }

    private void FilesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            foreach(var fileViewModel in dataGrid.Items.Cast<FileViewModel>())
            {
                fileViewModel.IsSelected = false;
            }

            foreach(var fileViewModel in dataGrid.SelectedItems.Cast<FileViewModel>())
            {
                fileViewModel.IsSelected = true;
            }
        }
    }
}
