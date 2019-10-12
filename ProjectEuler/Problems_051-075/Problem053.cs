using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=53
    /// There are exactly ten ways of selecting three from five, 12345:
    /// 
    /// 123, 124, 125, 134, 135, 145, 234, 235, 245, and 345
    /// 
    /// In combinatorics, we use the notation, 5C3 = 10.
    /// 
    /// In general,
    /// 
    /// nCr =
    /// n!
    /// r!(n−r)!
    /// ,where r ≤ n, n! = n×(n−1)×...×3×2×1, and 0! = 1.
    /// It is not until n = 23, that a value exceeds one-million: 23C10 = 1144066.
    /// 
    /// How many, not necessarily distinct, values of nCr, for 1 ≤ n ≤ 100, are greater than one-million?
    /// </summary>
    public class Problem053 : EulerProblemBase
    {
        public Problem053() : base(53, "Combinatoric selections", 0, 4075) { }

        public override long Solve(long n)
        {
            int result = 0;
            for (int r = 1; r <= 100; r++)
                for (int m = 1; m <= 100; m++)
                    if (C(m, r))
                        result++;

            return result;
        }

        /// <summary>
        ///  Retruns true if nCr > 1e6, where nCr = n! / (r!(n-r)!)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private bool C(int n, int r)
        {
            var numer = new List<int>();
            var denom = new List<int>();

            // add all i for i = n...m where m=max(r, n-r)
            int m = Math.Max(r, n - r);
            for (int i = n; i > m; i--)
                numer.Add(i);

            // add all i for i = 1..q where q=min(r, n-r)
            int q = Math.Min(r, n - r);
            for (int i = q; i > 1; i--)
                denom.Add(i);
            
            double result = 1;
            for (int i = 0; i < Math.Min(numer.Count(), denom.Count()); i++)
                result *= (double)numer[i] / denom[i];
            if (numer.Count() > denom.Count())
                for (int i = denom.Count(); i < numer.Count(); i++)
                    result *= (double)numer[i];
            else
                for (int i = numer.Count(); i < denom.Count(); i++)
                    result *= 1.0d/denom[i];

            return (result > 1000000);
        }
    }
}
