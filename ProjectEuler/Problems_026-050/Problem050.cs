using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=50
    /// 
    /// The prime 41, can be written as the sum of six consecutive primes:
    /// 41 = 2 + 3 + 5 + 7 + 11 + 13
    /// This is the longest sum of consecutive primes that adds to a prime below one-hundred.
    /// The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953.
    /// Which prime, below one-million, can be written as the sum of the most consecutive primes?
    /// </summary>
    public class Problem050 : EulerProblemBase
    {
        public Problem050() : base(50, "Consecutive prime sum", 1_000_000, 997651) { }

        public override bool Test() => Solve(100) == 41 && Solve(1000) == 953;
        
        public override long Solve(long n)
        {            
            var sieve = new SieveOfEratosthenes((ulong)n);
            var primes = sieve.GetPrimes();

            // first, calculate the maximum number of summands there can be, by adding
            // all primes starting at 2 until the given problem size is exeeded
            ulong sum = 0;
            int i = 0;
            while (sum < (ulong)n)
                sum += primes[i++];
            ulong limit = (ulong)(i - 1);

            // try to find a solution using that many summands
            // if none is found, decrease the limit
            for (ulong currentLimit = limit; currentLimit > 0; currentLimit--)
            {
                ulong currentSum = 0;
                for (int j = 0; j < (int)currentLimit; j++)
                    currentSum += primes[j];

                int maxIdx = primes.Count - (int)currentLimit;
                for (int startIdx = 0; startIdx <= maxIdx; startIdx++)
                {
                    if (sieve.IsPrime(currentSum))
                    {                        
                        return (long)currentSum;
                    }
                    else
                    {
                        int endIdx = startIdx + (int)currentLimit;
                        currentSum = currentSum - primes[startIdx] + primes[endIdx];
                        if (currentSum > (ulong)n)
                            break;
                    }                                        
                }
            }

            return 0;
        }

    }
}
