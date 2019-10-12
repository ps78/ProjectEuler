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
    /// https://projecteuler.net/problem=63
    /// The 5-digit number, 16807=75, is also a fifth power. Similarly, the 9-digit number, 134217728=89, is a ninth power.
    /// How many n-digit positive integers exist which are also an nth power?
    /// </summary>
    public class Problem063 : EulerProblemBase
    {
        public Problem063() : base(63, "Powerful digit counts", 0, 49) { }

        public override long Solve(long n)
        {
            int result = 0;

            for (ulong bas = 1; bas <= 9; bas++) // for base >= 10, the length is always too long (but for b^1, where it is too short)
            {                
                ulong exp = 1;
                var m = new BigInteger(bas);
                while (true)
                {
                    ulong len = (ulong)m.ToString().Length;
                    if (len == exp)
                    {
                        result++;
                        //Console.WriteLine("{0}^{1} = {2}", bas, exp, n);
                    }
                    else if (len < exp)
                        break;

                    m = m * bas;
                    exp++;
                }                                
            }

            return result;
        }        
    }
}
