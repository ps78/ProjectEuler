using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    public abstract class EulerProblemBase: IEulerProblem
    {
        public int ProblemNumber { get; }

        public long ProblemSize { get; }

        public string Title { get; }

        public long Solution { get; }        

        public bool IsSolved => Solution != 0;

        protected string ResourcePath => @"D:\Code\ProjectEuler\ProjectEuler\Resources";

        protected EulerProblemBase(int problemNumber, string title, long problemSize = 0, long solution = 0)
        {
            ProblemNumber = problemNumber;
            ProblemSize = problemSize;
            Title = title;
            Solution = solution;
        }

        public abstract long Solve(long n);

        /// <summary>
        /// Always return true unless an actual test is implemented
        /// </summary>
        public virtual bool Test() => true;
    }
}
