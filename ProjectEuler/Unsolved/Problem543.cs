using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=543
    /// 
    /// Define function P(n,k) = 1 if n can be written as the sum of k prime numbers (with repetitions allowed), 
    /// and P(n,k) = 0 otherwise.
    /// For example, P(10, 2) = 1 because 10 can be written as either 3 + 7 or 5 + 5, 
    /// but P(11,2) = 0 because no two primes can sum to 11.
    /// Let S(n) be the sum of all P(i, k) over 1 ≤ i,k ≤ n.
    /// For example, S(10) = 20, S(100) = 2402, and S(1000) = 248838.
    /// Let F(k) be the kth Fibonacci number(with F(0) = 0 and F(1) = 1).
    /// Find the sum of all S(F(k)) over 3 ≤ k ≤ 44
    /// </summary>
    public class Problem543 : EulerProblemBase
    {
        public Problem543() : base(543, "Prime-Sum Numbers", 0, 0) { }

        public override long Solve(long n)
        {
            long sum = 0;
            for (long fib = 3; fib < 44; fib++)
                sum += S(fib);
            return sum;
        }

        private long MaxNumber;

        private SieveOfEratosthenes Sieve;

        /// <summary>
        /// P(n,k) finds the number of sums consisting of k elements such that each element is a prime and the sum equals n        
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool P(long n, long k)
        {
            if ((n > MaxNumber) || (k > MaxNumber))
                throw new ArgumentException("n and k must be no larger than " + MaxNumber.ToString());

            var primes = Sieve.GetPrimes(2, (ulong)n);

            return P_recursive(primes, 0, n, k, 0);
            
        }

        /// <summary>
        /// S(n) is the sum of all P(i,k) with 1< =i,k <=n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private long S(long n)
        {
            long sum = 0;
            for (long i = 1; i <= n; i++)
                for (long k = 1; k <= i / 2; k++)
                    if (P(i, k))
                        sum++;
        
            return sum;
        }
        
        private bool P_recursive(List<ulong> primes, long partCount, long n, long k, long currentSum)
        {
            if ((currentSum > n) || (partCount > k))
                return false;
            else if (currentSum == n)
                return (partCount == k);
            else if (partCount == k)
                return (currentSum == n);
            else
            {
                foreach (var prime in primes)
                {
                    if (currentSum + (long)prime > n)
                        break;

                    if (P_recursive(primes, partCount + 1, n, k, currentSum + (long)prime))
                        return true;
                }
                return false;
            }
        }
                
    }
}
