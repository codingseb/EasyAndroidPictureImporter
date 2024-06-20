using CodingSeb.Localization;
using CommunityToolkit.Mvvm.Input;
using EasyAndroidPictureImporter.Utils;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// To store the configuration of the app
/// </summary>
public partial class Configuration : NotifyPropertyChangedOnChildsChanges
{
    public Configuration()
    {
        SetDefaultDirectories();
    }

    /// <summary>
    /// The selected UI language
    /// </summary>
    public string SelectedLanguage
    {
        get => Loc.Instance.CurrentLanguage;
        set => Loc.Instance.CurrentLanguage = value;
    }

    /// <summary>
    /// To specify to show thumbnails in the File DataGrid in place of file icon
    /// </summary>
    public bool ShowThumbnailsInPlaceOfIconInGrid { get; set; }

    /// <summary>
    /// The collection of root directories to scan for files
    /// </summary>
    public ObservableCollection<FavoriteDirectoryViewModel> FavoriteDirectories { get; set; }

    [RelayCommand]
    [property: JsonIgnore]
    public void AddNewFavoriteDirectory()
    {
        FavoriteDirectories.Add(new FavoriteDirectoryViewModel(""));
    }

    [RelayCommand]
    [property: JsonIgnore]
    public void RemoveFavoriteDirectory(FavoriteDirectoryViewModel toRemove)
    {
        if (MessageBox.Show(string.Format(Loc.Tr("FavoriteDirectoryRemoveValidationMessage"), toRemove?.RootPath), Globals.APPLICATION_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            FavoriteDirectories.Remove(toRemove);
        }
    }

    private bool askForDefaultDirectories = false;

    [RelayCommand]
    [property: JsonIgnore]
    public void SetDefaultDirectories()
    {
        if (!askForDefaultDirectories || MessageBox.Show(Loc.Tr("AskForRestoreDefaultDirectoriesMessage"), Globals.APPLICATION_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            FavoriteDirectories =
            [
                new ("DCIM", true),
                new ("Download"),
                new ("Pictures"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Audio"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Documents"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Images"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Video"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Video Notes"),
                new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Voice Notes"),
                new (@"Android\media\org.telegram.messenger\Telegram", true),
            ];

            askForDefaultDirectories = true;
        }
    }
}