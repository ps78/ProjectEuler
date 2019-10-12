using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=79
    /// A common security method used for online banking is to ask the user for three random characters from a passcode.
    /// For example, if the passcode was 531278, they may ask for the 2nd, 3rd, and 5th characters; the expected reply would be: 317.
    /// 
    /// The text file, keylog.txt, contains fifty successful login attempts.
    /// 
    /// Given that the three characters are always asked for in order, 
    /// analyse the file so as to determine the shortest possible secret passcode of unknown length.
    /// </summary>
    public class Problem079 : EulerProblemBase
    {
        public Problem079() : base(79, "Passcode derivation", 0, 73162890) { }

        public override long Solve(long n)
        {
            var codes = new string[] {
                "319","680","180","690","129","620","762","689","762","318","368","710","720","710","629","168","160",
                "689","716","731","736","729","316","729","729","710","769","290","719","680","318","389","162","289",
                "162","718","729","319","790","680","890","362","319","760","316","729","380","319","728","716" };

            // extract rules in the form of (x) is preceeded by (a1,a2,a3,..), stored as Dictionary(x, Hashset(a1, a2, ..))
            var rules = new Dictionary<char, HashSet<char>>();
            foreach (var c in codes)
            {
                if (!rules.ContainsKey(c[0])) rules.Add(c[0], new HashSet<char>());
                if (!rules.ContainsKey(c[1])) rules.Add(c[1], new HashSet<char>());
                if (!rules.ContainsKey(c[2])) rules.Add(c[2], new HashSet<char>());
                if (!rules[c[1]].Contains(c[0])) rules[c[1]].Add(c[0]);
                if (!rules[c[2]].Contains(c[0])) rules[c[2]].Add(c[0]);
                if (!rules[c[2]].Contains(c[1])) rules[c[2]].Add(c[1]);
            }
            var chars = new HashSet<char>(rules.Keys);

            // process those elements that have the largest amout of preceeding other elements first
            string result = "";
            var sortedRules = rules.ToList();
            sortedRules.Sort((r1, r2) => r1.Value.Count.CompareTo(r2.Value.Count));
            foreach (var rule in sortedRules)
            {
                result += rule.Key;
                //Console.WriteLine("{0} is preceeded by {1}", rule.Key, rule.Value.Select((ch) => int.Parse(ch.ToString())).ToString<int>());
            }

            return long.Parse(result);
        }
    }
}
