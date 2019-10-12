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
    /// https://projecteuler.net/problem=43
    /// 
    /// The number, 1406357289, is a 0 to 9 pandigital number because it is made up of each of the digits 0 to 9 in some order, but it also has a rather interesting sub-string divisibility property.
    /// 
    /// Let d1 be the 1st digit, d2 be the 2nd digit, and so on.In this way, we note the following:
    /// 
    /// d2d3d4= 406 is divisible by 2
    /// d3d4d5= 063 is divisible by 3
    /// d4d5d6= 635 is divisible by 5
    /// d5d6d7= 357 is divisible by 7
    /// d6d7d8= 572 is divisible by 11
    /// d7d8d9= 728 is divisible by 13
    /// d8d9d10= 289 is divisible by 17
    /// Find the sum of all 0 to 9 pandigital numbers with this property.
    /// </summary>
    public class Problem043 : EulerProblemBase
    {
        public Problem043() : base(43, "Sub-string divisibility", 0, 16695334890) { }

        public override long Solve(long n)
        {            
            var t = new Dictionary<int, Dictionary<string, HashSet<string>>>();
            foreach (var m in new int[] { 2, 3, 5, 7, 11, 13, 17 })
                t.Add(m, new Dictionary<string, HashSet<string>>());
            for (int i = 0; i <= 9; i++)
            {
                string leadZero = i == 0 ? "0" : "";
                for (int j = 0; j <= 9; j++)
                    for (int k = 0; k <= 9; k++)
                        if ((i != j) && (i != k) && (j != k))
                            foreach (var p in new int[] { 2, 3, 5, 7, 11, 13, 17 })
                            {
                                int num = 100 * i + 10 * j + k;
                                if ((num % p) == 0)
                                {
                                    string lead = leadZero + (10 * i + j).ToString();
                                    if (!t[p].ContainsKey(lead))
                                        t[p].Add(lead, new HashSet<string>());
                                    t[p][lead].Add(leadZero + num.ToString());
                                }

                            }
            }

            List<string> numbers = new List<string>();
            foreach (var d234dic in t[2])
                foreach (var d234 in d234dic.Value)
                    foreach (var d345 in t[3][d234.Substring(1,2)])
                        if (t[5].ContainsKey(d345.Substring(1,2)))
                            foreach (var d456 in t[5][d345.Substring(1,2)])
                                if (t[7].ContainsKey(d456.Substring(1, 2)))
                                    foreach (var d567 in t[7][d456.Substring(1,2)])
                                        if (t[11].ContainsKey(d567.Substring(1, 2)))
                                            foreach (var d678 in t[11][d567.Substring(1, 2)])
                                                if (t[13].ContainsKey(d678.Substring(1, 2)))
                                                    foreach (var d789 in t[13][d678.Substring(1, 2)])
                                                        if (t[17].ContainsKey(d789.Substring(1, 2)))
                                                            foreach (var d890 in t[17][d789.Substring(1, 2)])
                                                            {
                                                                string number = d234 + d567 + d890;
                                                                bool isPanDigital = true;
                                                                string missing = "0123456789";
                                                                foreach (var ch in "0123456789".ToCharArray())
                                                                {
                                                                    int chCount = number.Count(c => c == ch);
                                                                    if (chCount > 1)
                                                                    {
                                                                        isPanDigital = false;
                                                                        break;
                                                                    }
                                                                    else if (chCount == 1)
                                                                        missing = missing.Replace(ch.ToString(), "");
                                                                }

                                                                if (isPanDigital)
                                                                    numbers.Add(missing + number);
                                                            }

            return numbers.Select(s => long.Parse(s)).Sum();
        }        
        
    }
    
}
