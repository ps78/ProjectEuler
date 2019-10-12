using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NumberTheory
{
    public static class NumberExtensions
    {
        #region (Big)Power

        /// <summary>
        /// calculates b^exp using exponentiation by squaring method
        /// 
        /// complexity is O( (exp log(b))^k )
        /// </summary>
        public static long Power(this long b, long exp)
        {
            if (b == 0 && exp == 0)
                throw new Exception("0^0 is not defined");
            else if (exp < 0)
                throw new Exception("Negative exponents not supported");
            else if (exp == 0)
                return 1;
            else if (exp == 1)
                return b;

            long result = 1;
            long ex = exp;
            long bas = b;
            while (ex > 0)
            {
                if (ex % 2 == 1)
                    result = (result * bas);
                ex = ex >> 1;
                if (ex > 0)
                    bas = (bas * bas);
            }
            return result;
        }

        public static ulong Power(this ulong b, ulong exp)
        {
            if (b == 0 && exp == 0)
                throw new Exception("0^0 is not defined");            
            else if (exp == 0)
                return 1;
            else if (exp == 1)
                return b;

            ulong result = 1;
            ulong ex = exp;
            ulong bas = b;
            while (ex > 0)
            {
                if (ex % 2 == 1)
                    result = (result * bas);
                ex = ex >> 1;
                if (ex > 0)
                    bas = (bas * bas);
            }
            return result;
        }

        public static BigInteger BigPower(this BigInteger b, int exp)
        {
            if (b.IsZero && exp == 0)
                throw new Exception("0^0 is not defined");
            else if (exp < 0)
                throw new Exception("Negative exponents not supported");
            else if (b.IsZero)
                return BigInteger.Zero;
            else if (b.IsOne || exp == 0)
                return BigInteger.One;

            BigInteger result = 1;
            int ex = exp;
            BigInteger bas = b;
            while (ex > 0)
            {
                if (ex % 2 == 1)
                    result = (result * bas);
                ex = ex >> 1;
                if (ex > 0)
                    bas = (bas * bas);
            }

            return result;
        }
        
        #endregion
        #region Exponential modulo b^(ex mod m)

        /// <summary>
        /// Calculates b^exp (mod modulo) using bit shift method
        /// Fast method, but required modulus to be smaller than 2^32        
        /// 
        /// Scales with O(log exp)
        /// </summary>                
        public static ulong ModPower(this ulong b, ulong exp, ulong modulus)
        {
            if (modulus == 1)
                return 0;
            else if (exp == 0)
                return 1;
            else if (exp == 1)
                return b % modulus;

            BigInteger result = BigInteger.One;
            BigInteger bas = new BigInteger(b) % modulus;
            BigInteger ex = new BigInteger(exp);
            while (ex > 0)
            {
                if (ex % 2 == 1)
                    result = (result * bas) % modulus;
                ex = ex >> 1;
                if (ex > 0)
                    bas = (bas * bas) % modulus;
            }

            return ulong.Parse(result.ToString());
        }

        #endregion
        #region Factorial n!

        /// <summary>
        /// computes n!
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ulong Factorial(this ulong n)
        {
            if (n == 0)
                return 1;
            ulong result = 1;
            for (ulong i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        public static long Factorial(this long n)
        {
            if (n == 0)
                return 1;
            long result = 1;
            for (long i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        public static BigInteger BigFactorial(this ulong n)
        {
            if (n == 0)
                return BigInteger.One;
            var result = BigInteger.One;
            for (ulong i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        public static BigInteger BigFactorial(this long n)
        {
            if (n == 0)
                return BigInteger.One;
            var result = BigInteger.One;
            for (long i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        public static BigInteger BigFactorial(this BigInteger n)
        {
            if (n == 0)
                return BigInteger.One;
            var result = BigInteger.One;
            for (long i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        #endregion
        #region Factorial div n!/k!

        /// <summary>
        /// Calulates n! / denominator!
        /// </summary>        
        public static long FactorialDiv(this long n, long denominator)
        {
            long result = 1;
            for (long i = denominator + 1; i <= n; i++)
                result *= i;
            return result;
        }

        #endregion
        #region Square Root

        /// <summary>
        /// returns the largest x for which x*x is smaller or equal to n
        /// </summary>
        /// <returns></returns>
        public static BigInteger Sqrt(this BigInteger n)
        {
            var x = n;
            var y = (x + 1) / 2;
            while (y < x)
            {
                x = y;
                y = (x + n / x) / 2;
            }
            return x;
        }

        #endregion
        #region Base Conversion
        /// <summary>
        /// Converts n to base bas, where bas is in the range [2..16]
        /// </summary>
        /// <param name="n"></param>
        /// <param name="bas"></param>
        /// <returns></returns>
        public static string ToBase(this long n, long bas)
        {
            char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string result = "";
            while (n > 0)
            {
                result += digits[n % bas];
                n /= bas;
            }
            return result;
        }

        #endregion
        #region CrossSum

        /// <summary>
        /// The sum of the digits
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long CrossSum(this long n)
        {
            long sum = 0;
            foreach (char c in n.ToString().ToCharArray())
                sum += long.Parse(c.ToString());
            return sum;
        }
        public static ulong CrossSum(this ulong n)
        {
            ulong sum = 0;
            foreach (char c in n.ToString().ToCharArray())
                sum += ulong.Parse(c.ToString());
            return sum;
        }

        #endregion
        #region Special Functions

        /// <summary>
        /// True if n is identical when reversed
        /// </summary>        
        public static bool IsPalindrom(this long n) => n.ToString().IsPalindrom();


        /// <summary>
        /// Converts the number in a string ordered by the digits
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string ToOrderedString(this ulong n)
        {
            string result = "";
            n.ToString().ToCharArray().OrderBy((c) => c).ToList().ForEach((c) => result += c);
            return result;
        }

        #endregion
    }
}
