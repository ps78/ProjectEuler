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
    /// https://projecteuler.net/problem=81
    /// n the 5 by 5 matrix below, the minimal path sum from the top left to the bottom right, by only moving to the right and down, 
    /// is indicated in bold red and is equal to 2427.
    /// Find the minimal path sum, in matrix.txt(right click and "Save Link/Target As..."), a
    /// 31K text file containing a 80 by 80 matrix, from the top left to the bottom right by only moving right and down.
    /// </summary>
    public class Problem081 : EulerProblemBase
    {        
        #region Fields

        private int[,] Matrix { get; }
        private int NRows { get; }
        private int NCols { get; }

        #endregion        
        #region Public Methods

        public Problem081(): base(81, "Path sum: two ways", 0, 427337)
        {
            Matrix = Path.Combine(ResourcePath, "problem081.txt").ReadMatrix();
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
                    if (x.Row < NRows - 1)
                        result.Add((Matrix[x.Row + 1, x.Col], (x.Row + 1, x.Col)));
                    return result;
                }
            );

            var solutions = search.Search(new(int, int)[] { (0, 0) }).ToList();

            //Console.WriteLine("Iteration count: {0:N0}", search.IterationCount);

            return (long)solutions.First().Select(x => Matrix[x.Row, x.Col]).Sum();
        }
        #endregion
        #region Helper functions
        
        private double[,] PreComputeHeuristics()
        {
            var result = new double[NRows, NCols];
            for (int row = NRows - 1; row >= 0; row--)
                for (int col = NCols - 1; col >= 0; col--)
                    if (!(row == NRows - 1 && col == NCols - 1))
                    {
                        double hRight = double.MaxValue, hDown = double.MaxValue;
                        if (col < NCols - 1) hRight = result[row, col + 1] + Matrix[row, col + 1];
                        if (row < NRows - 1) hDown = result[row + 1, col] + Matrix[row + 1, col];

                        result[row, col] = (hRight < hDown ? hRight : hDown);
                    }
            return result;
        }

        private int[,] SubMatrix(int[,] m, int rows, int cols)
        {
            var tmp = new int[rows, cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    tmp[row, col] = m[row, col];
            return tmp;
        }

        #endregion
    }
}
