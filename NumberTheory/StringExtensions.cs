using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public static class StringExtensions
    {
        /// <summary>
        /// True if s is identical when reversed
        /// </summary>
        public static bool IsPalindrom(this string s)
        {
            int n = s.Length;
            for (int i = 0; i < n / 2; i++)
                if (s[i] != s[n - i - 1])
                    return false;
            return true;
        }

        /// <summary>
        /// Checks if the strings consist of the identical characters
        /// </summary>
        /// <param name="s"></param>
        /// <param name="otherString"></param>
        /// <returns></returns>
        public static bool IsPermuationOf(this string s, string otherString)
        {
            if (s.Length != otherString.Length)
                return false;

            var thisChars = s.ToCharArray().OrderBy((c) => c);
            var otherChars = otherString.ToCharArray().OrderBy((c) => c);
            return thisChars.SequenceEqual(otherChars);
        }

        public static string? Reverse(this string? s)
        {
            if (s == null)
                return null;

            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Returns all indices of otherString withing s
        /// </summary>
        /// <param name="s"></param>
        /// <param name="otherString"></param>
        /// <returns></returns>
        public static int[] IndicesOf(this string? s, string? otherString)
        {
            if (s == null || otherString == null)
                return [];
             
            var result = new List<int>();
            int index = -1;
            do
            {
                index = s.IndexOf(otherString, index + 1);
                if (index >= 0)
                    result.Add(index);
            }
            while (index >= 0);

            return result.ToArray();
        }
    }
}
