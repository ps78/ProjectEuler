using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=67
    /// By starting at the top of the triangle below and moving to adjacent numbers on the row below, the maximum total from top to bottom is 23.
    /// 
    /// 3
    /// 7 4
    /// 2 4 6
    /// 8 5 9 3
    /// 
    /// That is, 3 + 7 + 4 + 9 = 23.
    /// 
    /// Find the maximum total from top to bottom in triangle.txt (right click and 'Save Link/Target As...'), a 15K text file containing a triangle with one-hundred rows.
    /// </summary>
    public class Problem067 : EulerProblemBase
    {
        public Problem067() : base(67, "Maximum path sum II", 0, 7273)
        {
            ReadFile(Path.Combine(ResourcePath, "Problem067.txt"));
        }

        public override long Solve(long n)
        {
            int best = 0;

            CalculateHeuristics();

            AStarSearch(0, 0, 0, ref best);

            return best;
        }

        private int[] data;
        private int[] heuristic;
        private int RowCount;

        /// <summary>
        /// returns the value at the given row and column, both 0-based
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public int GetValue(int row, int col)
        {
            return data[row * (row + 1) / 2 + col];
        }

        public int GetHeuristic(int row, int col)
        {
            return heuristic[row * (row + 1) / 2 + col];
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

        private void ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("file '" + fileName + "' not found");

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                RowCount = lines.Length;
                data = new int[(RowCount + 1) * RowCount / 2];                
                int idx = 0;
                foreach (string line in lines.Take(100))
                {
                    var numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var n in numbers)
                        data[idx++] = int.Parse(n);
                }                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error parsing file", ex);
            }
        }

        private void AStarSearch(int row, int col, int pathSumUntilHere, ref int currentBest)
        {
            int currentPathSum = pathSumUntilHere + GetValue(row, col);

            // reached bottom of triangle
            if (row == RowCount - 1)
            {                
                if (currentPathSum > currentBest)
                    currentBest = currentPathSum;
            }
            // do recursive search
            else
            {
                // compute the heuristics of the two possible paths
                int h1 = currentPathSum + GetHeuristic(row + 1, col);
                int h2 = currentPathSum + GetHeuristic(row + 1, col + 1);
                
                // explore the paths only if the heuristic indicates that they can be 
                // better than the current max
                if ((h1 > currentBest) || (h2 > currentBest))
                {
                    // search first subtree first
                    if (h1 > h2)
                    {
                        AStarSearch(row + 1, col, currentPathSum, ref currentBest);                        
                        UpdateHeuristic(row + 1, col, currentBest - currentPathSum);

                        // and then the second
                        if (h2 > currentBest)
                        {
                            AStarSearch(row + 1, col + 1, currentPathSum, ref currentBest);
                            UpdateHeuristic(row + 1, col + 1, currentBest - currentPathSum);
                        }
                    }
                    else
                    // search second subtree first
                    {
                        AStarSearch(row + 1, col + 1, currentPathSum, ref currentBest);
                        UpdateHeuristic(row + 1, col + 1, currentBest - currentPathSum);

                        // and then the first
                        if (h1 > currentBest)
                        {
                            AStarSearch(row + 1, col, currentPathSum, ref currentBest);
                            UpdateHeuristic(row + 1, col, currentBest - currentPathSum);
                        }
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
        private void CalculateHeuristics()
        {
            heuristic = new int[data.Length];

            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col <= row; col++)
                {
                    int sum = 0;
                    int maxCol = col;
                    for (int r = row; r < RowCount; r++)
                    {
                        sum += GetMaxOfRow(r, col, maxCol);
                        maxCol++;
                    }

                    int idx = row * (row + 1) / 2 + col;
                    heuristic[idx] = sum;
                }
        }

        private void UpdateHeuristic(int row, int col, int value)
        {
            int idx = row * (row + 1) / 2 + col;
            heuristic[idx] = value;
        }

    }
}
