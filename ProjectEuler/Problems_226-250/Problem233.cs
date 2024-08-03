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
        public Problem233() : base(233, "Lattice Points on a Circle", 4, 0) { }

        /// <summary>
        /// Circle radius r = Sqrt(N^2/2)
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
        /// </summary>
        public override long Solve(long n)
        {
            long N = (long)Math.Pow(10, n);
            var hist = new Dictionary<long, List<long>>();

            for (long i = 1; i <= N; i++)
            {
                long latticePoints = CountLatticePoints(i);

                if (latticePoints != 4) // 4 is the trivial case, ignore this
                {
                    if (hist.TryGetValue(latticePoints, out List<long>? lst))
                        lst.Add(i);
                    else
                        hist[latticePoints] = [i];
                }
            }

            foreach (var entry in hist.OrderBy(kv => kv.Key))
            {
                var y = entry.Value;
                var values = string.Join(",", entry.Value.Select(x => x.ToString()));
                Console.WriteLine($">>{entry.Key}<<: {values}\n");
            }

            return 0;
        }

        private long CountLatticePoints(long N)
        {
            long count = 0;
            long N2 = N * N;
            for (long x = 0; x < N; x++)
            {
                long t = N2 - 4 * (x * x - N * x);
                long root = (long)Math.Sqrt(t);
                if (root * root == t)
                    count++;
            }
            return 4 * count;
        }

    }
}
