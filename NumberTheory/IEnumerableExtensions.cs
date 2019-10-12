using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public static class IEnumerableExtensions
    { 
        public static string ToString<T>(this IEnumerable<T> list, string separator = ", ")
        {
            var sb = new StringBuilder();

            bool start = true;
            foreach (T element in list)
            {
                if (!start)
                    sb.Append(separator);
                else
                    start = false;
                sb.Append(element.ToString());                
            }

            return sb.ToString();
        }

        public static ulong Sum(this IEnumerable<ulong> list)
        {
            ulong sum = 0;
            foreach (ulong l in list)
                sum += l;
            return sum;
        }

        public static ulong Product(this IEnumerable<ulong> list)
        {
            ulong result = 1;
            foreach (var x in list)
                result *= x;
            return result;
        }

        public static long Product(this IEnumerable<long> list)
        {
            long result = 1;
            foreach (var x in list)
                result *= x;
            return result;
        }

        public static long Product(this IEnumerable<int> list)
        {
            long result = 1;
            foreach (var x in list)
                result *= x;
            return result;
        }

    }
}
