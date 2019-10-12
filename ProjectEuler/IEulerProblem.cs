using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// Interface for Euler Problems
    /// </summary>
    public interface IEulerProblem
    {
        /// <summary>
        /// Number of the problem from projectEuler.net
        /// </summary>
        int ProblemNumber { get; }

        /// <summary>
        /// The relevant "n" asked to solve the problem for
        /// </summary>
        long ProblemSize { get; }

        /// <summary>
        /// The solution for the problem as asked, i.e. for an input equal to ProblemSize
        /// Set to 0 if not overwritten, indicating that the problem is not solved yet
        /// </summary>
        long Solution { get; }

        /// <summary>
        /// True the solution is known, in which case it can be retrieved with
        /// the Solution property
        /// </summary>
        bool IsSolved { get; }

        /// <summary>
        /// Short title of the problem from the web-page
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Runs some test on a sub problem or smaller size of the problem 
        /// with known solution. Returns true if successful.
        /// </summary>
        /// <returns></returns>
        bool Test();
        
        /// <summary>
        /// Run the solver for the given problem size
        /// </summary>        
        long Solve(long n);
    }
}
