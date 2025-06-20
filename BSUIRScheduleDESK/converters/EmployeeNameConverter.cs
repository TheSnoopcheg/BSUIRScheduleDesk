﻿using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class EmployeeNameConverter : IValueConverter
    {
        public static readonly EmployeeNameConverter Instance = new EmployeeNameConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string name) return string.Empty;
            if (int.TryParse(name, out int numVal))
            {
                return name;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                string[] parts = name!.Split(' ');
                stringBuilder.Append(parts[0], 0, 3);
                stringBuilder.Append("\n" + parts[1] + parts[2]);
                return stringBuilder.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
