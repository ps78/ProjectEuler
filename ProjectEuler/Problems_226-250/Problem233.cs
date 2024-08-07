using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=233
    /// 
    /// Let f(N) be the number of points with integer coordinates that are on a circle 
    /// passing through (0,0), (0,N), (N,0) and (N,N).
    /// 
    /// It can be shown that f(10'000) = 36
    /// 
    /// What is the sum of all positive integers N <= 10^11 such that f(N) = 420?
    /// </summary>
    public class Problem233 : EulerProblemBase
    {
        public Problem233() : base(233, "Lattice Points on a Circle", 7, 0) { }

        private SieveOfEratosthenes Sieve = new SieveOfEratosthenes(1_000_000);

        /// <summary>
        /// Circle radius r = Sqrt(N^2/2) = N/sqrt(2)
        /// 
        /// Equation of circle with center in (0,0): x^2 + y^2 = r^2
        /// Equation of circle with center at (N/2,N/2):  (x-N/2)^2 + (y-N/2)^2 = r^2 = N^2/2
        /// 
        /// transformed: 2(x-N/2)^2 + 2(y-N/2)^2                 = N^2
        ///              2x^2 - 2Nx + N^2/2 + 2y^2 - 2Ny + N^2/2 = N^2
        ///                              2x^2 + 2y^2 - 2Nx - 2Ny = 0
        ///                                          x^2 + y^2   = N(x+y)
        /// 
        /// solving for y:
        /// 
        /// y^2 - Ny + x^2 - Nx = 0
        /// 
        /// y = (N +/- Sqrt[N^2 - 4(x^2-Nx)]) / 2
        /// 
        /// sqlr(N^2 - 4x(x-N))
        /// There is an 8-fold symmmetry, we only need to consider 
        ///     0 < x < N/2, N/2-Sqrt(N^2/2) < y < 0
        ///     
        /// In addition, the 4 points given lie on the circle
        /// Additionally, the 4 points at X=N/2 and y=N/2 may lie on the circle
        /// 
        /// In our problem, we are looing for circles with 420 lattice points, or 416 plus the 
        /// given 4. 416 is divisible by 8 (416/8=52). Hence the circles we are looking for
        /// must NOT have a lattice point at (x,y)=(N/2,N/2-Sqrt(N^2/2).
        /// 
        /// There must be exactly 52 solutions for y for x in the range [1..N/2) 
        /// and x=N/2 must not be a solution.
        /// condition is that N^2-4(x^2-Nx) = N^2+4x(N-x) is a square number
        /// 
        /// let Z = Sqrt[N^2+4x(N-x)]
        /// then Z^2 - N^2 = 4x(N-x) = (Z+N)(Z-N)
        /// 
        /// All N < 2E6 for which we have 420 lattice points are a multiple of 25
        /// 
        /// Maybe this helps: https://mathworld.wolfram.com/CircleLatticePoints.html
        /// 
        /// For our problem, we have a circle with radius of Sqrt(N^2/2). And the center
        /// can be in the center between two lattice points.
        /// For N^2/2 to be an integer, N^2 needs to be even => N needs to be even.
        /// This is not the case, hence we scale the problem by factor of 2: M = N^2/2.
        /// We need to solve the problem for M = 
        /// 
        /// With this new problem, this video https://www.youtube.com/watch?v=NaL_Cb42WyY
        /// explains how to count lattice points on circles with radius Sqrt(M) where M is an integer.
        /// 
        /// Recipe: Given the task is to find the number of lattice points on a circle with radius Sqrt(M), 
        /// M integer, and 0,0 at the center, factor M into prime factors
        /// 
        /// 1) for any prime factor of the form 4k-1 (3, 7, 11, 19,..): 
        ///         if the exponent is even, take 1 as factor
        ///         if the exponent is odd, there are no lattice points on the circle
        /// 2) for any prime factor of the form 4k+1 (5, 13, 17, 29, ..): 
        ///         take it's exponent+1 as factor
        /// 3) for the prime factor of 2, take a factor of 1 regardless of the exponent
        /// 
        /// The product of these factors is the number of lattice points on the circle
        /// 
        /// For L=420, removing factors 2^k, the remaining 105 can be formed as:
        ///   105 = 3 * 5 * 7
        ///   105 = 3 * 35
        ///   105 = 5 * 21
        ///   105 = 7 * 15
        ///   105 = 105
        /// </summary>
        public override long Solve(long n)
        {            
            const long L = 420;
            const long start = 3232125; // 359125;
            const long step = 100;

            long N = (long)Math.Pow(10, n);
            long sum = 0;

            for (long i = start; i <= N; i += step)
            {
                if (CountLatticePoints(i) == L)
                {
                    sum += i;
                    var factors = Sieve.GetPrimeFactors((ulong)i).Select(f => f.Item2 == 1 ? $"{f.Item1}" : $"{f.Item1}^{f.Item2}");
                    Console.WriteLine($"{i} = {string.Join(' ', factors)}");
                }
            }

            return sum;
        }

        private long CountLatticePoints(long N)
        {
            // we actually need to do this for M = (2N)^2/2
            // this in fact doubles all exponents, hence we cannot have any odd exponents
            var factors = Sieve.GetPrimeFactors((ulong)N);
            ulong product = 4;
            foreach(var factor in factors)
            {
                if (factor.Item1 > 2)
                {
                    // is it 4k+1 ?
                    if ((factor.Item1 - 1) % 4 == 0)
                    {
                        product *= (2*factor.Item2 + 1);
                    }
                }
            }
            return (long)product;
        }

    }
}
