using CodingSeb.Localization;
using CodingSeb.Localization.Translators;

namespace EasyAndroidPictureImporter.DependencyInjection.Localization;

public class SubLanguageTranslatorProxy(List<ITranslator> translators) : ITranslator
{
    private List<ITranslator> _translators = translators;

    public bool CanTranslate(string textId, string languageId)
    {
        return _translators.Any(tr => tr.CanTranslate(textId, languageId) || tr.CanTranslate(textId, languageId[..2]));
    }

    public string Translate(string textId, string languageId)
    {
        foreach (var tr in _translators)
        {
            if(tr.CanTranslate(textId,languageId))
                return tr.Translate(textId,languageId);
            else
                return tr.Translate(textId, languageId[..2]);
        }

        return textId;
    }
}