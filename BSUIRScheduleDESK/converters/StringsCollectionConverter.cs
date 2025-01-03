using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class StringsCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter is not string sparam) return null;
            switch (sparam)
            {
                case "Employee":
                    {
                        var values = value as List<Employee>;
                        return string.Join("\n", values?.Select(s => s.ToString()) ?? Enumerable.Empty<string>());
                    }
                case "Group":
                    {
                        var values = value as List<StudentGroup>;
                        return string.Join("\n", values?.Select(s => s.name!) ?? Enumerable.Empty<string>());
                    }
                case "Auditories":
                    {
                        List<string>? values = value as List<string>;
                        return string.Join("\n", values!);
                    }
                case "Weeks":
                    {
                        List<int>? values = value as List<int>;
                        return string.Join(", ", values?.Select(i => i.ToString()) ?? Enumerable.Empty<string>());
                    }
                case "SubGroup":
                    {
                        int subGroup = (int)value;
                        if(subGroup != 0)
                            return subGroup.ToString() + " п.";
                        return null;   
                    }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
