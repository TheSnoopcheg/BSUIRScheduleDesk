using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;


namespace BSUIRScheduleDESK.Converters
{
    public class EmployeeOrGroupConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string? sparam = parameter as string;
            switch (sparam)
            {
                case "Type":
                    {
                        Lesson? schedule = value as Lesson;
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

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0) return null;
            string? sparam = parameter as string;
            if (sparam != "Type") return null;
            if (values[0] is not Announcement announcement) return null;
            if (values[1] is not bool isEmplAnn) return null;
            if (isEmplAnn)
                return announcement.studentGroups?.OrderBy(s => s.name)!;
            else
                return new List<Employee> { new Employee { firstName = announcement.employee, urlId = announcement.urlId } };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
