using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    /// <summary>
    /// A class for fast prime tests of inputs of any size
    /// Uses a sieve for small numbers and the Miller Rabin Test for large numbers
    /// </summary>
    public class GeneralPrimeTest : IPrimeTest
    {
        public ulong SieveLimit { get; }

        public SieveOfEratosthenes Sieve { get; }

        public MillerRabinTest MillerRabin { get; }

        public GeneralPrimeTest(ulong sieveLimit = 1000000)
        {
            this.SieveLimit = sieveLimit;
            this.Sieve = new SieveOfEratosthenes(SieveLimit);
            MillerRabin = new MillerRabinTest();
        }

        public bool IsPrime(ulong n)
        {
            if (n <= SieveLimit)
                return Sieve.IsPrime(n);
            else
                return MillerRabin.IsPrime(n);            
        }
    }
}
