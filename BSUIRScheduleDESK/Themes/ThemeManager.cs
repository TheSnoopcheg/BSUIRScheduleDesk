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
            if(themeName == Properties.Settings.Default.currentTheme)
            {
                return;
            }
            App.Current.Resources.MergedDictionaries.RemoveAt(0);
            App.Current.Resources.MergedDictionaries.Insert(0, new ResourceDictionary() { Source = new System.Uri($"Themes/ColourDictionaries/{themeName}.xaml", System.UriKind.Relative) });
            Properties.Settings.Default.currentTheme = themeName;
            Properties.Settings.Default.Save();
        }
    }
}
