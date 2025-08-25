using System;

namespace BSUIRScheduleDESK.Langs;

public enum LanguageType
{
    RUS,
    BEL,
    ENG
}
public static class LanguageTypeExtension
{
    public static string GetIetfLanguageTag(this LanguageType languageType)
    {
        switch (languageType)
        {
            case LanguageType.RUS: return "ru";
            case LanguageType.BEL: return "be";
            case LanguageType.ENG: return "en";
            default: throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null);
        }
    }
}
