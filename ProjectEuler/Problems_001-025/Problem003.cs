using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=3
    /// 
    /// The prime factors of 13195 are 5, 7, 13 and 29.
    /// What is the largest prime factor of the number 600851475143 ?
    /// </summary>
    public class Problem003 : EulerProblemBase
    {
        public Problem003() : base(3, "Largest prime factor", 600851475143, 6857) { }

        public override bool Test() =>  Solve(13195) == 29;

        public override long Solve(long n)
        {
            var factors = new List<long>();

            if ((n % 2) == 0)
            {
                factors.Add(2);
                n /= 2;
            }

            long limit = (long)Math.Sqrt(n);

            long i = 3;
            while (true)
            {                
                if ((n % i) == 0)
                {
                    factors.Add(i);
                    do
                    {
                        n /= i;
                    }
                    while ((n % i) == 0);
                }
                
                i += 2;
                if (i >= limit)
                    break;
            }

            return factors.Max();            
        }
    }
}
