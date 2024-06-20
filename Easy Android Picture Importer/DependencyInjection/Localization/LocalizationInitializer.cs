using CodingSeb.Localization;
using CodingSeb.Localization.Loaders;
using System.IO;

namespace EasyAndroidPictureImporter.DependencyInjection.Localization;

public class LocalizationInitializer : IInitializable
{
    public void Init()
    {
        LocalizationLoader.Instance.FileLanguageLoaders.Add(new JsonFileLoader());
        LocalizationLoader.Instance.AddDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lang"));

        var translatorProxy = new SubLanguageTranslatorProxy(Loc.Instance.Translators.ToList());
        Loc.Instance.Translators.Clear();
        Loc.Instance.Translators.Add(translatorProxy);
    }
}