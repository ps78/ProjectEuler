using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=543
    /// 
    /// Define function P(n,k) = 1 if n can be written as the sum of k prime numbers (with repetitions allowed), 
    /// and P(n,k) = 0 otherwise.
    /// 
    /// For example, P(10, 2) = 1 because 10 can be written as either 3 + 7 or 5 + 5, 
    /// but P(11,2) = 0 because no two primes can sum to 11.
    /// 
    /// Let S(n) be the sum of all P(i, k) over 1 ≤ i,k ≤ n.
    /// 
    /// For example, S(10) = 20, S(100) = 2402, and S(1000) = 248838.
    /// 
    /// Let F(k) be the kth Fibonacci number(with F(0) = 0 and F(1) = 1).
    /// 
    /// Find the sum of all S(F(k)) over 3 ≤ k ≤ 44
    /// </summary>
    public class Problem543 : EulerProblemBase
    {
        private IPrimeTest PrimeTest = new MillerRabinTest();

        public Problem543() : base(543, "Prime-Sum Numbers", 44, 199007746081234640) { }

        public override long Solve(long n)
        {
            ulong sum = 0;
            foreach (int k in Enumerable.Range(3, (int)n - 2))
                sum += S(Fibonacci.Get(k));
            
            return (long)sum;
        }

        /// <summary>
        /// S(n) is the sum of all P(i,k) with 1< =i,k <=n
        /// 
        /// Some observations from https://oeis.org/A051034:
        /// Assuming Goldbachs conjecture is true:
        /// - the minimum number of prime partition constiuents of any n is 1, 2 or 3
        /// - the maximum number of prime partition constituents of any n is n/2
        /// 
        /// k  |      F(k)|           S(F(k))
        /// ----------------------------------
        /// 3  |        2 |                 1
        /// 4  |        3 |                 2
        /// 5  |        5 |                 5
        /// 6  |        8 |                13
        /// 7  |       13 |                35
        /// 8  |       21 |                96
        /// 9  |       34 |               262
        /// 10 |       55 |               707
        /// 20 |     6765 |          11432902
        /// 30 |   832040 |      173071524976
        /// 40 |102334155 |  2618069678166177
        /// 44 |701408733 |122993551702697596
        /// 
        /// TODO: replace the IsPrime() by prime counting such as this:
        /// https://github.com/kimwalisch/primecount?tab=readme-ov-file
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private ulong S(ulong n)
        {
            if (n <= 6)
                return new ulong[] { 0, 0, 1, 2, 3, 5, 7 }[n];
            else
            {
                ulong primeCount = CountPrimes.Get(n);
                ulong delta = n % 2 == 0 ? 1UL : 0;
                ulong primeCount3_to_n2 = primeCount - 1 - (PrimeTest.IsPrime(n - delta) ? 1UL : 0);

                ulong sum = 0;

                // k = 1
                sum += primeCount;

                // k = 2
                sum += ((n - 4) / 2 + 1) - (n - 3) * 2;
                sum += primeCount3_to_n2;
                
                if (n % 2 == 0)
                    sum += (n - 4) / 2 * (n / 2 + 1) + n / 2;
                else
                    sum += (n / 2 - 1) * (2 + n / 2);

                return sum;
            }
        }
    }
}
