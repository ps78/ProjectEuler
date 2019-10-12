using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    public abstract class EulerProblemBase_Legacy : IEulerProblem_Legacy
    {
        private ulong problemSize;

        /// <summary>
        /// The identifer of the problem as posted on htts://projecteuler.net
        /// </summary>
        public abstract int ProblemNumber { get; }

        /// <summary>
        /// Minium allowed value for ProblemSize
        /// </summary>
        public virtual ulong MinProblemSize { get { return 0; } }

        /// <summary>
        /// Maximum allowed value for ProblemSize
        /// </summary>
        public virtual ulong MaxProblemSize { get { return ulong.MaxValue; } }

        /// <summary>
        /// The problem size as stated in the original challenge
        /// </summary>
        public virtual ulong DefaultProblemSize { get { return 0; } }

        /// <summary>
        /// The actual problem size
        /// </summary>
        public ulong ProblemSize
        {
            get { return problemSize; }
            set
            {
                if ((value < MinProblemSize) || (value > MaxProblemSize))
                    throw new ArgumentException("The problem size is outside of the allowed range of [" + MinProblemSize.ToString() + ", " + MaxProblemSize.ToString());

                problemSize = value;
                ProblemSizeChanged();
            }
        }

        /// <summary>
        /// Initializes the problem with the default problem size as stated 
        /// </summary>
        public EulerProblemBase_Legacy()
        {
            ProblemSize = DefaultProblemSize;
        }

        /// <summary>
        /// Initializes the problem with a non-standard size
        /// </summary>
        /// <param name="problemSize"></param>
        public EulerProblemBase_Legacy(ulong problemSize)
        {
            ProblemSize = problemSize;
        }

        /// <summary>
        /// Reset / calculate whatever is necessary when the problem size changes
        /// </summary>
        protected virtual void ProblemSizeChanged() { }

        /// <summary>
        /// Caluclates the solution of the problem
        /// </summary>
        /// <returns></returns>
        public abstract ulong Solve();
    }
}
