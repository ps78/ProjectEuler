using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=64
    /// All square roots are periodic when written as continued fractions and can be written in the form:
    /// It can be seen that the sequence is repeating.For conciseness, we use the notation √23 = [4;(1,3,1,8)], to indicate that the block(1,3,1,8) repeats indefinitely.
    /// 
    /// The first ten continued fraction representations of (irrational) square roots are:
    /// 
    /// √2=[1;(2)], period=1
    /// √3=[1;(1,2)], period=2
    /// √5=[2;(4)], period=1
    /// √6=[2;(2,4)], period=2
    /// √7=[2;(1,1,1,4)], period=4
    /// √8=[2;(1,4)], period=2
    /// √10=[3;(6)], period=1
    /// √11=[3;(3,6)], period=2
    /// √12= [3;(2,6)], period=2
    /// √13=[3;(1,1,1,1,6)], period=5
    /// 
    /// Exactly four continued fractions, for N ≤ 13, have an odd period.
    /// 
    /// How many continued fractions for N ≤ 10000 have an odd period?
    /// </summary>
    public class Problem064 : EulerProblemBase
    {
        public Problem064() : base(64, "Odd period square roots", 10000, 1322) { }

        public override bool Test() => Solve(13) == 4;

        public override long Solve(long n)
        {
            ulong odd_count = 0;

            var squares = new HashSet<int>(Enumerable.Range(2, (int)Math.Sqrt(n)).Select(x => x * x));

            for (int m = 2; m <= (int)n; m++)
                if (!squares.Contains(m))
                {
                    var seq = GetSequence(m);                    
                    if ((seq.Count - 1) % 2 != 0)
                        odd_count++;
                }
            
            return (long)odd_count;
        }

        private IList<int> GetSequence(int n)
        {
            double root = Math.Sqrt(n);
            long a = (long)Math.Floor(root);            
            long c = -a;
            long d = 1;

            var seq = new List<int>(new int[] { (int)a });
            
            while (true)
            {
                //Console.WriteLine("(a,b,c,d) = ({0},{1},{2},{3})", a, b, c, d);                
                a = (long)Math.Floor(d / (root + c));                
                long d_next = (n - c * c) / d;
                long c_next = - a * (d_next) - c;

                seq.Add((int)a);
                
                c = c_next; d = d_next;
                
                if (d == 1)
                    return seq;
            }            
        }
        /*
        private int GetPeriod(IList<int> l, int skipFirstElements, int minPeriod, out int maxPeriod)
        {
            int n = l.Count;
            int startIdx = skipFirstElements;
            maxPeriod = (n - startIdx) / 2;            

            // test periods
            for (int p = minPeriod; p <= maxPeriod; p++)
            {
                int multi = (n - startIdx) / p - 1;
                bool isPeriod = true;
                for (int k = 0; k < p; k++)
                    for (int m = 1; m <= multi; m++)
                        if (l[startIdx + k] != l[startIdx + m*p + k])
                        {
                            isPeriod = false;
                            break;
                        }

                if (isPeriod)
                    return p;                   
            }
            return 0;
        }
        */
   }
}
