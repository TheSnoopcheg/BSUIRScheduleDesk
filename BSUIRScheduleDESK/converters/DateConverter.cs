using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if(value is string str)
            {
                if (DateTime.TryParse(str, out DateTime dt))
                    return dt.ToString("d", CultureInfo.CurrentUICulture) + dt.ToString(", ddd", CultureInfo.CurrentUICulture);
                else
                    return string.Empty;
            }
            else
            {
                string? param = parameter as string;
                DateTime dateTime = (DateTime)value;
                string day = dateTime.ToString("dddd", CultureInfo.CurrentUICulture);
                day = Char.ToUpper(day[0]) + day.Substring(1);
                string result = string.Empty;
                if(param != null)
                {
                    if(param == "linear")
                    {
                        result = day + dateTime.ToString(" dd.MM.yyyy", CultureInfo.CurrentUICulture);
                    }
                    if (param == "monthyear")
                    {
                        string month = dateTime.ToString("MMMM yyyy", CultureInfo.CurrentUICulture);
                        month = char.ToUpper(month[0]) + month.Substring(1);
                        return month;
                    }
                }
                else
                {
                    result = day + "\n" + dateTime.ToString("dd.MM", CultureInfo.CurrentUICulture);
                }
                if (dateTime == DateTime.Today)
                {
                    result += $" ({Langs.Language.Today})";
                }
                else if (dateTime == DateTime.Today.AddDays(1))
                {
                    result += $" ({Langs.Language.Tomorrow})";
                }
                return result;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
