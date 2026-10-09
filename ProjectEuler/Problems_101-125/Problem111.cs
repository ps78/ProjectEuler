using NumberTheory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=111
/// 
/// Considering 4-digit primes containing repeated digits it is clear that they cannot all be the same:
/// 1111 is divisible by 11, 2222 is divisible by 22, and so on.But there are nine 4-digit primes containing three ones:
///    1117, 1151, 1171, 1181, 1511, 1811, 2111, 4111, 8111
/// We shall say that M(n,d) represents the maximum number of repeated digits for an n-digit prime 
/// where d is the repeated digit, N(n,d) represents the number of such primes, and S(n,d) represents the sum of these primes.
/// So M(4,1)=3 is the maximum number of repeated digits for a 4-digit prime where one is the repeated digit, 
/// there are N(4n1)=9 such primes, and the sum of these primes is S(4,1)=22275. 
/// It turns out that for d=0, it is only possible to have M(4,0)=2 repeated digits, but there are N(4,0)=13 such cases.
/// In the same way we obtain the following results for 4-digit primes.
/// 
/// Digit, d    M(4, d)   N(4, d)   S(4, d)
///    0           2        13      67061
///    1           3        9       22275
///    2           3        1        2221
///    3           3        12      46214
///    4           3        2        8888
///    5           3        1        5557
///    6           3        1        6661
///    7           3        9       57863
///    8           3        1        8887
///    9           3        7       48073
///    
/// For d=0 to 9, the sum of all S(4,d) is 273700.
/// Find the sum of all S(10,d).
/// </summary>
public class Problem111 : EulerProblemBase
{
    public Problem111() : base(111, "Primes with Runs", 10, 612407567715) { }

    public override long Solve(long n)
    {
        var primeTest = new SimplePrimeTest();
        
        ulong sum = 0;
        for (int digit = 0; digit <= 9; digit++)
        {
            ulong digitSum = 0;
            for (int repeat = (int)n - 1; repeat >= 2; repeat--)
            { 
                foreach (var candidate in ConstructCandidates((int)n, repeat, digit))
                {
                    if (primeTest.IsPrime(candidate))
                        digitSum += candidate;
                }
                if (digitSum > 0)
                    break;
            }
            sum += digitSum;
        }

        return (long)sum;
    }

    private List<ulong> ConstructCandidates(int nDigits, int nRepeat, int digitToRepeat)
    {
        // the positions of the non-repeated digits.
        var otherDigitPos = Combination.Create(Enumerable.Range(0, nDigits).ToArray(), nDigits - nRepeat);

        // all possible tuples of the non-repeated digits. Order matters here (1009 and 9001 both have to be
        // generated), so this is the cartesian product otherDigits^m, not combinations with repetition.
        var otherDigits = Enumerable.Range(0, 10).Where(d => d != digitToRepeat).Select(d => d.ToString()[0]).ToArray();
        int m = nDigits - nRepeat;
        int nTuples = (int)Math.Pow(otherDigits.Length, m);
        var otherDigitsComb = new char[nTuples][];
        for (int t = 0; t < nTuples; t++)
        {
            otherDigitsComb[t] = new char[m];
            for (int i = 0, r = t; i < m; i++, r /= otherDigits.Length)
                otherDigitsComb[t][i] = otherDigits[r % otherDigits.Length];
        }

        // the number with all repeated digits, e.g. 1111, 2222, etc.
        char[] baseNumber = Enumerable.Repeat(digitToRepeat.ToString()[0], nDigits).ToArray();

        List<ulong> candidates = new();

        // chose the positions where to insert the non-repeating digits
        foreach (var posComb in otherDigitPos)
        {
            foreach (var digitComb in otherDigitsComb)
            {
                char[] number = (char[])baseNumber.Clone();
                for (int i = 0; i < m; i++)
                {
                    number[posComb[i]] = digitComb[i];
                }
                if (number[0] != '0' && 
                    number[nDigits-1] != '0' &&
                    number[nDigits - 1] != '2' &&
                    number[nDigits - 1] != '4' &&
                    number[nDigits - 1] != '5' &&
                    number[nDigits - 1] != '6' &&
                    number[nDigits - 1] != '8')
                    candidates.Add(ulong.Parse(new string(number)));
            }
        }

        return candidates.Distinct().ToList();
    }
}
