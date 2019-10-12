using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public static class LongDivison
    {
        /// <summary>
        /// returns numerator/denominator with a maximal precision of decimalCount decimals
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <param name="decimalCount">the maximum number of characters after the decimal point</param>
        /// <returns></returns>
        public static string Divide(long numerator, long denominator, int decimalCount)
        {
            string result = "";

            long num = numerator;
            long den = denominator;

            long res = num / den;
            result += res.ToString() + ".";

            int maxLen = result.Length + decimalCount;

            num = num - res * den;
            while (true)
            {
                if (num == 0)
                    break;

                num *= 10;
                while (num < den)
                {
                    num *= 10;
                    result += "0";
                    if (result.Length >= maxLen)
                        break;
                }
                long part = num / den;
                result += part.ToString();
                num = num - part * den;

                if (result.Length >= maxLen)
                    break;
            }

            return result;
        }
    }
}
