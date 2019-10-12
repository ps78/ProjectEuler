using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    using Triple = Tuple<int, int, int>;

    /// <summary>
    /// https://projecteuler.net/problem=52
    /// It can be seen that the number, 125874, and its double, 251748, contain exactly the same digits, but in a different order.
    /// Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits.
    /// </summary>
    public class Problem052 : EulerProblemBase
    {
        public Problem052() : base(52, "Permuted multiples", 0, 142857) { }

        public override long Solve(long n)
        {
            long m = 10;
            while (true)
            {
                if (IsSolution(m))
                    break;
                if (m.ToString()[0] != '1')
                    m = (long)Math.Pow(10, 1 + ((long)Math.Log10(m)));
                else
                    m++;
            }
            /*
            Console.WriteLine("1 x n = {0}  /  {1}", n, ToSortedString(n));
            Console.WriteLine("2 x n = {0}  /  {1}", 2*n, ToSortedString(2*n));
            Console.WriteLine("3 x n = {0}  /  {1}", 3*n, ToSortedString(3*n));
            Console.WriteLine("4 x n = {0}  /  {1}", 4*n, ToSortedString(4*n));
            Console.WriteLine("5 x n = {0}  /  {1}", 5*n, ToSortedString(5*n));
            Console.WriteLine("6 x n = {0}  /  {1}", 6*n, ToSortedString(6*n));
            */

            return m;
        }

        private bool IsSolution(long n)
        {
            var nChars = ToSortedString(n);
            if (nChars != ToSortedString(2 * n))
                return false;
            if (nChars != ToSortedString(3 * n))
                return false;
            if (nChars != ToSortedString(4 * n))
                return false;
            if (nChars != ToSortedString(5 * n))
                return false;
            if (nChars != ToSortedString(6 * n))
                return false;
            return true;
        }

        private string ToSortedString(long n)
        {
            return n.ToString().ToCharArray().OrderBy(c => c).Select(c => c.ToString()).Aggregate((res, c) => res += c);
        }
    }
}
