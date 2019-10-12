using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class SieveOfEratosthenes : IPrimeTest
    {
        #region Fields, private properties

        private readonly ulong[] bitMasks;

        private readonly Byte[] bitPosition;

        private ulong[] sieve;

        private List<ulong> primeCache = null;
        private ulong primeCacheLBound = 0;
        private ulong primeCacheUBound = 0;
                
        /// <summary>
        /// gets/sets a bit in the sieve. 
        /// the first bit has index 0
        /// the maximum index is larger or equal to limit-1
        /// </summary>        
        /// <returns></returns>        
        private bool this[ulong index]
        {
            get
            {                
                // special case: 1 is not a prime
                if (index <= 1)
                    return false;
                // special case: 2, 3, 5 are primes
                else if ((index == 2) || (index == 3) || (index == 5))
                    return true;
                // multiples of 2, 3 and 5 always return 0
                if ((index % 2 == 0) || (index % 3 == 0) || (index % 5 == 0))
                    return false;
                else
                {                 
                    ulong sieveValue = sieve[index / 240];
                    return (sieveValue & bitMasks[bitPosition[index % 240]]) != 0;
                }
            }
            set
            {
                if ((index % 2 == 0) || (index % 3 == 0) || (index % 5 == 0))
                    throw new InvalidOperationException("Cannot set bit for number " + index.ToString() + " because it is dividible by 2, 3 or 5 and is hence excluded from the sieve");
                else
                {
                    if (value == true)
                        sieve[index / 240] |= bitMasks[bitPosition[index % 240]];
                    else
                        sieve[index / 240] &= ~bitMasks[bitPosition[index % 240]];
                }
            }
        }

        #endregion
        #region public Properties     

        /// <summary>
        /// returns the size of the sieve in bytes
        /// </summary>
        public ulong Size
        {
            get { return (ulong)sieve.Length * 8; }
        }

        /// <summary>
        /// The upper limit for which the sieve has been built
        /// </summary>
        public ulong Limit { get; }

        #endregion
        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="limit"></param>
        public SieveOfEratosthenes(ulong limit)
        {
            if (limit < 30)
                limit = 30;                

            this.Limit = limit;

            // create bitmasks
            bitMasks = new ulong[64];
            for (int i = 0; i < 64; i++)
                bitMasks[i] = ((ulong)1 << i);

            // create bit positions, mapping numbers in a 240-number window to bit positionsn withing a 64-bit UInt
            // note, numbers that are dividible by 2, 3 or 5 are not set
            byte bitPos = 0;
            bitPosition = new byte[240];
            for (int i = 0; i < 8; i++)
            {
                bitPosition[i * 30 + 1] = bitPos++;
                bitPosition[i * 30 + 7] = bitPos++;
                bitPosition[i * 30 + 11] = bitPos++;
                bitPosition[i * 30 + 13] = bitPos++;
                bitPosition[i * 30 + 17] = bitPos++;
                bitPosition[i * 30 + 19] = bitPos++;
                bitPosition[i * 30 + 23] = bitPos++;
                bitPosition[i * 30 + 29] = bitPos++;
            }

            BuildSieve();
        }

        /// <summary>
        /// Returns all primes in the sieve
        /// </summary>
        /// <returns></returns>
        public List<ulong> GetPrimes()
        {
            return GetPrimes(2, Limit);
        }

        /// <summary>
        /// Returns the primes in the given range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ulong> GetPrimes(ulong from = 2, ulong to = 0)
        {
            if (from < 2)
                from = 2;
            if (to <= 0)
                to = Limit;            

            if (from > to)
                throw new ArgumentException("from must be smaller than to");
            if (to > Limit)
                throw new ArgumentException("to must be no larger than the limit of the sieve (" + Limit.ToString() + ")");

            var result = new List<ulong>();
            // take result from cache
            if (primeCache != null && primeCacheLBound <= from && primeCacheUBound >= to)
            {
                result.AddRange(primeCache.Where(p => p >= from && p <= to));
            }
            // calculate result and store in cache
            else
            {
                if ((from <= 2) && (to >= 2)) result.Add(2);

                ulong start = (from % 2 == 0 ? from + 1 : from);
                for (ulong i = start; i <= to; i += 2)
                    if (this[i] == true)
                        result.Add(i);

                primeCache = result;
                primeCacheLBound = from;
                primeCacheUBound = to;
            }
            return result;
        }

        /// <summary>
        /// checks if n is a prime. Very fast if n is below the sieve limit
        /// If n is larger than the sieve limit, but smaller than limit^2, 
        /// brute-force prime divisions will be made
        /// If primes are already retrieved, they should be passsed in the primes array 
        /// to prevent recalculation
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsPrime(ulong n)
        {
            if (n <= Limit)
            {
                return this[n];
            }
            else if (n <= Limit * Limit)
            {
                ulong root = (ulong)Math.Sqrt(n);                
                foreach (var p in GetPrimes(2, root))
                {
                    if (n % p == 0)
                        return false;
                    if (p > root)
                        break;
                }
                return true;
            }
            else
            {
                throw new ArgumentException("The given number is larger than the sieve limit squared");
            }
        }

        /// <summary>
        /// returns a list of prime factors for n, there the 1st element is the factor and
        /// the second the exponent. Includes n if n is a prime
        /// I.e. GetPrimeFactors(20) returns
        ///  { {2, 2}, {5, 1}
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<ulong, ulong>> GetPrimeFactors(ulong n, ulong[] primes = null)
        {
            ulong root = (ulong)Math.Sqrt(n);

            if (primes == null)
            {
                if (root >= 2)
                    primes = GetPrimes(2, root).ToArray();
                else
                    primes = new ulong[] { };
            }

            ulong remainder = n;
            for (int i = 0; i < primes.Length; i++)            
            {
                ulong p = primes[i];
                if (p > root)
                    break;
                
                if (remainder % p == 0)
                {
                    ulong expCount = 0;
                    ulong primeExp = 1;
                    do
                    {
                        primeExp *= p;
                        remainder = remainder / p;
                        expCount++;
                    } while (remainder % p == 0);
                  
                    yield return new Tuple<ulong, ulong>(p, expCount);

                    if (IsPrime(remainder))
                    {
                        yield return new Tuple<ulong, ulong>(remainder, 1);
                        break;
                    }

                    if (remainder == 1)
                        break;
                }
            }

            if (IsPrime(n))
                yield return new Tuple<ulong, ulong>(n, 1);
        }

        /// <summary>
        /// returns all factors of n, including 1 and n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="primes"></param>
        /// <returns></returns>
        public IEnumerable<ulong> GetFactors(ulong n, ulong[] primes = null)
        {
            var factors = new List<ulong>();

            if (primes == null)
            {
                ulong root = (ulong)Math.Sqrt(n);
                if (root >= 2)
                    primes = GetPrimes(2, root).ToArray();
                else
                    primes = new ulong[] { };
            }

            GetFactorsRecursive(1, GetPrimeFactors(n, primes), ref factors);
            
            return factors;
        }

        /// <summary>
        /// Returns the number of primes in the given range
        /// </summary>
        /// <param name="from">Lower bound, inclusive. 0 if omitted</param>
        /// <param name="to">Upper bound, inclusive. set to Sieve Limit if omitted</param>
        /// <returns></returns>
        public ulong CountPrimes(ulong from = 0, ulong to = 0)
        {
            if (to == 0)
                to = Limit;

            if (from > to)
                throw new ArgumentException("from must be smaller than to");
            if (to > Limit)
                throw new ArgumentException("to must be no larger than the limit of the sieve (" + Limit.ToString() + ")");

            ulong count = 0;
            if ((from <= 2) && (to >= 2)) count++;

            ulong start = (from % 2 == 0 ? from + 1 : from);
            for (ulong i = start; i <= to; i += 2)
                if (this[i] == true)
                    count++;

            return count;
        }

        /// <summary>
        /// Calculates the value of the totient function (phi)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public ulong Totient(ulong n)
        {
            ulong result = 1;
            foreach (var f in GetPrimeFactors(n))
                if (f.Item1 > 2)
                    for (ulong i = 1; i <= f.Item2; i++)
                        result *= (f.Item1 - 1);
            return result;
        }

        #endregion
        #region private methods

        /// <summary>
        /// Builds the sieve
        /// </summary>
        private void BuildSieve()
        {
            // compute required size. Don't store multiples of 2, 3 and 5. 
            // this means we need only store 16 numbers for every 60 integers
            // Also, one entry stores 64 bits. I.e. we can store the state of 240 integers with one 64-bit Integer
            int size = (int)Math.Ceiling(Limit / 240.0);
            sieve = new ulong[size];

            // initialize sieve with 1's
            for (int i = 0; i < size; i++)
                sieve[i] = 0xFFFFFFFFFFFFFFFF;

            ulong limitSqrt = (ulong)Math.Ceiling(Math.Sqrt(Limit));
            ulong currentPrime = 7;
            while (currentPrime <= limitSqrt)
            {
                // remove all multiples of the current prime from the sieve. 
                // start at currentPrime^2, all smaller multiples have already been removed
                ulong multiple = currentPrime * currentPrime;
                while (multiple < Limit)
                {
                    // only use the multiple if it is not already declassified as prime
                    if (this[multiple] == true)
                        this[multiple] = false;

                    multiple += currentPrime;
                }

                // move to next potential prime
                currentPrime += 2;
                while ((currentPrime <= limitSqrt) && (this[currentPrime] == false))
                    currentPrime += 2;
            }            
        }

        private void GetFactorsRecursive(ulong currentFactor, IEnumerable<Tuple<ulong, ulong>> primeFactors, ref List<ulong> factors)
        {
            if (!primeFactors.Any())
            {
                factors.Add(currentFactor);
                return;
            }

            var curPrimeFactor = primeFactors.First();
            var nextPrimeFactors = primeFactors.Skip(1).ToArray();
            ulong f = 1;
            for (ulong e = 0; e <= curPrimeFactor.Item2; e++)
            {
                GetFactorsRecursive(currentFactor * f, nextPrimeFactors, ref factors);
                f *= curPrimeFactor.Item1;
            }
        }

        #endregion
    }
}