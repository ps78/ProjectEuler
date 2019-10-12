using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;
using System.Numerics;
using System.Collections.Concurrent;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=100
    /// If a box contains twenty-one coloured discs, composed of fifteen blue discs and six red discs, 
    /// and two discs were taken at random, it can be seen that the probability of taking two 
    /// blue discs, P(BB) = (15/21)×(14/20) = 1/2.
    /// 
    /// The next such arrangement, for which there is exactly 50% chance of taking two blue discs at 
    /// random, is a box containing eighty-five blue discs and thirty-five red discs.
    /// 
    /// By finding the first arrangement to contain over 1012 = 1,000,000,000,000 discs in total, 
    /// determine the number of blue discs that the box would contain.
    /// </summary>
    public class Problem100 : EulerProblemBase
    {
        public Problem100() : base(100, "Arranged probability", 1_000_000_000_000, 756872327473) { }
        
        public override long Solve(long n)
        {
            // the eqation n/b * (n-1)/(b-1) = 1/2 where n=number of all discs and b = number of blue discs can be reduced 
            // to Pell's equation u^2 -2v^2 = 1
            // for n = 1/2 (u + v + 1) and b = 1/2 (u + 2v + 1)
            
            // the solutions to Pell's equation can be derived from the convergents of the continued fraction u/v of sqrt(2)
            // get the first one, that solves the equation
            long u1 = 0, v1 = 0;
            foreach (var convergent in Sqrt2Convergents())
            {
                if (convergent.Item1 * convergent.Item1 - 2 * convergent.Item2 * convergent.Item2 == 1)
                {
                    u1 = convergent.Item1;
                    v1 = convergent.Item2;
                    break;
                }                    
            }

            long k = 0, b = 0;
            long u = u1, v = v1;            
            do
            {
                b = (u + v + 1) / 2;
                k = (u + 2 * v + 1) / 2;

                //Console.WriteLine("N = {0,20:N0}  b = {1,20:N0}   u = {2,20:N0} v = {3,20:N0}", n, b, u, v);

                long u_next = u1 * u + 2 * v1 * v;
                long v_next = u1 * v + v1 * u;

                u = u_next;
                v = v_next;

            } while (k <= n);

            // note: this does NOT find all solutions, but obviously the right one. -

            return b;
            

            // brute-force approach, finds solution in ~100s and also generally finds all solutions
            /*
            double sqrt2 = Math.Sqrt(2.0);

            long startRange = 1;
            long endRange = 1_100_000_000;
            
            var results = new ConcurrentBag<Tuple<long, long>>();

            var rangePartitioner = Partitioner.Create(startRange, endRange);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (long n = range.Item1; n < range.Item2; n++)
                {                    
                    var b = (long)(n / sqrt2) + 1;
                    if (2 * b * (b - 1) == n * (n -1))
                        results.Add(new Tuple<long, long>(n, b));
                }
            });

            foreach (var r in results.ToList().OrderBy(x => x.Item1))
                Console.WriteLine("N = {0,20:N0}   b = {1,20:N0}", r.Item1, r.Item2);

            return (ulong)results.Select(x => x.Item2).Min();
            */
        }

        /// <summary>
        /// continued fraction sqrt(2), see problem 057 
        /// returns num/den as continously improved approximations of sqrt(2)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Tuple<long, long>> Sqrt2Convergents()
        {
            long a = 1;
            long b = 2;
            for (int i = 2; i <= 20; i++)
            {
                long t = a;
                a = b;
                b = 2 * b + t;
                yield return new Tuple<long, long>(a + b, b);
            }
        }
    }
}
