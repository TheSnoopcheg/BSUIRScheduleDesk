using System;

namespace BSUIRScheduleDESK.Themes;

public enum ThemeType
{
    IIS,
    Dark
}
public static class ThemeTypeExtension
{ 
    public static string GetName(this ThemeType themeType)
    {
        switch (themeType)
        {
            case ThemeType.IIS: return "IISTheme";
            case ThemeType.Dark: return "DarkTheme";
            default: throw new ArgumentOutOfRangeException(nameof(themeType), themeType, null);
        }
    }
}
