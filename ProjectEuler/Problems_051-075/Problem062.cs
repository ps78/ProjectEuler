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
    /// https://projecteuler.net/problem=62
    /// The cube, 41063625 (3453), can be permuted to produce two other cubes: 56623104 (3843) and 66430125 (4053). 
    /// In fact, 41063625 is the smallest cube which has exactly three permutations of its digits which are also cube.
    /// 
    /// Find the smallest cube for which exactly five permutations of its digits are cube.
    /// </summary>
    public class Problem062 : EulerProblemBase
    {
        public Problem062() : base(62, "Cubic permutations", 5, 127035954683) { }

        public override bool Test() => Solve(3) == 41063625;

        public override long Solve(long n)
        {
            ulong m = 1;

            int PermutationCount = (int)n;

            var cubeDic = new Dictionary<string, HashSet<ulong>>();
            HashSet<ulong> solution = null;
            while (true)
            {
                ulong cube = m * m * m;
                string s = cube.ToOrderedString();
                var set = cubeDic.ContainsKey(s) ? cubeDic[s] : null;
                if (set == null)
                {
                    cubeDic.Add(s, new HashSet<ulong>());
                    set = cubeDic[s];
                }
                set.Add(cube);
                if (set.Count == PermutationCount)
                {
                    solution = set;
                    break;
                }
                m++;

                if (m >= 2642245) // with this, the cube would be > 2^64
                    break;
            }

            //Console.WriteLine(SetToString(solution));

            if (solution == null)
            {
                Console.WriteLine("No solution found");
                return 0;
            }
            else 
                return (long)solution.Min();
        }

        private string ToOrderedString(ulong n)
        {
            string result = "";
            n.ToString().ToCharArray().OrderBy((c) => c).ToList().ForEach((c) => result += c);
            return result;
        }
        
        private string SetToString<T>(HashSet<T> set)
        {
            var sb = new StringBuilder();
            foreach (var el in set)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(el.ToString());
            }
            return "{" + sb.ToString() + "}";
        }
    }
}
