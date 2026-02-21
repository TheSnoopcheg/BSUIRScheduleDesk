using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters;

public class HtmlTextConverter : IValueConverter
{
    public static readonly HtmlTextConverter Instance = new HtmlTextConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string text) return value;
        text.Insert(0, "<div>");
        text += "</div>";
        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
