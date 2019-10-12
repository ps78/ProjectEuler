using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=78
    /// Let p(n) represent the number of different ways in which n coins can be separated into piles.
    /// For example, five coins can be separated into piles in exactly seven different ways, so p(5)=7.
    /// 
    /// OOOOO
    /// OOOO   O
    /// OOO   OO
    /// OOO   O O
    /// OO OO   O
    /// OO   O O   O
    /// O   O O   O O
    /// Find the least value of n for which p(n) is divisible by one million.
    /// </summary>
    public class Problem078 : EulerProblemBase
    {
        public Problem078() : base(78, "Coin partitions", 1_000_000, 55374) { }

        /// <summary>
        /// p(n) is the partition number of n
        /// 
        /// p(n) = p(n-1) + p(n-2) - p(n-5) - p(n-7) + p(n-12) + p(n-15) - p(n-22)-...
        /// with p(0) = 1, p(i) = 0 for  i negative
        /// and the numbers in the formula have the form m(3m − 1)/2 for m = 1, 2, ...
        /// the signs are (-1)^(|m|-1)
        /// 
        /// </summary>
        /// <returns></returns>
        public override long Solve(long n)
        {
            var p = new List<int>();
            p.Add(1);   // p(0) = 1

            int i = 1;
            while(true)
            {
                int pn = 0;
                int m = 1;
                while(true)
                {
                    int k = m * (3 * m - 1) / 2;
                    if (i - k < 0)
                        break;

                    int sign = m % 2 == 0 ? -1 : +1;
                    pn += sign * p[i - k];
                    pn %= 1000000;
                    if (m > 0) m *= -1; else m = -m + 1;                    
                }
                p.Add(pn);

                if (pn == 0)
                    break;

                i++;
            }

            return i;
        }        
    }
}
