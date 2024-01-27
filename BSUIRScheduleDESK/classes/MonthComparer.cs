using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BSUIRScheduleDESK.classes
{
    public class MonthComparer : IEqualityComparer<DateTime>
    {
        public bool Equals(DateTime x, DateTime y)
        {
            return x.Year == y.Year && x.Month == y.Month;
        }

        public int GetHashCode([DisallowNull] DateTime obj)
        {
            return obj.Month ^ obj.Year;
        }
    }
}
