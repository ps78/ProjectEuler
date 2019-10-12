using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=41
    /// 
    /// We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once.For example, 2143 is a 4-digit pandigital and is also prime.
    /// What is the largest n-digit pandigital prime that exists?
    /// 
    /// </summary>
    public class Problem041 : EulerProblemBase
    {
        public Problem041() : base(41, "Pandigital prime", 0, 7652413) { }

        private ulong[] Power10 = new ulong[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        public override long Solve(long n)
        {
            /// A number is divisible by 3, if the sum of the digits is divisible by three.
            /// Therefore, the largest pan digital number must have 7 digits, because any 
            /// 8 and 9 digit number has a sum of digits which is divisble by 3 and can therefore not produce
            /// a prime number
            var sieve = new SieveOfEratosthenes(8000000);
             
            var solutions = new List<ulong>();
            FindLargestPrime(sieve, "", "7654321", solutions);

            return (long)solutions[0];
        }

        private void FindLargestPrime(SieveOfEratosthenes sieve, string fixedPositions, string remainingDigits, List<ulong> solutions)
        {
            // if 6 positions are fixed, the 7th is given
            if (remainingDigits.Length == 1)
            {
                ulong n = ulong.Parse(remainingDigits);
                for (int i = 0; i < 6; i++)
                    n += ulong.Parse(fixedPositions[i].ToString()) * Power10[6-i];

                if (sieve.IsPrime(n))
                    solutions.Add(n);
            }
            // otherwise fix one more digit 
            else
            {
                for (int i = 0; i < remainingDigits.Length; i++)
                    FindLargestPrime(sieve, fixedPositions + remainingDigits[i], remainingDigits.Remove(i, 1), solutions);                    
            }
        }

    }
}
