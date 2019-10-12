using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=22
    /// Using names.txt (right click and 'Save Link/Target As...'), a 46K text file containing over five-thousand first names, 
    /// begin by sorting it into alphabetical order. Then working out the alphabetical value for each name, multiply 
    /// this value by its alphabetical position in the list to obtain a name score.
    /// For example, when the list is sorted into alphabetical order, COLIN, which is worth 3 + 15 + 12 + 9 + 14 = 53,
    /// is the 938th name in the list.So, COLIN would obtain a score of 938 × 53 = 49714.
    /// What is the total of all the name scores in the file?
    /// </summary>
    public class Problem022 : EulerProblemBase
    {
        public Problem022() : base(22, "Names scores", 0, 871198282) { }

        public override long Solve(long n)
        {
            var words = ReadFile(Path.Combine(ResourcePath, "problem022.txt")).ToList();
            words.Sort();
            int sum = 0;
            int alphaPos = 1;
            foreach (string word in words)
            {
                int alphaValue = word.ToCharArray().Sum((c) => c - 64);
                sum += alphaValue * alphaPos;                
                alphaPos++;
            }

            return sum;
        }

        private IEnumerable<string> ReadFile(string filename)
        {
            var s = File.ReadAllText(filename);
            foreach (string word in s.Split(new char[] { ',' }))
            {
                yield return word.Replace("\"", "");
            }            
        }
    }
}
