using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.Classes
{
    public static class ObjectComparer
    {
        public static List<Difference> GetDifferences(object? left, object? right, out string? leftName, out string? rightName)
        {
            List<Difference> differences = new List<Difference>();

            leftName = null;
            rightName = null;

            if(left == null || right == null) return differences;

            Type type = left == null ? right!.GetType() : left.GetType();

            leftName = (left == null || left.ToString() == type.FullName) ? null : left.ToString();
            rightName = (right == null || right.ToString() == type.FullName) ? null : right.ToString();


            foreach (var prop in type.GetProperties().Where(p => !p.GetIndexParameters().Any()))
            {
                var leftVal = prop.GetValue(left);
                var rightVal = prop.GetValue(right);

                object[] attributes = prop.GetCustomAttributes(false);
                if (attributes.Any(a => a is JsonIgnoreAttribute)) continue;

                if (leftVal == rightVal)
                {
                    continue;
                }
                else if (leftVal == null && rightVal != null)
                {
                    differences.Add(new Difference
                    {
                        PropertyName = prop.Name,
                        OldValue = null,
                        NewValue = rightVal.ToString()
                    });
                }
                else if (rightVal == null && leftVal != null)
                {
                    differences.Add(new Difference
                    {
                        PropertyName = prop.Name,
                        OldValue = leftVal.ToString(),
                        NewValue = null
                    });
                }
                else
                {
                    if (prop.PropertyType.IsGenericType)
                    {
                        Difference genericDiff = new Difference() { PropertyName = prop.Name };
                        Type elementType = prop.PropertyType.GetGenericArguments()[0];
                        genericDiff.Differences = GetGenericDifferences(leftVal, rightVal, elementType);
                        if (genericDiff.Differences != null && genericDiff.Differences.Count != 0)
                            differences.Add(genericDiff);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        if (string.Equals(leftVal, rightVal)) continue;
                        differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        if ((int)leftVal == (int)rightVal) continue;
                        differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        if ((bool)leftVal == (bool)rightVal) continue;
                        differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType.IsClass)
                    {
                        Difference tempDiff = new Difference() { PropertyName = prop.Name };
                        tempDiff.Differences = GetDifferences(leftVal, rightVal, out string? tempLeftName, out string? tempRightName);
                        if (tempDiff.Differences == null || tempDiff.Differences.Count == 0) continue;
                        tempDiff.OldValue = tempLeftName;
                        tempDiff.NewValue = tempRightName;
                        differences.Add(tempDiff);
                    }
                }
            }
            return differences;
        }
        private static List<Difference> GetGenericDifferences(object? left, object? right, Type type)
        {
            List<Difference>? differences = new List<Difference>();

            differences = type.Name switch
            {
                nameof(String) => GetGenericDifferences<string>(left, right),
                nameof(Int32) => GetGenericDifferences<int>(left, right),
                nameof(Lesson) => GetGenericDifferences<Lesson>(left, right),
                nameof(Employee) => GetGenericDifferences<Employee>(left, right),
                nameof(StudentGroup) => GetGenericDifferences<StudentGroup>(left, right),
                _ => differences
            };
            return differences;
        }

        private static List<Difference> GetGenericDifferences<T>(object? left, object? right)
        {
            List<Difference>? differences = new List<Difference>();
            if (left is not IList<T> leftList || right is not IList<T> rightList) return differences;
            string? leftName, rightName;
            int index = -1;
            HashSet<int> indexes = new HashSet<int>();
            for(int i = 0; i < leftList.Count; i++)
            {
                index = rightList.IndexOf(leftList[i]);
                if(index < 0)
                {
                    differences.Add(new Difference { NewValue = null, OldValue = leftList[i]!.ToString() });
                    continue;
                }
                var tempDiffs = GetDifferences(leftList[i], rightList[index], out leftName, out rightName);
                if (tempDiffs != null && tempDiffs.Count > 0)
                    differences.Add(new Difference { NewValue = rightName, OldValue = leftName, Differences = tempDiffs });
                indexes.Add(index);
            }

            for(int i = 0; i < rightList.Count; i++)
            {
                if (indexes.Contains(i)) continue;
                differences.Add(new Difference { OldValue = null, NewValue = rightList[i]!.ToString() });
            }

            return differences;
        }
    }

    public record class Difference
    {
        public string? PropertyName { get; set; }
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        public List<Difference> Differences { get; set; } = new List<Difference>();
    }
}
