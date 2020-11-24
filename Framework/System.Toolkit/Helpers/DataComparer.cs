using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Toolkit.Helpers
{
    public class DataComparer
    {
        public new static bool Equals(object d1, object d2)
        {
            if (d1 == d2)
                return true;
            if (d1 == null && d2 != null)
                return false;
            if (d1 != null && d2 == null)
                return false;
            if (d1.Equals(d2))
                return true;
            if (d1.GetType().IsValueType && d2.GetType().IsValueType)
            {
                decimal td1 = 0;
                decimal td2 = 0;
                return decimal.TryParse(d1.ToString(), out td1) && decimal.TryParse(d2.ToString(), out td2) &&
                       td1.Equals(td2);
            }
            if (d1.GetType() != d2.GetType())
                return false;
            if (d1 is IEnumerable)
            {
                var e1 = (d1 as IEnumerable).Cast<object>().AsParallel();
                var e2 = (d2 as IEnumerable).Cast<object>().AsParallel();
                return e1.SequenceEqual(e2, new DataEqualityComparer());
            }
            return false;
        }
    }

    public class DataEqualityComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            return DataComparer.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}