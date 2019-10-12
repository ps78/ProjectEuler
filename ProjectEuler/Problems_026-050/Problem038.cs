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
    /// https://projecteuler.net/problem=38
    /// </summary>
    public class Problem038 : EulerProblemBase
    {
        public Problem038() : base(38, "Pandigital multiples", 0, 932718654) { }

        public override long Solve(long n)
        {
            var solutions = new List<ulong>();
            var bases = new List<int>();

            bases.Add(9);
            for (int i = 91; i <= 98; i++) bases.Add(i);
            for (int i = 912; i <= 987; i++) bases.Add(i);
            for (int i = 9123; i <= 9876; i++) bases.Add(i);

            foreach (int i in bases)
            {
                string n1 = i.ToString();
                string n2 = (2 * i).ToString();
                string num = n1 + n2;
                if (num.Length < 9)
                {
                    num += (3 * i).ToString();
                    if (num.Length < 9)
                    {
                        num += (4 * i).ToString();
                        if (num.Length < 9)
                            num += (5 * i).ToString();
                    }
                }
                if ((num.Length == 9) && Is1To9Pandigital(num))
                    solutions.Add(ulong.Parse(num));
            }

            return (long)solutions.Max();
        }

        private bool Is1To9Pandigital(string s)
        {
            return (s.IndexOf('1') != -1) && (s.IndexOf('2') != -1) && (s.IndexOf('3') != -1) &&
                   (s.IndexOf('4') != -1) && (s.IndexOf('5') != -1) && (s.IndexOf('6') != -1) &&
                   (s.IndexOf('7') != -1) && (s.IndexOf('8') != -1) && (s.IndexOf('9') != -1);
        }
    }
}
