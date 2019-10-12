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
    /// https://projecteuler.net/problem=18
    ///     
    /// By starting at the top of the triangle below and moving to adjacent numbers on the row below, the maximum total from top to bottom is 23.
    ///    3
    ///   7 4
    ///  2 4 6
    /// 8 5 9 3
    /// That is, 3 + 7 + 4 + 9 = 23.
    /// Find the maximum total from top to bottom of the triangle below
    /// </summary>
    public class Problem018 : EulerProblemBase
    {
        public Problem018() : base(18, "Maximum path sum I", 15, 1074) { }

        public override bool Test()
        {
            var tmp = data;
            data = new int[]
            { 3,
              7, 4,
              2, 4, 6,
              8, 5, 9, 3
            };
            bool result = Solve(4) == 23;
            data = tmp;
            return result;
        }

        #region data

        private int[] data = new int[]
            { 75,
              95, 64,
              17, 47, 82,
              18, 35, 87, 10,
              20, 04, 82, 47, 65,
              19, 01, 23, 75, 03, 34,
              88, 02, 77, 73, 07, 63, 67,
              99, 65, 04, 28, 06, 16, 70, 92,
              41, 41, 26, 56, 83, 40, 80, 70, 33,
              41, 48, 72, 33, 47, 32, 37, 16, 94, 29,
              53, 71, 44, 65, 25, 43, 91, 52, 97, 51, 14,
              70, 11, 33, 28, 77, 73, 17, 78, 39, 68, 17, 57,
              91, 71, 52, 38, 17, 14, 91, 43, 58, 50, 27, 29, 48,
              63, 66, 04, 68, 89, 53, 67, 30, 73, 16, 69, 87, 40, 31,
              04, 62, 98, 27, 23, 09, 70, 98, 73, 93, 38, 53, 60, 04, 23
            };

        #endregion

        public override long Solve(long n)
        {
            int best = 0;

            AStarSearch(0, 0, 0, (int)n, ref best);

            return (long)best;
        }

        /// <summary>
        /// returns the value at the given row and column, both 0-based
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public int GetValue(int row, int col)
        {
            return data[row * (row + 1) / 2 + col];
        }

        /// <summary>
        /// Returns the maximum value between startCol and endCol (inclusive)
        /// on the given row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="startCol"></param>
        /// <param name="endCol"></param>
        /// <returns></returns>
        public int GetMaxOfRow(int row, int startCol, int endCol)
        {
            int result = 0;
            int startIdx = row * (row + 1) / 2 + startCol;
            int endIdx = startIdx + (endCol - startCol);
            
            for (int i = startIdx; i <= endIdx; i++)
                result = Math.Max(result, data[i]);

            return result;
        }

        private void AStarSearch(int row, int col, int pathSumUntilHere, int rowCount, ref int currentBest)
        {
            int currentPathSum = pathSumUntilHere + GetValue(row, col);

            // reached bottom of triangle
            if (row == rowCount - 1)
            {                
                if (currentPathSum > currentBest)
                    currentBest = currentPathSum;
            }
            // do recursive search
            else
            {
                // compute the heuristics of the two possible paths
                int h1 = currentPathSum + Heuristic(row + 1, col, rowCount);
                int h2 = currentPathSum + Heuristic(row + 1, col + 1, rowCount);
                
                // explore the paths only if the heuristic indicates that they can be 
                // better than the current max
                if ((h1 > currentBest) || (h2 > currentBest))
                {
                    // search first subtree first
                    if (h1 > h2)
                    {
                        AStarSearch(row + 1, col, currentPathSum, rowCount, ref currentBest);
                        if (h2 > currentBest)
                            AStarSearch(row + 1, col + 1, currentPathSum, rowCount, ref currentBest);
                    }
                    else
                    // search second subtree first
                    {
                        AStarSearch(row + 1, col + 1, currentPathSum, rowCount, ref currentBest);
                        if (h1 > currentBest)
                            AStarSearch(row + 1, col, currentPathSum, rowCount, ref currentBest);
                    }
                }
            }
        }

        /// <summary>
        /// computes the heuristic of the sub-triangle with top corner at the 
        /// given row, col
        /// The heuristic is the maximum possible path sum, computed by taking tha maximum of each row
        /// This can be used for the A* tree search
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int Heuristic(int row, int col, int rowCount)
        {
            int sum = 0;
            int maxCol = col;
            for (int r = row; r < rowCount; r++)
            {
                sum += GetMaxOfRow(r, col, maxCol);
                maxCol++;
            }
            return sum;
        }

    }
}
