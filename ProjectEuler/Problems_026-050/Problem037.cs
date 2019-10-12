using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=37
    ///
    /// The number 3797 has an interesting property.Being prime itself, it is possible to continuously 
    /// remove digits from left to right, and remain prime at each stage: 3797, 797, 97, and 7. 
    /// Similarly we can work from right to left: 3797, 379, 37, and 3.
    /// Find the sum of the only eleven primes that are both truncatable from left to right and right to left.
    /// NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes.
    /// </summary>
    public class Problem037 : EulerProblemBase
    {
        public Problem037() : base(37, "Truncatable primes", 0, 748317) { }

        SieveOfEratosthenes sieve;
        MillerRabinTest mrt;

        public override long Solve(long n)
        {
            sieve = new SieveOfEratosthenes(10000);
            mrt = new MillerRabinTest();

            var results = new List<ulong>();
            var nums = new List<string>(new string[] { "2", "3", "5", "7" });
            while (results.Count < 11)
            {
                List<string> extendedNums = new List<string>();
                foreach (var ch in new string[] { "1", "3", "7", "9" })
                    foreach (string m in nums)                    
                    {
                        string newNum = m + ch;
                        if (IsPrime(newNum))
                            extendedNums.Add(newNum);

                        if (IsLeftTruncateablePrime(newNum))
                            results.Add(ulong.Parse(newNum));
                    }                                                
                nums = extendedNums;
            }
            
            //results.ForEach(r => Console.WriteLine("{0}", r));
            return (long)results.Sum();
        }

        private bool IsPrime(string n)
        {
            ulong m = ulong.Parse(n);
            return m > sieve.Limit ? mrt.IsPrime(m) : sieve.IsPrime(m);
        }

        private bool IsLeftTruncateablePrime(string n)
        {
            for (int i = 0; i < n.Length; i++)
                if (!IsPrime(n.Substring(i)))
                    return false;
            return true;
        }

    }
}
