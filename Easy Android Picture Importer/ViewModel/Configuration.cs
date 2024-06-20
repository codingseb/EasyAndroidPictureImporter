using CodingSeb.Localization;

namespace EasyAndroidPictureImporter.ViewModel;

public class Configuration : ViewModelBase
{
    public string SelectedLanguage
    {
        get => Loc.Instance.CurrentLanguage;
        set => Loc.Instance.CurrentLanguage = value;
    }
}