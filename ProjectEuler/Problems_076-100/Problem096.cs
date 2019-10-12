using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SudokuGame;

namespace ProjectEuler
{    
    /// <summary>
    /// Solves https://projecteuler.net/problem=096
    /// Su Doku (Japanese meaning number place) is the name given to a popular puzzle concept. Its origin is unclear, but credit must be attributed to 
    /// Leonhard Euler who invented a similar, and much more difficult, puzzle idea called Latin Squares. The objective of Su Doku puzzles, however, 
    /// is to replace the blanks (or zeros) in a 9 by 9 grid in such that each row, column, and 3 by 3 box contains each of the digits 1 to 9. Below 
    /// is an example of a typical starting puzzle grid and its solution grid.
    /// ...
    /// A well constructed Su Doku puzzle has a unique solution and can be solved by logic, although it may be necessary to employ "guess and test" 
    /// methods in order to eliminate options(there is much contested opinion over this). The complexity of the search determines the difficulty of the puzzle; 
    /// the example above is considered easy because it can be solved by straight forward direct deduction.
    ///
    /// The 6K text file, sudoku.txt (right click and 'Save Link/Target As...'), contains fifty different Su Doku puzzles ranging in difficulty, 
    /// but all with unique solutions(the first puzzle in the file is the example above).
    ///
    /// By solving all fifty puzzles find the sum of the 3-digit numbers found in the top left corner of each solution grid; for example, 
    /// 483 is the 3-digit number found in the top left corner of the solution grid above.
    /// </summary>
    public class Problem096 : EulerProblemBase
    {
        #region data
        #endregion
        #region Fields

        #endregion
        #region Private Methods

        #endregion
        #region Public Methods

        public Problem096() : base(96, "Su Doku", 0, 24702) { }

        public override long Solve(long n)
        {
            var originalSudokus = Sudoku.ReadFromFile(Path.Combine(ResourcePath, "problem096.txt"), SudokuLayout.Layout9x9, SudokuCharacters.Sudoku9Set);

            //foreach (var s in originalSudokus)
            //   Console.WriteLine(s);

            var solver = new SudokuSolver();

            ConcurrentBag<Sudoku> solvedSudokus = new ConcurrentBag<Sudoku>();
            Parallel.ForEach(originalSudokus, (s) =>
            {
                var solutions = solver.Solve(s);
                if (solutions.Count() != 1)
                    Console.WriteLine("Sudoku {0} has {1} solutions!", s.Number, solutions.Count());
                else
                    solvedSudokus.Add(solutions.First());
            });
            
            //foreach (var sol in solvedSudokus.OrderBy((x) => x.Id))
            //    Console.WriteLine(sol);
            
            return solvedSudokus.Sum((s) =>  int.Parse(s[0, 0].ToString()) * 100 + 
                                             int.Parse(s[0, 1].ToString()) * 10 + 
                                             int.Parse(s[0, 2].ToString()));
        }
        
        #endregion
    }
}
