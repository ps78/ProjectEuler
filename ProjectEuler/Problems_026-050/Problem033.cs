using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=33
    /// 
    /// The fraction 49/98 is a curious fraction, as an inexperienced mathematician in attempting to simplify it may 
    /// incorrectly believe that 49/98 = 4/8, which is correct, is obtained by cancelling the 9s.
    /// We shall consider fractions like, 30/50 = 3/5, to be trivial examples.
    /// There are exactly four non-trivial examples of this type of fraction, less than one in value, 
    /// and containing two digits in the numerator and denominator.
    /// If the product of these four fractions is given in its lowest common terms, find the value of the denominator.
    /// </summary>
    public class Problem033 : EulerProblemBase
    {
        public Problem033() : base(33, "Digit cancelling fractions", 0, 100) { }

        public override long Solve(long n)
        {            
            var fracs = new List<Tuple<int, int>>();
            for (int numerator = 10; numerator < 100; numerator++)
            {
                string numStr = numerator.ToString();
                for (int denominator = numerator + 1; denominator < 100; denominator++)
                    if (((numerator % 10) != 0) || ((denominator % 10) != 0))
                    {
                        string denStr = denominator.ToString();
                        double correctFrac = ((double)numerator) / denominator;
                        if (denStr[0] != '0')
                        {
                            double frac = 0;
                            if (numStr[1] == denStr[1])
                                frac = double.Parse(numStr[0].ToString()) / double.Parse(denStr[0].ToString());
                            else if (numStr[0] == denStr[1])
                                frac = double.Parse(numStr[1].ToString()) / double.Parse(denStr[0].ToString());
                            if ((frac != 0) && (Math.Abs(frac - correctFrac) < 1E-8))
                                fracs.Add(new Tuple<int, int>(numerator, denominator));
                        }
                        if (denStr[1] != '0')
                        {
                            double frac = 0;
                            if (numStr[1] == denStr[0])
                                frac = double.Parse(numStr[0].ToString()) / double.Parse(denStr[1].ToString());
                            else if (numStr[0] == denStr[0])
                                frac = double.Parse(numStr[1].ToString()) / double.Parse(denStr[1].ToString());
                            if ((frac != 0) && (Math.Abs(frac - correctFrac) < 1E-8))
                                fracs.Add(new Tuple<int, int>(numerator, denominator));
                        }                        
                    }
            }
            var prod = new Tuple<long, long>(1, 1);
            foreach (var f in fracs)
                prod = new Tuple<long, long>(prod.Item1 * (long)f.Item1, prod.Item2 * (long)f.Item2);

            return SimplifyFraction(prod, 100).Item2;
        }

        private Tuple<long, long> SimplifyFraction(Tuple<long, long> frac, long maxPrimeFactor)
        {
            var sieve = new SieveOfEratosthenes((ulong)maxPrimeFactor);
            ulong n = (ulong)frac.Item1;
            ulong d = (ulong)frac.Item2;
            foreach (var p in sieve.GetPrimes())
                while (((n % p) == 0) && ((d % p) == 0))
                {
                    n /= p;
                    d /= p;
                }

            return new Tuple<long, long>((long)n, (long)d);
        }
    }
}
