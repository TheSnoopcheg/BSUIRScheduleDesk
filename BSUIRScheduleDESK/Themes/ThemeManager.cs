using BSUIRScheduleDESK.classes;
using System.Windows;

namespace BSUIRScheduleDESK.Themes
{
    public static class ThemeManager
    {
        public static void SetTheme(ThemeType theme)
        {
            string themeName = theme.GetName();
            if (string.IsNullOrEmpty(themeName))
            {
                return;
            }
            if (themeName == Config.Instance.CurrentTheme)
            {
                return;
            }
            App.Current.Resources.MergedDictionaries.RemoveAt(0);
            App.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary() { Source = new System.Uri($"Themes/ColourDictionaries/{themeName}.xaml", System.UriKind.Relative) });
            Config.Instance.CurrentTheme = themeName;
            Config.Instance.Save();
        }
    }
}
