using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SudokuGame
{
    /// <summary>
    /// Class to generate Sudokus
    /// </summary>
    public class Generator
    {
        #region Data Fields

        private Random rand = new Random();
        private SudokuSolver solver = new SudokuSolver();

        #endregion
        #region Public Methods

        /// <summary>
        /// Generates the given number of random Sudokus with the required number of empty positions
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="amountToGenerate"></param>
        /// <param name="clueCount"></param>
        /// <returns></returns>
        public IList<Sudoku> GenerateSudokus(SudokuLayout layout, SudokuCharacters charSet, int amountToGenerate, int clueCount)
        {
            ConcurrentBag<Sudoku> results = new ConcurrentBag<Sudoku>();

            Parallel.For(0, amountToGenerate, (i) =>
            {
                var subresults = new HashSet<Sudoku>();
                Sudoku s = solver.Solve(new Sudoku(layout, charSet), SudokuSolver.SearchMode.Random, 1).First();

                var state = s.BackupState();
                RecursiveGenerate(ref subresults, s, 1, clueCount);
                s.RestoreState(state);

                foreach (var sudoku in subresults)
                {
                    sudoku.Solution = s;
                    results.Add(sudoku);
                }
            });
            
            int n = 1;
            foreach (var s in results)
            {
                s.Number = n++;
                s.Solution.Number = s.Number;
            }

            return results.ToList();
        }

        #endregion
        #region Private Methods

        private void RecursiveGenerate(ref HashSet<Sudoku> solutions, Sudoku current, int maxSolutions, int clueCount)
        {
            if (current.ClueCount == clueCount)
                solutions.Add(new Sudoku(current));
            else
            {
                var positions = GetRandomSolvedIndexVector(current);
                foreach (var pos in positions)
                {
                    byte prevValue = current.State[pos];
                    current.State.Clear(pos);
                    if (solver.HasUniqueSolution(current))
                    {
                        RecursiveGenerate(ref solutions, current, maxSolutions, clueCount);
                        if (solutions.Count >= maxSolutions)
                            return;
                    }
                    else
                        current.State.TrySet(pos, prevValue);
                }
            }
        }

        /// <summary>
        /// Returns the indexes of the solved positions (not empty) in a random order
        /// </summary>
        /// <returns></returns>
        private int[] GetRandomSolvedIndexVector(Sudoku s)
        {
            var list = new List<Tuple<int, double>>();
            for (int idx = 0; idx < s.Layout.FieldCount; idx++)
                if (s.State[idx] != 0)
                    list.Add(new Tuple<int, double>(idx, rand.NextDouble()));

            list.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            return list.Select(x => x.Item1).ToArray();
        }

        #endregion
    }
}
