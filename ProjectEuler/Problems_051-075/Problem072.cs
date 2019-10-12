using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=72
    /// Consider the fraction, n/d, where n and d are positive integers. If n smaller d and HCF(n,d)=1, it is called a reduced proper fraction.
    /// If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:
    /// 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
    /// It can be seen that there are 21 elements in this set.
    /// 
    /// How many elements would be contained in the set of reduced proper fractions for d ≤ 1,000,000?
    /// </summary>
    public class Problem072 : EulerProblemBase
    {
        public Problem072() : base(72, "Counting fractions", 1_000_000, 303963552391) { }

        public override bool Test() => Solve(8) == 21;

        public override long Solve(long n)
        {
            /* Slow approach :
            var sieve = new SieveOfEratosthenes(ProblemSize);

            ulong count = 0;
            for (ulong d = 2; d <= ProblemSize; d++)
            {
                if (sieve.IsPrime(d))
                    count += d - 1;
                else
                    count += Totient.ComputeForNonPrimes(d, sieve);
            }

            return count;
            */

            // because the Totient function is phi(n) = n * product (1-1/p) where p divides n, this can be speeded up by using a sieve
            // and not calculating these part all over again

            var totients = CreateTotients((int)n);
            ulong sum = 0;
            for (int i = 2; i < totients.Length; i++)
                sum += (ulong)totients[i];

            return (long)sum;
        }

        public int[] CreateTotients(int n)
        {
            var t = new int[n + 1];

            // initilize array t with t[i] = i
            for (int i = 0; i <= n; i++)
                t[i] = i;

            for (int i = 2; i <= n; i++)
                if (t[i] == i)
                    for (int j = i; j <= n; j += i)
                        t[j] -= t[j] / i;

            return t;
        }
    }
}
