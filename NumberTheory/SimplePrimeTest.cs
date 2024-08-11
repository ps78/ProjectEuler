using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class SimplePrimeTest : IPrimeTest
    {
        //static ulong[] smallPrimes = new ulong[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101 };
        private static readonly ulong[] smallPrimes = [2, 3, 5, 7, 11, 13, 17, 19, 23];

        public bool IsPrime(ulong n)
        {
            if (n < 2)
                return false;

            if (smallPrimes.Contains(n))
                return true;

            foreach (var p in smallPrimes)
                if ((n % p) == 0)
                    return false;

            ulong sqn = (ulong)Math.Sqrt(n);
            ulong m = smallPrimes.Last() + 2;
            for (ulong d = m; d <= sqn; d += 2)
                if ((n % d) == 0)
                    return false;

            return true;
        }
    }
}
