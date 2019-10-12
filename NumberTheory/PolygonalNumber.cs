using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    /// <summary>
    /// Class to compute polygonal numbers, like 
    ///     Triangle numbers    P3,n = n(n+1)/2
    ///     Square numbers      P4,n = n^2
    ///     Pentagonal          P5,n = n(3n-1)/2
    ///     Hexagonal           P6,n = n(2n-1)
    ///     Heptagonal          P7,n = 5(n-3)/2
    ///     Octonal             P8,n = n(3n-2)
    ///     p-gonal number      Pp,n = n/2 * (n*(p-2) - (p-4))
    /// </summary>
    public static class PolygonalNumber
    {
        /// <summary>
        /// returns the n-th element of the general p-polygonal number series
        /// </summary>
        /// <returns></returns>
        public static ulong P(int p, ulong n)
        {
            if (p < 4)
                return (ulong)( (p - 2) * (int)n - (p - 4)) * n / 2;
            else
                return ((ulong)(p - 2) * n - (ulong)(p - 4)) * n / 2;
                
        }

        public static IEnumerable<ulong> P(int p, ulong startn, ulong endn)
        {
            for (ulong n = startn; n <= endn; n++)
                yield return P(p, n);
        }

        public static ulong Triangle(ulong n)
        {
            return (ulong)n * ((ulong)n + 1) / 2;
        }

        public static ulong Square(ulong n)
        {
            return n * n;
        }

        public static ulong Pentagonal(ulong n)
        {
            return n * (3 * n - 1) / 2;
        }

        public static ulong Hexagonal(ulong n)
        {
            return n * (2 * n - 1);
        }

        public static ulong Heptagonal(ulong n)
        {
            return n * (5 * n - 3) / 2;
        }

        public static ulong Octagonal(ulong n)
        {
            return n * (3 * n - 2);
        }
    }
}
