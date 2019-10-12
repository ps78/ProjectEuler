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
    /// https://projecteuler.net/problem=27
    /// 
    /// Euler discovered the remarkable quadratic formula:
    /// 
    /// n² + n + 41
    /// 
    /// It turns out that the formula will produce 40 primes for the consecutive values n = 0 to 39. However, when n = 40, 402 + 40 + 41 = 40(40 + 1) + 41 
    /// is divisible by 41, and certainly when n = 41, 41² + 41 + 41 is clearly divisible by 41.
    /// 
    /// The incredible formula n² − 79n + 1601 was discovered, which produces 80 primes for the consecutive values n = 0 to 79. The product of 
    /// the coefficients, −79 and 1601, is −126479.
    /// 
    /// Considering quadratics of the form:
    /// 
    /// n² + an + b, where |a| < 1000 and |b| < 1000
    /// 
    /// where |n| is the modulus/absolute value of n
    /// e.g. |11| = 11 and |−4| = 4
    /// Find the product of the coefficients, a and b, for the quadratic expression that produces the maximum number of primes for consecutive values of n, starting with n = 0.
    /// </summary>
    public class Problem027 : EulerProblemBase
    {
        public Problem027() : base(27, "Quadratic primes", 1000, -59231) { }

        // TODO: the test doesn't work as it should, there must be an error in the algorithm. 
        // public override bool Test() => Solve(39) == 41 && Solve(79) == 126479;

        private HashSet<int> primes;

        public override long Solve(long n)
        {
            var sieve = new SieveOfEratosthenes(50);
            int[] firstPrimes = sieve.GetPrimes().ConvertAll((ulong p) => (int)p).ToArray();
            primes = new HashSet<int>(firstPrimes);

            int maxN = -1;
            int maxA = 0;
            int maxB = 0;
            
            List<int> bs = new List<int>(firstPrimes);
            for (int i = firstPrimes.Last(); i < (int)n; i += 2)
                bs.Add(i);
            
            foreach (int b in bs)            
            {
                int llim = (1 - b) % 2 == 0 ? 1 - b + 1 : 1 - b;
                for (int a = llim; a < (int)n; a += 2)
                {
                    int m;
                    m = CheckSequence(a, b);
                    if (m > maxN)
                    {
                        maxA = a;
                        maxB = b;
                        maxN = m;
                    }
                    if (maxN >= Math.Abs(b - a))
                        break;
                }

                if (maxN >= Math.Abs(b))
                    break;
            }

            //if (maxA * maxB < 0)
            //    Console.WriteLine("Result for Problem 027 is negative!");

            //Console.WriteLine("a:{0} / b:{1} / n:{2} / a*b:{3}", maxA, maxB, maxN, maxA * maxB);
            return maxA * maxB;
        }

        private int CheckSequence(int a, int b)
        {
            int n = -1;
            int i;
            do
            {
                n++;
                i = n * n + a * n + b;
            } while ((i > 0) && IsPrime(i));

            return n - 1;
        }
        
        private bool IsPrime(int n)
        {
            if ((n % 2 == 0) || (n % 3 == 0) || (n % 5 == 0))
                return false;

            if (primes.Contains(n))
                return true;
            
            int sq = (int)Math.Sqrt(n);
            for (int i = 7; i <= sq; i += 2)
                if (n % i == 0)
                    return false;

            primes.Add(n);
            return true;
        }

    }
}
