using System;
using System.Collections;
using System.Collections.Generic;

namespace BSUIRScheduleDESK.Classes
{
    public static class ObjectComparer
    {
        public static Difference? GetDifferences(object left, object right)
        {
            Type type = left.GetType();

            Difference difference = new Difference() { PropertyName = left.ToString(), PropertyType = type };

            foreach (var prop in type.GetProperties())
            {
                var leftVal = prop.GetValue(left);
                var rightVal = prop.GetValue(right);

                if (Equals(leftVal, rightVal))
                {
                    continue;
                }
                else if ((leftVal == null && rightVal != null) || (rightVal == null && leftVal != null))
                {
                    difference.Differences.Add(new Difference
                    {
                        PropertyName = prop.Name,
                        PropertyType = prop.PropertyType,
                        OldValue = leftVal,
                        NewValue = rightVal
                    });
                }
                else
                {
                    if (prop.PropertyType.IsGenericType)
                    {
                        Difference genericDiff = new Difference() { PropertyName = prop.Name, PropertyType = prop.PropertyType };
                        Type elementType = prop.PropertyType.GetGenericArguments()[0];
                        var leftListVal = leftVal as IList;
                        var rightListVal = rightVal as IList;
                        if (leftListVal == null || rightListVal == null) return difference;
                        int maxSize = leftListVal.Count > rightListVal.Count ? leftListVal.Count : rightListVal.Count;
                        int minSize = leftListVal.Count > rightListVal.Count ? rightListVal.Count : leftListVal.Count;
                        int isLeftIsMin = leftListVal.Count == rightListVal.Count ? 0 : leftListVal.Count < rightListVal.Count ? 1 : -1;
                        int i = 0;
                        if (elementType == typeof(string))
                        {
                            for (; i < minSize; i++)
                            {
                                if (string.Equals(leftListVal[i], rightListVal[i])) continue;
                                genericDiff.Differences.Add(new Difference
                                {
                                    PropertyName = prop.Name,
                                    PropertyType = prop.PropertyType,
                                    OldValue = leftListVal[i],
                                    NewValue = rightListVal[i]
                                });
                            }
                        }
                        else if (elementType == typeof(int))
                        {
                            for (; i < minSize; i++)
                            {
                                if ((int?)leftListVal[i] == (int?)rightListVal[i]) continue;
                                genericDiff.Differences.Add(new Difference
                                {
                                    PropertyName = prop.Name,
                                    PropertyType = prop.PropertyType,
                                    OldValue = leftListVal[i],
                                    NewValue = rightListVal[i]
                                });
                            }
                        }
                        else
                        {
                            for (; i < minSize; i++)
                            {
                                Difference tempDiff = GetDifferences(leftListVal[i], rightListVal[i]);
                                if (tempDiff.Differences.Count == 0) continue;
                                genericDiff.Differences.Add(tempDiff);
                            }
                        }
                        if (isLeftIsMin == 1)
                        {
                            for (; i < maxSize; i++)
                            {
                                genericDiff.Differences.Add(new Difference
                                {
                                    PropertyName = prop.Name,
                                    PropertyType = prop.PropertyType,
                                    OldValue = null,
                                    NewValue = rightListVal[i]
                                });
                            }
                        }
                        else if (isLeftIsMin == -1)
                        {
                            for (; i < maxSize; i++)
                            {
                                genericDiff.Differences.Add(new Difference
                                {
                                    PropertyName = prop.Name,
                                    PropertyType = prop.PropertyType,
                                    OldValue = leftListVal[i],
                                    NewValue = null
                                });
                            }
                        }
                        if (genericDiff.Differences.Count != 0)
                        {
                            difference.Differences.Add(genericDiff);
                        }
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        if (string.Equals(leftVal, rightVal)) continue;
                        difference.Differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            PropertyType = prop.PropertyType,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        if ((int)leftVal == (int)rightVal) continue;
                        difference.Differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            PropertyType = prop.PropertyType,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        if ((bool)leftVal == (bool)rightVal) continue;
                        difference.Differences.Add(new Difference
                        {
                            PropertyName = prop.Name,
                            PropertyType = prop.PropertyType,
                            OldValue = leftVal,
                            NewValue = rightVal
                        });
                    }
                    else if (prop.PropertyType.IsClass)
                    {
                        Difference tempDiff = GetDifferences(leftVal, rightVal);
                        if (tempDiff.Differences.Count == 0) continue;
                        difference.Differences.Add(tempDiff);
                    }
                }

            }

            return difference;
        }

    }

    public record class Difference
    {
        public string? PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        public List<Difference> Differences { get; set; } = new List<Difference>();
    }
}
