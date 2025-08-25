using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters;

public class StringsCollectionConverter : IValueConverter
{
    public static readonly StringsCollectionConverter Instance = new StringsCollectionConverter();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter is not string sparam) return null;
        if(sparam == "Auditories")
        {
            List<string>? values = value as List<string>;
            return string.Join("\n", values!);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
