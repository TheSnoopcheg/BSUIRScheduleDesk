using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BSUIRScheduleDESK.Classes
{
    public static class ObjectComparer
    {
        
        public static List<Difference> GetDifferences(object left, object right)
        {
            List<Difference> differences = new List<Difference>();

            Type type = left.GetType();

            foreach(var prop in type.GetProperties())
            {
                var leftVal = prop.GetValue(left);
                var rightVal = prop.GetValue(right);
                if(Equals(leftVal, rightVal))
                {
                    continue;
                }
                else if (leftVal == null || rightVal == null)
                {
                    differences.Add(new Difference
                    {
                        PropertyName = prop.Name,
                        PropertyType = prop.PropertyType,
                        OldValue = leftVal,
                        NewValue = rightVal
                    });
                }
                else if (prop.PropertyType.IsClass)
                {

                }
            }

            return differences;
        }

    }

    public record class Difference
    {
        public string? PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public object? OldValue {  get; set; }
        public object? NewValue { get; set; }
    }
}
