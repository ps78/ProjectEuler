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
    /// https://projecteuler.net/problem=66
    /// Consider quadratic Diophantine equations of the form:
    /// 
    /// x2 – Dy2 = 1
    /// 
    /// For example, when D=13, the minimal solution in x is 6492 – 13×1802 = 1.
    /// 
    /// It can be assumed that there are no solutions in positive integers when D is square.
    /// 
    /// By finding minimal solutions in x for D = {2, 3, 5, 6, 7}, we obtain the following:
    /// 
    /// 3^2 – 2 × 2^2 = 1
    /// 2^2 – 3 × 1^2 = 1
    /// 9^2 – 5 × 4^2 = 1
    /// 5^2 – 6 × 2^2 = 1
    /// 8^2 – 7 × 3^2 = 1
    /// 
    /// Hence, by considering minimal solutions in x for D ≤ 7, the largest x is obtained when D=5.
    /// 
    /// Find the value of D ≤ 1000 in minimal solutions of x for which the largest value of x is obtained.
    /// </summary>
    public class Problem066 : EulerProblemBase
    {
        public Problem066() : base(66, "Diophantine equation", 1000, 661) { }

        public override bool Test() => Solve(7) == 5;
        
        public override long Solve(long n)
        {

            var maxState = new { maxX = new BigInteger(), maxD = (int)0 };
            
            for (int d = 2; d <= (int)n; d++)
            {
                int root = (int)Math.Sqrt(d);
                if (root * root != d)
                {
                    var a = ContinuedFractionOfSquareRoot(d).ToList();
                    int period = a.Count - 1;
                    
                    if (period % 2 == 0)
                        a.RemoveAt(a.Count - 1);
                    else
                        a.AddRange(a.Skip(1).Take(a.Count - 1).ToArray());

                    BigInteger num, den;
                    Simplify(a.ToArray(), out num, out den);
                    
                    if (num > maxState.maxX)
                        maxState = new { maxX = num, maxD = d };
                }
            }
            return maxState.maxD;
        }
        
        /// <summary>
        /// Returns the continued fraction of sqrt(n) up to the first period, i.e. [a0,(a1,a2,a3,..,an)] where (a1,..an) is periodic and
        /// sqrt(n) = a0 + 1/( a1 + 1/( a2 + 1/... )))
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private List<int> ContinuedFractionOfSquareRoot(int n)
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
                long c_next = -a * (d_next) - c;

                seq.Add((int)a);

                c = c_next; d = d_next;

                if (d == 1)
                    return seq;
            }
        }
        
        /// <summary>
        /// simplifies the continues fraction given by series  (a0,a1,..an): a0+1/(a1+1/(a2+...)) to numerator/denominator
        /// </summary>
        /// <param name="series"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        private void Simplify(int[] series, out BigInteger numerator, out BigInteger denominator)
        {
            var z = BigInteger.Zero;
            var n = BigInteger.One;

            for (int idx = series.Length - 1; idx >= 0; idx--)
            {
                if (idx == 0)
                    z = (ulong)series[0] * n + z;
                else
                {
                    BigInteger tmp = n;
                    n = new BigInteger(series[idx]) * n + z;
                    z = tmp;
                }         
            }

            numerator = z;
            denominator = n;
        }
                
    }
}
