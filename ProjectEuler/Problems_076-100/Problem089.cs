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
    /// https://projecteuler.net/problem=89
    /// For a number written in Roman numerals to be considered valid there are basic rules which must be followed.
    /// Even though the rules allow some numbers to be expressed in more than one way there is always 
    /// a "best" way of writing a particular number.
    /// 
    /// For example, it would appear that there are at least six ways of writing the number sixteen:
    /// 
    /// IIIIIIIIIIIIIIII
    /// VIIIIIIIIIII
    /// VVIIIIII
    /// XIIIIII
    /// VVVI
    /// XVI
    /// 
    /// However, according to the rules only XIIIIII and XVI are valid, and the last example is considered 
    /// to be the most efficient, as it uses the least number of numerals.
    /// 
    /// The 11K text file, roman.txt (right click and 'Save Link/Target As...'), contains one thousand 
    /// numbers written in valid, but not necessarily minimal, Roman numerals; see About...Roman Numerals 
    /// for the definitive rules for this problem.
    /// 
    /// Find the number of characters saved by writing each of these in their minimal form.
    /// 
    /// Note: You can assume that all the Roman numerals in the file contain no more than four consecutive identical units.
    /// </summary>
    public class Problem089 : EulerProblemBase
    {
        public Problem089() : base(89, "Roman numerals", 0, 743) { }

        public override long Solve(long n)
        {
            string[] numerals = File.ReadAllLines(Path.Combine(ResourcePath, "problem089.txt"));

            int savings = 0;

            foreach (var N in numerals)
            {
                var r = new RomanNumeral(N);
                savings += (N.Length - r.Numeral.Length);
            }

            return savings;
        }
    }

   
}
