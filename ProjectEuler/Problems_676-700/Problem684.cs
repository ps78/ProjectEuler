using NumberTheory;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=684
/// </summary>
public class Problem684 : EulerProblemBase
{
    private static readonly Dictionary<ulong, ulong> PowerOfTen;
    private static Dictionary<ulong, ulong> PowerOfTenSums;
    private static readonly ulong MOD = 1_000_000_007;
    static Problem684()
    {
        PowerOfTen = Enumerable.Range(0, 19).Select(i => ((ulong)i, (ulong)Math.Pow(10, i) % MOD)).ToDictionary();
        PowerOfTenSums = Enumerable.Range(1, 18).Select(i => ((ulong)i, ulong.Parse(new String('1', i)) % MOD)).ToDictionary();
        PowerOfTenSums[0] = 0;
    }

    public Problem684() : base(684, "Inverse Digit Sum", 0, 0) { }

    public override long Solve(long n)
    {
        ulong P = 12;
        Console.WriteLine($"{PowerOfTenSums[P]}");
        Console.WriteLine($"{PowerOfTenSumMod(P)}");

        ulong sum = 0;
        for (ulong k = 0; k < P; k++)
        {
            sum = sum + (ulong)Math.Pow(10, k);
        }
        Console.WriteLine($"{sum % MOD}");


        for (ulong i = 1; i <= 45; i++)
            Console.WriteLine($"{i} : {S(i)}");

        /*
        ulong sum = 0;
        for (int k = 2; k <= 90; k++)
        {
            ulong f = Fibonacci.Get(k);
            ulong Sf = S(f);
            Console.WriteLine($"{f}: {Sf}");
            sum += Sf;
        }
        return (long)(sum % MOD);
        */
        // wrong: 131683844
        return 0;
    }

    private ulong S(ulong n)
    {
        // S(45) = 599949
        ulong P = n / 9;
        ulong idx = Math.Min(P, 14);
        ulong baseSum = 54 * PowerOfTenSums[idx] - 9 * P;

        ulong remSum = 0;
        for (ulong i = 1; i <= n - 9 * P; i++)
            remSum += ((PowerOfTen[idx] - 1) + i * PowerOfTen[idx]);

        return (baseSum + remSum) % MOD;
    }

    /// <summary>
    /// Calculates Sum[k=0..P](10^k) % MOD
    /// </summary>
    /// <param name="P"></param>
    /// <returns></returns>
    private ulong PowerOfTenSumMod(ulong P)
    {
        if (P <= 10)
            return PowerOfTenSums[P];
        else
            return (PowerOfTenSumMod(P / 2) * PowerOfTenSumMod(P - P / 2)) % MOD;
    }
}
