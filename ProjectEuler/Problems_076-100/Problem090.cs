using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=90
    /// Each of the six faces on a cube has a different digit (0 to 9) written on it; the same is done to a second cube. 
    /// By placing the two cubes side-by-side in different positions we can form a variety of 2-digit numbers.
    /// For example, the square number 64 could be formed:
    /// 
    /// In fact, by carefully choosing the digits on both cubes it is possible to display all of the square numbers 
    /// below one-hundred: 01, 04, 09, 16, 25, 36, 49, 64, and 81.
    /// 
    /// For example, one way this can be achieved is by placing {0, 5, 6, 7, 8, 9}
    ///     on one cube and {1, 2, 3, 4, 8, 9}
    ///     on the other cube.
    /// 
    /// However, for this problem we shall allow the 6 or 9 to be turned upside-down so that an arrangement like { 0, 5, 6, 7, 8, 9}
    ///     and {1, 2, 3, 4, 6, 7}
    /// allows for all nine square numbers to be displayed; otherwise it would be impossible to obtain 09.
    /// 
    /// In determining a distinct arrangement we are interested in the digits on each cube, not the order.
    /// 
    /// {1, 2, 3, 4, 5, 6} is equivalent to {3, 6, 4, 1, 2, 5}
    /// {1, 2, 3, 4, 5, 6} is distinct from {1, 2, 3, 4, 5, 9}
    /// 
    /// But because we are allowing 6 and 9 to be reversed, the two distinct sets in the last example both 
    /// represent the extended set {1, 2, 3, 4, 5, 6, 9} for the purpose of forming 2-digit numbers.
    /// 
    /// How many distinct arrangements of the two cubes allow for all of the square numbers to be displayed?
    /// </summary>
    public class Problem090 : EulerProblemBase
    {
        #region Public Methods

        public Problem090(): base(90, "Cube digit pairs", 0, 1217) { }

        public override long Solve(long n)
        {
            var set1 = GetCombinations().ToList();
            var set2 = new List<string>(set1);

            int count = 0;
            foreach (var s1 in set1)
                foreach (var s2 in set2)
                    if (s1.CompareTo(s2) > 0 && CheckSets(s1, s2))
                        count++;
            
            return count;
        }

        #endregion
        #region Private Methods

        private IEnumerable<string> GetCombinations(string given = "", string remaining = "0123456789", int length = 6)
        {
            int missing = length - given.Length;

            if (missing == 0)
                yield return given;
            else
                for (int pos = 0; pos <= remaining.Length - missing; pos++)
                    foreach (var c in GetCombinations(given + remaining.Substring(pos, 1), remaining.Substring(pos + 1), length))
                        yield return c;
        }

        private bool CheckSets(string set1, string set2)
        {
            if (!((set1.Contains("0") && set2.Contains("1")) || (set1.Contains("1") && set2.Contains("0")))) return false;
            if (!((set1.Contains("0") && set2.Contains("4")) || (set1.Contains("4") && set2.Contains("0")))) return false;
            if (!((set1.Contains("0") && set2.Contains("9")) || (set1.Contains("9") && set2.Contains("0")) || (set1.Contains("0") && set2.Contains("6")) || (set1.Contains("6") && set2.Contains("0")))) return false;
            if (!((set1.Contains("1") && set2.Contains("6")) || (set1.Contains("6") && set2.Contains("1")) || (set1.Contains("1") && set2.Contains("9")) || (set1.Contains("9") && set2.Contains("1")))) return false;
            if (!((set1.Contains("2") && set2.Contains("5")) || (set1.Contains("5") && set2.Contains("2")))) return false;
            if (!((set1.Contains("3") && set2.Contains("6")) || (set1.Contains("6") && set2.Contains("3")) || (set1.Contains("3") && set2.Contains("9")) || (set1.Contains("9") && set2.Contains("3")))) return false;
            if (!((set1.Contains("4") && set2.Contains("9")) || (set1.Contains("9") && set2.Contains("4")) || (set1.Contains("4") && set2.Contains("6")) || (set1.Contains("6") && set2.Contains("4")))) return false;
            if (!((set1.Contains("6") && set2.Contains("4")) || (set1.Contains("4") && set2.Contains("6")) || (set1.Contains("9") && set2.Contains("4")) || (set1.Contains("4") && set2.Contains("9")))) return false;
            if (!((set1.Contains("8") && set2.Contains("1")) || (set1.Contains("1") && set2.Contains("8")))) return false;
            return true;
        }

        #endregion
    }
    
}
