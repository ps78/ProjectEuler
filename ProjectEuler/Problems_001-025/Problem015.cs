using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=15
    ///     
    /// Starting in the top left corner of a 2×2 grid, and only being able to move to the right and down, there are exactly 6 routes to the bottom right corner.
    /// ow many such routes are there through a 20×20 grid?
    /// </summary>
    public class Problem015 : EulerProblemBase
    {
        public Problem015() : base(15, "Lattice paths", 20, 137846528820) { }

        public override bool Test() => Solve(2) == 6;
        
        public override long Solve(long n)
        {
            // the solution is (2n)! / (n!)^2 where n is the grid size
            // = product 2n/n * 2n-1/n-1 * 2n-2/n-2 * ... * n+1/1

            double product = 1;
            double d = n;
            for (ulong i = 0; i < d; i++)
                product *= (2 * d - i) / (d - i);

            return (long)product;
        }
        
    }
}
