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
    /// https://projecteuler.net/problem=82
    /// The minimal path sum in the 5 by 5 matrix below, by starting in any cell in the left column and 
    /// finishing in any cell in the right column, and only moving up, down, and right,
    /// is indicated in red and bold; the sum is equal to 994.
    /// 
    /// Find the minimal path sum, in matrix.txt(right click and "Save Link/Target As..."),
    /// a 31K text file containing a 80 by 80 matrix, from the left column to the right column.
    /// </summary>
    public class Problem082 : EulerProblemBase
    {
        #region Fields

        private int[,] Matrix { get; }
        private int NRows { get; }
        private int NCols { get; }

        #endregion        
        #region Public Methods

        public Problem082() : base(82, "Path sum: three ways", 0, 260324)
        {
            Matrix = Path.Combine(ResourcePath, "problem082.txt").ReadMatrix();
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

                isGoalFunc: (x) => (x.Col == NCols - 1),

                getNeighborsFunc: (x) =>
                {
                    var result = new List<(double, (int, int))>();
                    if (x.Col < NCols - 1)
                        result.Add((Matrix[x.Row, x.Col + 1], (x.Row, x.Col + 1)));
                    if (x.Row < NRows - 1)
                        result.Add((Matrix[x.Row + 1, x.Col], (x.Row + 1, x.Col)));
                    if (x.Row > 0)
                        result.Add((Matrix[x.Row - 1, x.Col], (x.Row - 1, x.Col)));
                    return result;
                }
            );

            var startNodes = new List<(int, int)>();
            for (int row = 0; row < NRows; row++)
                startNodes.Add((row, 0));

            var solutions = search.Search(startNodes).ToList();

            // Console.WriteLine("Iteration count: {0:N0}", search.IterationCount);

            return solutions.First().Select(x => Matrix[x.Row, x.Col]).Sum();
        }
        #endregion
        #region Helper functions
        
        private double[,] PreComputeHeuristics()
        {
            var result = new double[NRows, NCols];
            for (int col = NCols - 1; col >= 0; col--)
                if (col < NCols - 1)
                {
                    for (int row = NRows - 1; row >= 0; row--)
                    {
                        double hRight = double.MaxValue, hDown = double.MaxValue;
                        hRight = result[row, col + 1] + Matrix[row, col + 1];
                        if (row < NRows - 1) hDown = result[row + 1, col] + Matrix[row + 1, col];
                        result[row, col] = Math.Min(hRight, hDown);
                    }
                    for (int row = 0; row < NRows; row++)
                    {
                        double hRight = double.MaxValue, hUp = double.MaxValue;
                        hRight = result[row, col + 1] + Matrix[row, col + 1];
                        if (row > 0) hUp = result[row - 1, col] + Matrix[row - 1, col];
                        result[row, col] = Math.Min(result[row, col], Math.Min(hUp, hRight));
                    }
                }
            return result;
        }
        
        #endregion
    }
}
