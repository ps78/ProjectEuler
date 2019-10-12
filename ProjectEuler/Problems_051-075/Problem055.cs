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
    using Triple = Tuple<int, int, int>;

    /// <summary>
    /// https://projecteuler.net/problem=55
    /// If we take 47, reverse and add, 47 + 74 = 121, which is palindromic.
    /// Not all numbers produce palindromes so quickly.For example,
    /// 
    /// 349 + 943 = 1292,
    /// 1292 + 2921 = 4213
    /// 4213 + 3124 = 7337
    /// 
    /// That is, 349 took three iterations to arrive at a palindrome.
    /// Although no one has proved it yet, it is thought that some numbers, like 196, never produce a palindrome. 
    /// A number that never forms a palindrome through the reverse and add process is called a Lychrel number. 
    /// Due to the theoretical nature of these numbers, and for the purpose of this problem, we shall assume that a number is Lychrel until proven otherwise. 
    /// In addition you are given that for every number below ten-thousand, it will either 
    ///     (i) become a palindrome in less than fifty iterations, or, 
    ///     (ii)no one, with all the computing power that exists, has managed so far to map it to a palindrome.
    /// In fact, 10677 is the first number to be shown to require over fifty iterations before producing a palindrome: 4668731596684224866951378664(53 iterations, 28 - digits).
    /// Surprisingly, there are palindromic numbers that are themselves Lychrel numbers; the first example is 4994.
    /// How many Lychrel numbers are there below ten-thousand?
    /// </summary>
    public class Problem055 : EulerProblemBase
    {
        public Problem055() : base(55, "Lychrel numbers", 0, 249) { }

        public override long Solve(long n)
        {
            int result = 0;
            for (int i = 1; i < 10000; i++)
                if (IsLynchrel(i))
                {
                    //Console.WriteLine(i);
                    result++;
                }                    

            return result;
        }

        private bool IsLynchrel(BigInteger n, int recursion = 0)
        {
            if (recursion > 50)
                return true;

            BigInteger nrev = BigInteger.Parse(new string(n.ToString().ToCharArray().Reverse().ToArray()));
            BigInteger next = (n + nrev);
            if (IsPalindrome(next.ToString()))
                return false;
            else
                return IsLynchrel(next, ++recursion);            
        }

        private bool IsPalindrome(string s)
        {
            int n = s.Length;
            for (int i = 0; i < n / 2; i++)
                if (s[i] != s[n - i - 1])
                    return false;
            return true;
        }
    }
}
