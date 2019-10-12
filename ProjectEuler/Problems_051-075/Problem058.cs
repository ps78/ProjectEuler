using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=58
    /// Starting with 1 and spiralling anticlockwise in the following way, a square spiral with side length 7 is formed.
    /// 
    /// 37 36 35 34 33 32 31
    /// 38 17 16 15 14 13 30
    /// 39 18  5  4  3 12 29
    /// 40 19  6  1  2 11 28
    /// 41 20  7  8  9 10 27
    /// 42 21 22 23 24 25 26
    /// 43 44 45 46 47 48 49
    /// 
    /// It is interesting to note that the odd squares lie along the bottom right diagonal, but what is more 
    /// interesting is that 8 out of the 13 numbers lying along both diagonals are prime; that is, a ratio of 8/13 ≈ 62%.
    /// If one complete new layer is wrapped around the spiral above, a square spiral with side length 9 will be formed.
    /// If this process is continued, what is the side length of the square spiral for which the ratio of 
    /// primes along both diagonals first falls below 10%?
    /// </summary>
    public class Problem058 : EulerProblemBase
    {
        public Problem058() : base(58, "Spiral primes", 0, 26241) { }

        public override long Solve(long n)
        {
            var ptest = new SimplePrimeTest();

            int countPrimes = 0;
            int countAll = 1;
            int d = 3;
            while (true)
            {
                int a = d * d;
                int m = d - 1;
                
                countAll += 4;
                countPrimes += ((ptest.IsPrime((ulong)(a - m)) ? 1 : 0) + (ptest.IsPrime((ulong)(a - 2*m)) ? 1 : 0) + (ptest.IsPrime((ulong)(a - 3*m)) ? 1 : 0));

                if (countPrimes * 10 < countAll)
                    break;

                d += 2;
            }
            
            return d ;
        }
    }
}
