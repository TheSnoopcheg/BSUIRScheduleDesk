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
                if(DateTime.TryParse(str, out DateTime dt))
                    return dt.ToString("d") + dt.ToString(", ddd");
                else
                    return string.Empty;
            }
            else
            {
                string? param = parameter as string;
                DateTime dateTime = (DateTime)value;
                string day = dateTime.ToString("dddd");
                day = day.Replace(day[0], Char.ToUpper(day[0]));
                string result = string.Empty;
                if(param != null)
                {
                    if(param == "linear")
                    {
                        result = day + dateTime.ToString(" dd.MM.yyyy");
                    }
                    if (param == "monthyear")
                    {
                        string month = dateTime.ToString("MMMM yyyy");
                        month = month.Replace(month[0], char.ToUpper(month[0]));
                        return month;
                    }
                }
                else
                {
                    result = day + "\n" + dateTime.ToString("dd.MM");
                }
                if (dateTime == DateTime.Today)
                {
                    result += " (сегодня)";
                }
                else if (dateTime == DateTime.Today.AddDays(1))
                {
                    result += " (завтра)";
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
