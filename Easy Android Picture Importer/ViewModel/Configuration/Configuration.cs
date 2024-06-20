using CodingSeb.Localization;
using System.Collections.ObjectModel;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// To store the configuration of the app
/// </summary>
public class Configuration : NotifyPropertyChangedOnChildsChanges
{
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

    public Configuration()
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
            new (@"Android\media\com.whatsapp\WhatsApp\Media\WhatsApp Voice Notes"),
            new (@"Android\media\org.telegram.messenger\Telegram", true),
        ];
    }
}