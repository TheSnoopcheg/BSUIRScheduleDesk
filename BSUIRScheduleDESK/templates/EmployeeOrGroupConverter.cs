using BSUIRScheduleDESK.classes;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BSUIRScheduleDESK.templates
{
    public class EmployeeOrGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string? sparam = parameter as string;
            switch (sparam)
            {
                case "Type":
                    {
                        Schedule? schedule = value as Schedule;
                        if (schedule?.employees == null)
                            return schedule?.studentGroups!.OrderBy(s => s.name)!;
                        return schedule?.employees!;
                    }
                case "Text":
                    {
                        if(value is StudentGroup s)
                        {
                            return $"{s.specialityName} ({s.numberOfStudents} студентов)";
                        }
                        else if(value is Employee e)
                        {
                            return e.GetFullName();
                        }
                        return null;
                    }
                case "IsEmployee":
                    {
                        if(value is Employee v)
                        {
                            return true;
                        }
                        return false;
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
