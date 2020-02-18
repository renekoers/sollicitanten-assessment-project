using System;
using System.Collections.Generic;

namespace BackEnd
{
    public class Util
    {
        private Util() { }

        public static T Min<T>(List<T> list, Func<T, long> getValue) where T : class
        {
            long? min = null;
            T minT = null;
            foreach (T t in list)
            {
                if (!min.HasValue || getValue(t) < min.Value)
                {
                    minT = t;
                    min = getValue(t);
                }
            }
            return minT;
        }
        public static int? Min(List<int> list)
        {
            int? min = null;
            foreach (int t in list)
            {
                if (!min.HasValue || t < min.Value)
                {
                    min = t;
                }
            }
            return min;
        }
        public static int? Max(List<int> list)
        {
            int? max = null;
            foreach (int t in list)
            {
                if (!max.HasValue || t > max.Value)
                {
                    max = t;
                }
            }
            return max;
        }
    }
}
