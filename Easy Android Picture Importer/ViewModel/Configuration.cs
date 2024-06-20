using CodingSeb.Localization;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// To store the configuration of the app
/// </summary>
public class Configuration : ViewModelBase
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
}