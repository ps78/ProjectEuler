using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SudokuGame
{
    public class SudokuSolver
    {
        #region Types

        public enum SearchMode
        {
            Fast,
            Random
        }

        public class SolverStatistics
        {
            public int AllTimeRecursionCount = 0;
            public int LastRecursionCount = 0;
            public TimeSpan AllTimeSolverTime = TimeSpan.Zero;
            public TimeSpan LastSolverTime = TimeSpan.Zero;            
        }

        #endregion
        #region Private Fields

        private static Random rand = new Random();

        private SolverStatistics stats = new SolverStatistics();

        private Stopwatch clock = new Stopwatch();

        #endregion
        #region Properties

        public SolverStatistics Stats {  get { return stats; } }

        #endregion
        #region Public Methods

        /// <summary>
        /// Solves the Sudoku and returns a set with all solutions.
        /// A maximum of maxSolutions solutions will be returned
        /// </summary>
        /// <returns></returns>
        public HashSet<Sudoku> Solve(Sudoku s, SearchMode searchMode = SearchMode.Fast, int maxSolutions = -1)
        {
            var backup = s.BackupState();

            HashSet<Sudoku> solutions = new HashSet<Sudoku>();

            stats.LastRecursionCount = 0;
            clock.Restart();
            if (maxSolutions != 0)
                RecursiveSolve(s, ref solutions, searchMode, maxSolutions);
            clock.Stop();

            s.RestoreState(backup);

            if (solutions.Count > 0)
            {
                s.Solution = solutions.First();
                s.Solution.Number = s.Number;
            }

            stats.AllTimeSolverTime.Add(clock.Elapsed);
            stats.LastSolverTime = clock.Elapsed;
            stats.AllTimeRecursionCount += stats.LastRecursionCount;

            return solutions;
        }

        /// <summary>
        /// Checks if Sudoku has exactly one solution
        /// </summary>
        /// <returns></returns>
        public bool HasUniqueSolution(Sudoku s)
        {
            var solutions = Solve(s, SearchMode.Fast, 2);
            return (solutions.Count == 1);
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Recursive, intelligent backtracking solver. Always explores sub-trees with minimum number of possible states first
        /// </summary>
        /// <param name="s"></param>
        /// <param name="solutions"></param>
        /// <param name="searchMode"></param>
        /// <param name="maxSolutionCount"></param>
        private void RecursiveSolve(Sudoku s, ref HashSet<Sudoku> solutions, SearchMode searchMode = SearchMode.Fast, int maxSolutionCount = -1)
        {
            stats.LastRecursionCount++;

            if (s.ClueCount == s.Layout.FieldCount)
                solutions.Add(new Sudoku(s));
            else
            {
                byte[] possibleValues;
                Int2D nextPos;
                switch (searchMode)
                {
                    case SearchMode.Fast: nextPos = GetFirstBestEmptyPosition(s, out possibleValues); break;
                    case SearchMode.Random: nextPos = GetRandomBestEmptyPosition(s, out possibleValues); break;
                    default: throw new InvalidOperationException("Unhandled search mode");
                }

                foreach (byte b in possibleValues)
                    if (s.State.TrySet(nextPos.Row, nextPos.Col, b))
                    {
                        RecursiveSolve(s, ref solutions, searchMode, maxSolutionCount);
                        if ((maxSolutionCount > 0) && (solutions.Count >= maxSolutionCount))
                            return;
                        s.State.Clear(nextPos.Row, nextPos.Col);
                    }
            }
        }

        /// <summary>
        /// Returns the next position that is empty and has the fewest possible values
        /// The values possible at that position are returned as well
        /// </summary>
        /// <param name="PossibleValues"></param>
        /// <returns></returns>
        private static Int2D GetFirstBestEmptyPosition(Sudoku s, out byte[] PossibleValues)
        {
            PossibleValues = null;

            var st = s.State;
            int bestPosIdx = -1;
            int minPossibleValues = int.MaxValue;
            ulong bestNotSetBits = 0;
            for (int idx = 0; idx < s.Layout.FieldCount; idx++)
                if (st[idx] == 0)
                {
                    ulong notSetBits = ~st.RowStates[idx / s.Layout.SideLength] & ~st.ColStates[idx % s.Layout.SideLength] & ~st.BlockStates[st.BlockIndex[idx]];
                    int numberOfBitsNotSet = notSetBits.CountBits(s.Layout.SideLength);
                    if (numberOfBitsNotSet < minPossibleValues)
                    {
                        minPossibleValues = numberOfBitsNotSet;
                        bestPosIdx = idx;
                        bestNotSetBits = notSetBits;
                    }
                    if (minPossibleValues == 1)
                        break;
                }

            if (bestPosIdx == -1)
                return Int2D.Undefined;
            else
            {
                PossibleValues = bestNotSetBits.GetBits(s.Layout.SideLength);
                for (int i = 0; i < PossibleValues.Length; i++)
                    PossibleValues[i] = (byte)(PossibleValues[i] + 1);
                return new Int2D(bestPosIdx / s.Layout.SideLength, bestPosIdx % s.Layout.SideLength);
            }
        }

        /// <summary>
        /// Returns randomly one of the best empty positions. The best empty positions 
        /// are those with minimal number of possible values.
        /// Also returns the possible values of the seleted position
        /// </summary>
        /// <param name="PossibleValues"></param>
        /// <returns></returns>
        private static Int2D GetRandomBestEmptyPosition(Sudoku s, out byte[] PossibleValues)
        {
            PossibleValues = null;

            var st = s.State;
            List<Tuple<int, int, ulong>> bestPositions = new List<Tuple<int, int, ulong>>();
            int minPossibleValues = int.MaxValue;
            for (int idx = 0; idx < s.Layout.FieldCount; idx++)
                if (st[idx] == 0)
                {
                    ulong notSetBits = ~st.RowStates[idx / s.Layout.SideLength] & ~st.ColStates[idx % s.Layout.SideLength] & ~st.BlockStates[st.BlockIndex[idx]];
                    int numberOfBitsNotSet = notSetBits.CountBits(s.Layout.SideLength);
                    if (numberOfBitsNotSet <= minPossibleValues)
                    {
                        bestPositions.Add(new Tuple<int, int, ulong>(idx, numberOfBitsNotSet, notSetBits));
                        minPossibleValues = numberOfBitsNotSet;
                    }
                }

            if (bestPositions.Count == 0)
                return Int2D.Undefined;
            else
            {
                var bestSolutions = bestPositions.Where(x => x.Item2 == minPossibleValues).ToArray();
                var selected = bestSolutions[rand.Next(0, bestSolutions.Length)];

                PossibleValues = selected.Item3.GetBits(s.Layout.SideLength);
                for (int i = 0; i < PossibleValues.Length; i++)
                    PossibleValues[i] = (byte)(PossibleValues[i] + 1);
                return new Int2D(selected.Item1 / s.Layout.SideLength, selected.Item1 % s.Layout.SideLength);
            }
        }

        #endregion
    }
}
