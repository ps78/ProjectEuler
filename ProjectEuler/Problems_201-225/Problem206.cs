using NumberTheory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=206
/// 
/// Find the unique positive integer whose square has the form 1_2_3_4_5_6_7_8_9_0,
/// where each “_” is a single digit.
/// </summary>
public class Problem206 : EulerProblemBase
{
    public Problem206() : base(206, "Concealed Square", 0, 1389019170) { }

    public override long Solve(long n)
    {
        string pattern = "1_2_3_4_5_6_7_8_9_0";
        ulong lBound = (ulong)Math.Sqrt(ulong.Parse(pattern.Replace("_", "0")));
        ulong uBound = (ulong)Math.Sqrt(ulong.Parse(pattern.Replace("_", "9")));

        while (uBound % 10 != 0) uBound--;

        for (ulong i = uBound; i >= lBound; i -= 10)
            if (IsMatch(i*i))
                return (long)i;

        return 0;
    }

    private static bool IsMatch(ulong n)
    {
        if ((n % 1000) / 100 != 9) return false;
        if ((n % 100000) / 10000 != 8) return false;
        if ((n % 10000000) / 1000000 != 7) return false;
        if ((n % 1000000000) / 100000000 != 6) return false;
        if ((n % 100000000000) / 10000000000 != 5) return false;
        if ((n % 10000000000000) / 1000000000000 != 4) return false;
        if ((n % 1000000000000000) / 100000000000000 != 3) return false;
        if ((n % 100000000000000000) / 10000000000000000 != 2) return false;
        return true;
    }
}