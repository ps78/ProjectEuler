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
        public Problem104() : base(104, "Pandigital Fibonacci Ends", 0, 329468) { }

        public override bool Test() => true;

        public override long Solve(long n)
        {
            ulong Fk2 = 0;
            ulong Fk1 = 1;
            int k = 2;
            while(true)
            {
                // compute the last 9 digits of the next Fibonacci number
                ulong Fk = (Fk2 + Fk1) % 1_000_000_000;
                Fk2 = Fk1;
                Fk1 = Fk;

                // if these last 9 digits are 1-9 pan-digital, compute the first digits of Fk
                var endingDigits = Fk.ToString("D9");
                if (Is19PanDigital(endingDigits))
                {                    
                    BigDecimal Fkbig = Fibonacci.EstimateBig(k);
                    string leadingDigits = (Fkbig.Mantissa * 1E9m).ToString()[..9];
                    if (Is19PanDigital(leadingDigits))
                    {
                        return k;
                    }
                }
                k++;
            }
        }

        private static bool Is19PanDigital(string s)
        {
            var set = s.ToHashSet();

            if (set.Contains('0'))
                return set.Count == 10;
            else
                return set.Count == 9;
        }

    }
}
