using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;

namespace ProjectEuler
{    
    /// <summary>
    /// Solves https://projecteuler.net/problem=099
    /// Comparing two numbers written in index form like 211 and 37 is not difficult, as any calculator would confirm that 2^11 = 2048 smaller then 3^7 = 2187.
    /// However, confirming that 632382^518061 > 519432^525806 would be much more difficult, as both numbers contain over three million digits.
    /// Using base_exp.txt(right click and 'Save Link/Target As...'), a 22K text file containing one thousand lines with a base/exponent 
    /// pair on each line, determine which line number has the greatest numerical value.
    /// 
    /// NOTE: The first two lines in the file represent the numbers in the example given above.
    /// </summary>
    public class Problem099 : EulerProblemBase
    {
        public Problem099() : base(99, "Largest exponential", 0, 709) { }

        public override long Solve(long n)
        {
            var lst = new List<Tuple<int, double>>();

            int lineNumber = 1;
            foreach (var line in File.ReadLines(Path.Combine(ResourcePath, "problem099.txt")))
            {
                var parts = line.Split(new char[] { ',' });
                double log = int.Parse(parts[1]) * Math.Log(int.Parse(parts[0]));
                lst.Add(new Tuple<int, double>(lineNumber++, log));                
            }

            var result = lst.OrderByDescending(t => t.Item2).First();

            return result.Item1;
        }
    }
}
