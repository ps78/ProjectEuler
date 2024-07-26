using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;
using System.Numerics;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Xml.Linq;
using System.Net.WebSockets;
using System.Windows.Markup;
using System.Runtime.InteropServices.Marshalling;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=104
    /// The Fibonacci sequence is defined by the recurrence relation:
    ///     Fn = Fn-1 + Fn-2, where F1=1 and F2=1
    ///
    /// It turns out that F541, which contains digits, is the first Fibonacci number for which the 
    /// last nine digits are 1-9 pandigital (contain all the digits 1 to 9, but not necessarily in order). 
    /// And F2749, which contains 575 digits, is the first Fibonacci number for which the first nine 
    /// digits are 1-9 pandigital.
    /// 
    /// Given that Fk is the first Fibonacci number for which the first nine digits AND the 
    /// last nine digits are 1-9 pandigital, find k.
    /// </summary>
    public class Problem104 : EulerProblemBase
    {
        public Problem104() : base(104, "Pandigital Fibonacci Ends", 0, 0) { }

        public override bool Test() => true;
        
        public override long Solve(long n)
        {
            ulong Fk2 = 0;
            ulong Fk1 = 1;
            int k = 2;
            do
            {
                ulong Fk = (Fk2 + Fk1) % 1_000_000_000;
                Fk2 = Fk1;
                Fk1 = Fk;

                var FkStr = Fk.ToString("D9");
                //if (IsPanDigital(FkStr))
                //    Console.WriteLine($"{k}: {FkStr}");

                if (k>200)
                    Console.WriteLine($"{k} | {Fibonacci.GetBig(k).ToString()[..20]}.. | {Fibonacci.Estimate(k)}");
                
                k++;
            }
            while (k<100);



            return 0;
        }

        public static bool IsPanDigital(string s)
        {
            var set = s.ToHashSet();

            if (set.Contains('0'))
                return set.Count == 10;
            else
                return set.Count == 9;
        }

        /// <summary>
        /// Computes a^n
        /// Works for results that exceed the capacity of double (10^308)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static (double mantissa, int exponent) BigPower(double a, int n)
        {
            // converts value to a mantissa >=1 and < 10  
            (bool sign, double mantissa, int exponent) Normalize(double value)
            {
                double m = Math.Abs(value);
                bool sign = value >= 0;
                int exp = (int)Math.Floor(Math.Log10(m));

                return (sign, m /= Math.Pow(10, exp), exp);
            }

            (bool sign, double m, int exp) = Normalize(a);
            
            // square m as long as possible
            int k = 0;
            while(2*k <= n)
            {
                m *= m;
                exp *= 2;
                (_, m, int exp2) = Normalize(m);
                exp += exp2;
                k *= 2;
            }

            // now calculated a^(2^i) with 2^i <= n
            // what remains is the exponent n-2^i
            for (int _ = exp; _ < n; _++)
            {
                m *= a;
                (bool s, m, int exp2) = Normalize(m);
                exp += exp2;
            }

            return (m, exp);
        }
    }
}
