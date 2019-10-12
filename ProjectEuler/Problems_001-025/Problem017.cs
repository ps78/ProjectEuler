using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=17
    ///     
    /// If the numbers 1 to 5 are written out in words: one, two, three, four, five, then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total.
    /// If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, how many letters would be used?
    /// NOTE: Do not count spaces or hyphens.For example, 342 (three hundred and forty-two) contains 23 letters and 115 (one hundred and fifteen) 
    /// contains 20 letters.The use of "and" when writing out numbers is in compliance with British usage.
    /// </summary>
    public class Problem017 : EulerProblemBase
    {
        public Problem017() : base(17, "Number letter counts", 1000, 21124) { }

        public override bool Test() => Solve(5) == 19;

        public override long Solve(long n)
        {
            int totalLen = 0;
            for (int i = 1; i <= (int)n; i++)
            {
                totalLen += AsWord(i).Length;
            }

            return (long)totalLen;
        }

        private string AsWord(int n)
        {
            if (n == 1000)
                return "onethousand";

            int ones = n % 10;
            int tens = (n % 100) / 10;
            int hundreds = (n % 1000) / 100;

            string result = "";
            
            if (tens == 1)
                result = AsWordTeens(10 * tens + ones);
            else
                result = AsWordTens(tens) + AsWordOnes(ones);

            if (hundreds > 0)
            {
                result = AsWordOnes(hundreds) + "hundred" + ((tens > 0) || (ones > 0) ? "and" : "") +result;
            }

            return result;
        }

        private string AsWordOnes(int n)
        {
            switch (n)
            {
                case 0: return "";
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                case 5: return "five";
                case 6: return "six";
                case 7: return "seven";
                case 8: return "eight";
                case 9: return "nine";
                default: return "";
            }
        }

        private string AsWordTeens(int n)
        {
            switch(n)
            {
                case 10: return "ten";
                case 11: return "eleven";
                case 12: return "twelve";
                case 13: return "thirteen";
                case 14: return "fourteen";
                case 15: return "fifteen";
                case 16: return "sixteen";
                case 17: return "seventeen";
                case 18: return "eighteen";
                case 19: return "nineteen";
                default: return "";
            }
        }

        private string AsWordTens(int n)
        {
            switch (n)
            {
                case 0: return "";
                case 1: return "";
                case 2: return "twenty";
                case 3: return "thirty";
                case 4: return "forty";
                case 5: return "fifty";
                case 6: return "sixty";
                case 7: return "seventy";
                case 8: return "eighty";
                case 9: return "ninety";
                default: return "";
            }
        }
    }
}
