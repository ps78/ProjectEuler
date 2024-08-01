using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class Fibonacci
    {
        // constants used for estimation formula
        private static readonly double SQRT5 = Math.Sqrt(5);
        private static readonly double PHI = (1 + SQRT5) / 2;
        private static readonly double PSI = (1 - SQRT5) / 2;
        private static readonly decimal SQRT5d = (decimal)Math.Sqrt(5);
        private static readonly BigDecimal PHId = new ((1m + SQRT5d) / 2);
        private static readonly BigDecimal PSId = new ((1m - SQRT5d) / 2);

        // k for largest Fk that can be represented as ulong
        public const int MaxUlongIndex = 93;

        // k for largest Fk that is pre-computed in the static class for bigintegers
        private const int BigCacheInitIndex = 1000;

        private static ulong[] cache;
        private static List<BigInteger> cacheBig;

        static Fibonacci()
        {
            // build the cache for the first 93 Fibonacci numbers that can be represented
            // as ulongs
            cache = new ulong[MaxUlongIndex+1];
            cache[0] = 0;
            cache[1] = 1;
            for (int k = 2; k <= MaxUlongIndex; k++)
                cache[k] = cache[k - 1] + cache[k - 2];

            // copy cache to bigint, which can be extended
            cacheBig = cache.Select(x => new BigInteger(x)).ToList();
            for (int k = MaxUlongIndex + 1; k <= BigCacheInitIndex; k++)
                cacheBig.Add(cacheBig[^1] + cacheBig[^2]);
        }

        /// <summary>
        /// Fibonacci numbers for k = 0..MaxUlongIndex (93)
        /// Numbers are stored in cache, function returns instantly
        /// </summary>
        /// <param name="k">Fibonacci index, F0=0, F1=1</param>
        /// <returns></returns>
        public static ulong Get(int k)
        {
            if (k <= MaxUlongIndex)
                return cache[k];
            else
                throw new OverflowException($"Fk for k > {MaxUlongIndex} cannot be represented as 64 bit integer");
        }

        /// <summary>
        /// Recursive Fibonacci numbers
        /// </summary>
        /// <param name="k">Fibonacci index, F1=1, F2=1</param>
        /// <param name="noCache">set to true to disable storing intermediate values in the internal cache</param>
        /// <returns></returns>
        public static BigInteger GetBig(int k, bool noCache = false)
        {
            if (k < cacheBig.Count)
            {
                return cacheBig[k];
            }
            else if (noCache)
            {
                var Fk2 = cacheBig[^2];
                var Fk1 = cacheBig[^1];
                BigInteger Fi;
                int i = cacheBig.Count;
                do
                {
                    Fi = Fk2 + Fk1;
                    Fk2 = Fk1;
                    Fk1 = Fi;
                    i++;
                }
                while (i <= k);

                return Fi;
            }
            else
            {
                for (int i = cacheBig.Count; i <= k; i++)
                {
                    cacheBig.Add(cacheBig[^2] + cacheBig[^1]);
                }
                return cacheBig[^1];
            }
        }

        /// <summary>
        /// Non-recursive calculation of Fibonacci numbers using 
        /// an approximation.
        /// 
        /// Exact for k = 1..83
        /// for k = 84 ..1474 returns rounded results 
        /// for k >= 1475 throws an overflow exception
        /// 
        /// </summary>
        /// <param name="k">Fibonacci index, F0=0, F1=1</param>
        /// <returns></returns>
        public static double Estimate(int k)
        {
            if (k == 0)
            {
                return 0;
            }
            else if (k <= 2)
            {
                return 1;
            }
            else if (k < 1475)
            {
                double x = (Math.Pow(PHI, k) - Math.Pow(PSI, k)) / SQRT5;
                return Math.Round(x);
            }
            else
                throw new OverflowException("Only Fibonacci numbers up to F1474 can be estimated. Use EstimateBig for larger k");
        }

        public static BigDecimal EstimateBig(int k)
        {
            if (k == 0)
            {
                return BigDecimal.Zero;
            }
            else if (k <= 2)
            {
                return BigDecimal.One;
            }
            else
            {
                BigDecimal t1 = (PHId ^ k) / SQRT5d;
                BigDecimal t2 = (PSId ^ k) / SQRT5d;
                return t1 - t2;
            }
        }
    }
}
