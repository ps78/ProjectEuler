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
        /// Also = 
        /// 
        /// All N < 2E6 for which we have 420 lattice points are a multiple of 25
        /// </summary>
        public override long Solve(long n)
        {
            const long L = 420;
            const long start = 359125;
            const long step = 100;

            long N = (long)Math.Pow(10, n);
            long sum = 0;

            for (long i = start; i <= N; i += step)
            {
                if (CheckLatticePoints(i, L))
                {
                    sum += i;
                    Console.WriteLine($"{i}");
                }
            }

            return sum;
        }

        private bool CheckLatticePoints(long N, long Target)
        {
            long count = 4;
            long Nsqr = N * N;
            long Nhalf = N / 2;
            for (long x = 1; x <= Nhalf; x++)
            {
                long t = Nsqr + 4 * x * (N - x);
                if (double.IsInteger(Math.Sqrt(t)))
                {
                    if (x == Nhalf)
                        count += 4;
                    else 
                        count += 8;
                }
                if (count > Target)
                    return false;
            }
            return count == Target;
        }

    }
}
