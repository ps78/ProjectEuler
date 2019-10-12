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
    /// https://projecteuler.net/problem=42
    /// The nth term of the sequence of triangle numbers is given by, tn = ½n(n+1); so the first ten triangle numbers are:
    /// 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, ...
    /// By converting each letter in a word to a number corresponding to its alphabetical position and adding these 
    /// values we form a word value.For example, the word value for SKY is 19 + 11 + 25 = 55 = t10.If the word value 
    /// is a triangle number then we shall call the word a triangle word.
    /// Using words.txt (right click and 'Save Link/Target As...'), a 16K text file containing nearly two-thousand 
    /// common English words, how many are triangle words?
    /// </summary>
    public class Problem042 : EulerProblemBase
    {
        public Problem042() : base(42, "Coded triangle numbers", 0, 162) { }

        public override long Solve(long n)
        {
            string buffer = File.ReadAllText(Path.Combine(ResourcePath, "problem042.txt"));
            var words = buffer.Split(new char[] { ',' }).ToList().Select(s => s.Replace("\"", ""));
            return words.Count(w => IsTriangleWord(w));
        } 

        private bool IsTriangleWord(string w)
        {
            int n = w.Select(c => (int)c - 64).Sum();
            double x = 0.5 * (Math.Sqrt(1 + 8 * n) - 1);
            return Math.Abs(x - (int)x) < 1e-8;
        }
    }
    
}
