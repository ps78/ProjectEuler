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
    /// https://projecteuler.net/problem=83
    /// In the 5 by 5 matrix below, the minimal path sum from the top left to the bottom right, by moving 
    /// left, right, up, and down, is indicated in bold red and is equal to 2297.
    /// 
    /// In the 5 by 5 matrix below, the minimal path sum from the top left to the 
    /// bottom right, by moving left, right, up, and down, is indicated in bold red and is equal to 2297.
    /// </summary>
    public class Problem083 : EulerProblemBase
    {
        #region Fields

        private int[,] Matrix { get; }
        private int NRows { get; }
        private int NCols { get; }

        #endregion        
        #region Public Methods

        public Problem083(): base(83, "Path sum: four ways", 0, 425185)
        {
            Matrix = Path.Combine(ResourcePath, "problem083.txt").ReadMatrix();
            /*
            Matrix = new int[,]{
                { 131, 673, 234, 103,  18 },
                { 201,  96, 342, 965, 150 },
                { 630, 803, 746, 422, 111 },
                { 537, 699, 497, 121, 956 },
                { 805, 732, 524,  37, 331 }
            };
            */
            NRows = Matrix.GetLength(0);
            NCols = Matrix.GetLength(1);
        }

        public override long Solve(long n)
        {
            var heuristicValues = PreComputeHeuristics();

            var search = new AStarSearch<(int Row, int Col)>
            (
                heuristicFunc: (x) => heuristicValues[x.Row, x.Col],

                costFunc: (x) => Matrix[x.Row, x.Col],

                isGoalFunc: (x) => (x.Row == NRows - 1) && (x.Col == NCols - 1),

                getNeighborsFunc: (x) =>
                {
                    var result = new List<(double, (int, int))>();
                    if (x.Col < NCols - 1)
                        result.Add((Matrix[x.Row, x.Col + 1], (x.Row, x.Col + 1)));
                    if (x.Col > 0)
                        result.Add((Matrix[x.Row, x.Col - 1], (x.Row, x.Col -  1)));
                    if (x.Row < NRows - 1)
                        result.Add((Matrix[x.Row + 1, x.Col], (x.Row + 1, x.Col)));
                    if (x.Row > 0)
                        result.Add((Matrix[x.Row - 1, x.Col], (x.Row - 1, x.Col)));
                    return result;
                }
            );

            var solutions = search.Search(new(int, int)[] { (0, 0) }).ToList();

            //Console.WriteLine("Iteration count: {0:N0}", search.IterationCount);

            return solutions.First().Select(x => Matrix[x.Row, x.Col]).Sum();
        }
        #endregion
        #region Helper functions
        
        private double[,] PreComputeHeuristics()
        {
            double[] rowMins = new double[NRows];
            for (int row = 0; row < NRows; row++)                
            {
                double min = double.MaxValue;
                for (int col = 0; col < NCols; col++)
                    min = Math.Min(min, Matrix[row, col]);
            }
            double[] colMins = new double[NCols];
            for (int col = 0; col < NCols; col++)
            {
                double min = double.MaxValue;
                for (int row = 0; row < NRows; row++)
                    min = Math.Min(min, Matrix[row, col]);
            }

            var result = new double[NRows, NCols];

            for (int row = NRows - 1 ; row >= 0; row--)
                for (int col = NCols - 1; col >= 0; col--)
                    if (!(row == NRows - 1 && col == NCols - 1))
                    {
                        double rsum = 0;
                        for (int r = row + 1; r < NRows; r++)
                            rsum += rowMins[r];
                        double csum = 0;
                        for (int c = col + 1; c < NCols; c++)
                            csum += colMins[c];                        

                        result[row, col] = Math.Max(rsum, csum);
                    }
            return result;
        }
        
        #endregion
    }
}
