
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=25
    /// F1 = 1
    /// What is the index of the first term in the Fibonacci sequence to contain 1000 digits?
    /// 
    /// </summary>
    public class Problem025 : EulerProblemBase
    {
        public Problem025() : base(25, "1000-digit Fibonacci number", 1000, 4782) { }

        public override bool Test() => Solve(3) == 12;

        public override long Solve(long n)
        {
            /* brute force solution:
            var fPrev = BigInteger.One;
            var fCur = BigInteger.One;
            ulong index = 2;
            int n = (int)ProblemSize;
            while (fCur.ToString().Length < n)
            {
                var fNext = fCur + fPrev;
                fPrev = fCur;
                fCur = fNext;
                index++;
            }
            return index;
            */

            // elegant solution, based on the formula for the n-th Fibonacci number:
            // Fn = 1/sqrt(5) * { [(1+sqrt5)/2]^n - [(1-sqrt5)/2]^n }
            // using https://en.wikipedia.org/wiki/Fibonacci_number
            // this can be resolved to Fn = round(phi^n/sqrt5) where phi = (1+sqrt5)/2
            // and ProlbemSize - 1 =: C =: Log10(Fn) == Log10( phi^n/sqrt5 )
            // resolves to n = round(C+log10(sqrt5) / log10(phi))

            double phi = (1.0 + Math.Sqrt(5)) / 2;
            return (long)Math.Ceiling((n - 1 + Math.Log10(Math.Sqrt(5))) / Math.Log10(phi));
        }
    }
}
