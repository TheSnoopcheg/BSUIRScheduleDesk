using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.Classes;

public static class ObjectComparer
{
    public static List<Difference> GetDifferences(object? left, object? right)
    {
        return GetDifferences(left, right, "root");
    }

    private static List<Difference> GetDifferences(object? left, object? right, string propertyName)
    {
        var differences = new List<Difference>();

        if (left == null && right == null)
        {
            return differences;
        }

        if (left == null || right == null)
        {
            differences.Add(new Difference
            {
                PropertyName = propertyName,
                OldValue = left?.ToString(),
                NewValue = right?.ToString()
            });
            return differences;
        }

        Type type = left.GetType();
        if (type != right.GetType())
        {
            differences.Add(new Difference
            {
                PropertyName = propertyName,
                OldValue = left.ToString(),
                NewValue = right.ToString(),
                Remark = "Object types do not match"
            });
            return differences;
        }

        if (type.IsValueType || type == typeof(string))
        {
            if (!Equals(left, right))
            {
                differences.Add(new Difference
                {
                    PropertyName = propertyName,
                    OldValue = left.ToString(),
                    NewValue = right.ToString()
                });
            }
            return differences;
        }

        if (left is IEnumerable leftEnumerable && right is IEnumerable rightEnumerable)
        {
            var collectionDifferences = GetCollectionDifferences(leftEnumerable, rightEnumerable, propertyName);
            if (collectionDifferences.Any())
            {
                differences.AddRange(collectionDifferences);
            }
            return differences;
        }


        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && !p.GetIndexParameters().Any()))
        {
            if (prop.IsDefined(typeof(JsonIgnoreAttribute), false))
            {
                continue;
            }

            var leftVal = prop.GetValue(left);
            var rightVal = prop.GetValue(right);

            differences.AddRange(GetDifferences(leftVal, rightVal, $"{propertyName}.{prop.Name}"));
        }


        return differences;
    }

    private static List<Difference> GetCollectionDifferences(IEnumerable left, IEnumerable right, string propertyName)
    {
        var differences = new List<Difference>();
        var leftList = left.Cast<object>().ToList();
        var rightList = right.Cast<object>().ToList();

        var addedItems = rightList.Except(leftList).ToList();
        var removedItems = leftList.Except(rightList).ToList();

        foreach (var item in addedItems)
        {
            differences.Add(new Difference
            {
                PropertyName = propertyName,
                NewValue = item?.ToString(),
                Remark = "Item added"
            });
        }

        foreach (var item in removedItems)
        {
            differences.Add(new Difference
            {
                PropertyName = propertyName,
                OldValue = item?.ToString(),
                Remark = "Item removed"
            });
        }

        return differences;
    }
}

public record class Difference
{
    public string? PropertyName { get; set; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
    public string? Remark { get; set; }
    public List<Difference> Differences { get; set; } = new List<Difference>();
}