using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NumberTheory
{
    /// <summary>
    /// Class to compute the partition number of an integer, i.e. the number of ways a given integer can
    /// be written as sum of positive integers - not counting permuations of parts.
    /// Using the series from Hans Rademacher (see https://en.wikipedia.org/wiki/Partition_(number_theory))
    /// </summary>
    public static class PartitionMath
    {
        /// <summary>
        /// Computes the partition function of n: p(n)
        /// </summary>                
        public static ulong Compute(ulong n)
        {
            double epsilon = 0.1;
            double sum = 0.0;
            double delta = double.MaxValue;

            double n24 = n - 1.0 / 24;
            double B = Math.Sqrt(2 / 3.0) * Math.PI * Math.Sqrt(n24);

            ulong k = 1;
            while (delta > epsilon)
            {
                double previousSum = sum;
                double derivative = Math.PI * Math.Cosh(B / k) / (Math.Sqrt(6) * k * n24)
                                  - Math.Sinh(B / k) / (2.0 * Math.Pow(n24, 1.5));
                sum += Math.Sqrt(k) * A(k, n) * derivative;
                delta = Math.Abs(sum - previousSum);
                k++;
            }
            sum /= (Math.Sqrt(2.0) * Math.PI);

            return (ulong)Math.Round(sum);
        }        

        private static double A(ulong k, ulong n)
        {
            Complex sum = 0;
            for (ulong m = 0; m < k; m++)
                if (GCD.Compute(m, k) == 1)
                    sum += Complex.Exp(Math.PI * Complex.ImaginaryOne * (DedekindSum(m, k) - 2.0 * n * m / k));
            return sum.Real;
        }

        /// <summary>
        /// Dedekind sum if b and c are coprime and > 0
        /// </summary>
        private static double DedekindSum(ulong b, ulong c)
        {
            double sum = 0;
            for (ulong j = 1; j <= c - 1; j++)
                sum += Cot(Math.PI * j / c) * Cot(Math.PI * j * b / c);
            return sum / (4 * c);
        }

        private static double Cot(double x)
        {
            return 1.0 / Math.Tan(x);
        }
    }
}
