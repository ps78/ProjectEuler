using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=243
    /// 
    /// A positive fraction whose numerator is less than its denominator is called a proper fraction.
    /// For any denominator, d, there will be d-1 proper fractions; for example, with d=12:
    /// 1/12, 2/12, 3/12, 4/12, 5/12, 6/12, 7/12, 8/12, 9/12, 10/12, 11/12.
    /// 
    /// We shall call a fraction that cannot be cancelled down a resilient fraction.
    /// Furthermore we shall define the resilience of a denominator,
    /// R(12)=4/11, to be the ratio of its proper fractions that are resilient; for example, .
    /// In fact, d=12 is the smallest denominator having a resilience R(d) < 4/10.
    /// 
    /// Find the smallest denominator d having a resilience R(d) < 15499/94744.
    /// </summary>
    public class Problem243 : EulerProblemBase
    {
        public Problem243() : base(243, "Resilience", 0, 892_371_480) { }

        public override bool Test() => true;

        public override long Solve(long n)
        {
            ulong primeLimit = 100000; // must be > sqrt(d) for max d
            ulong maxPrimeFactor = 29; 
            double limit = 15499.0 / 94744;

            var sieve = new SieveOfEratosthenes(primeLimit);
            var primes = sieve.GetPrimes(2, primeLimit).ToArray();
            
            foreach (var d in GenerateDSequence(sieve, maxPrimeFactor))
            {
                ulong resilientCount = Totient.ComputeForNonPrimes(d, sieve);
                if ((double)resilientCount / (d - 1) < limit)
                    return (long)d;
            }

            throw new Exception("No solution found");
        }

        private ulong Product(ulong[] factors, ulong[] exponents)
        {
            ulong product = 1;
            for (int i = 0; i < factors.Length; i++)
            {
                if (exponents[i] == 0)
                    break;

                for (ulong _ = 0; _ < exponents[i]; _++)
                    product *= factors[i];
            }
            return product;
        }

        /// <summary>
        /// Generates a sequence of numbers ordered by size that contain
        /// all primes factors in consecutive orders. i.e.
        /// 2 = 2
        /// 3 = 3
        /// 4 = 2^2
        /// 6 = 2*3
        /// 9 = 3*3
        /// 12 = 2^2*3
        /// etc.
        /// </summary>
        /// <returns></returns>
        private ulong[] GenerateDSequence(SieveOfEratosthenes sieve, ulong maxPrimeFactor)
        {
            var primeFactors = sieve.GetPrimes(2, maxPrimeFactor).ToArray();
            var exp = new ulong[primeFactors.Length];

            // this will be maximum value returned by the function
            ulong limit = primeFactors.Product();
            var numbers = new List<ulong>();

            CreateVariations(limit, primeFactors, ref exp, 0, ref numbers);

            return numbers.Order().ToArray();
        }

        private void CreateVariations(ulong limit, ulong[] primeFactors, ref ulong[] exponents, int expIndex,
                                      ref List<ulong> numbers)
        {
            exponents[expIndex] = 1;
            while (true)
            {
                ulong currentNumber = Product(primeFactors, exponents);
                
                if (currentNumber > limit)
                    break;
                else
                    numbers.Add(currentNumber);

                if (expIndex < exponents.Length - 1) 
                    CreateVariations(limit, primeFactors, ref exponents, expIndex+1, ref numbers);

                exponents[expIndex]++;
            }
            exponents[expIndex] = 0;
        }
    }
}
