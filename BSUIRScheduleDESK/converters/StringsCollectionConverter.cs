using BSUIRScheduleDESK.classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.converters
{
    public class StringsCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string? sparam = parameter as string;
            switch (sparam)
            {
                case "Employee":
                    {
                        var values = value as List<Employee>;
                        List<string> list = new List<string>();
                        foreach (Employee s in values!)
                        {
                            list.Add(s.ToString());
                        }
                        return string.Join("\n", list);
                    }
                case "Group":
                    {
                        var values = value as List<StudentGroup>;
                        List<string> list = new List<string>();
                        foreach (StudentGroup s in values!)
                        {
                            list.Add(s.name!);
                        }
                        return string.Join("\n", list);
                    }
                case "Auditories":
                    {
                        List<string>? values = value as List<string>;
                        return string.Join("\n", values);
                    }
                case "Weeks":
                    {
                        List<int>? values = value as List<int>;
                        List<string> list = new List<string>();
                        foreach (int i in values!)
                        {
                            list.Add(i.ToString());
                        }
                        return string.Join(", ", list);
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
