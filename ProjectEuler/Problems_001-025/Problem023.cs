using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=23
    /// A perfect number is a number for which the sum of its proper divisors is exactly equal to the number. For example,
    /// the sum of the proper divisors of 28 would be 1 + 2 + 4 + 7 + 14 = 28, which means that 28 is a perfect number.
    /// A number n is called deficient if the sum of its proper divisors is less than n and it is called abundant if this sum exceeds n.
    /// As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the smallest number that can be written as the sum of
    /// two abundant numbers is 24. By mathematical analysis, it can be shown that all integers greater than 28123 can be 
    /// written as the sum of two abundant numbers. However, this upper limit cannot be reduced any further by analysis even
    /// though it is known that the greatest number that cannot be expressed as the sum of two abundant numbers is less than this limit.
    ///
    /// Find the sum of all the positive integers which cannot be written as the sum of two abundant numbers.
    /// </summary>
    public class Problem023 : EulerProblemBase
    {
        public Problem023() : base(23, "Non-abundant sum", 28123, 4179871) { }

        public override bool Test() => true;

        public override long Solve(long n)
        {
            var sieve = new SieveOfEratosthenes((ulong)n);
            var primes = sieve.GetPrimes().ToArray();

            // find abundant numbers
            var isAbundant = new Dictionary<int, bool>((int)n);
            for (int i = 1; i <= (int)n; i++)
            {
                var sumOfFactors = (long)sieve.GetFactors((ulong)i, primes).Sum() - i;
                isAbundant.Add(i, sumOfFactors > i);
            }

            var abundant = isAbundant.Where((pair) => pair.Value == true).Select((pair) => pair.Key).ToList();

            // check all numbers from 1 to ProblemSize whether they can be partitioned into two abundand nubmers
            ulong sum = 0;
            for (int i = 1; i < (int)n; i++)
            {
                bool canBePartioned = false;
                foreach (var a in abundant)
                {
                    if (a >= i) break;

                    if (isAbundant[i - a])
                    {
                        canBePartioned = true;
                        break;
                    }
                }
                if (!canBePartioned)
                    sum += (ulong)i;
            }

            return (long)sum;
        }
    }
}
