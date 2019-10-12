using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=85
    /// By counting carefully it can be seen that a rectangular grid measuring 3 by 2 contains eighteen rectangles:
    /// 
    /// Although there exists no rectangular grid that contains exactly two million rectangles, 
    /// find the area of the grid with the nearest solution.
    /// </summary>
    public class Problem085 : EulerProblemBase
    {                
        #region Public Methods

        public Problem085() : base(85, "Counting rectangles", 2_000_000, 2772) { }

        public override bool Test() => Solve(18) == 6;

        public override long Solve(long n)
        {
            // a nxm lattice has [1/2 * (n^2+n)] * [1/2 * (m^2+m)] rectangles
            // compute all numbers 1/2*(i^2+i) that are < 2 million
            var numbers = new List<Tuple<int, int>>();
            int N = (int)n;
            for (int i = 1; i > 0; i++)
            { 
                int number = (i * i + i) / 2;
                if (number <= N + 1000)
                    numbers.Add(new Tuple<int, int>(number, i));
                else
                    break;
            }

            // test all combinations n x m that are near the target, with n and m part from the list of numbers generated
            int bestSolution = int.MaxValue, bestN = 0, bestM = 0, rectCount = 0;
            foreach (var t in numbers)
            {
                // choose m such that n x m ~ 2 mio
                int b = N / t.Item1; // find the 2 numbers that are nearest to b1
                
                for (int i = 0; i < numbers.Count-1; i++)
                {
                    if (numbers[i + 1].Item1 > b)
                    {
                        int solution = t.Item1 * numbers[i].Item1;
                        if (Math.Abs(N - solution) < bestSolution)
                        {
                            bestSolution = Math.Abs(N - solution);
                            bestN = t.Item2;
                            bestM = numbers[i].Item2;
                            rectCount = solution;
                        }
                        if (numbers[i].Item1 > b)
                            break;
                    }
                }                         
            }

            //Console.WriteLine("{0} x {1} = {2}, rect.count = {3}", bestN, bestM, bestN * bestM, rectCount);

            return bestN * bestM;
        }

        #endregion
        #region Private Methods

        private int CountRectangles(int n, int m) => (n * n + n) * (m * m + m) / 4;        

        #endregion
    }
}
