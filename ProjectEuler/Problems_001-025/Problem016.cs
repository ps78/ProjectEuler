using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=16
    ///     
    /// 215 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26
    /// What is the sum of the digits of the number 21000?
    /// </summary>
    public class Problem016 : EulerProblemBase
    {
        public Problem016() : base(16, "Power digit sum", 1000, 1366) { }

        public override bool Test() => Solve(15) == 26;

        public override long Solve(long n)
        {
            var m = new BigInteger(2);
            for (long i = 1; i < n; i++)
                m = m * 2;
            
            return m.ToString().ToCharArray().Select((c) => long.Parse(c.ToString())).Sum();
        }        
    }
}
