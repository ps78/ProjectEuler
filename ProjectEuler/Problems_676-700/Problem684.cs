using NumberTheory;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=684
/// </summary>
public class Problem684 : EulerProblemBase
{
    public Problem684() : base(684, "Inverse Digit Sum", 0, 0) { }

    private static readonly ulong MOD = 1_000_000_007;

    public override long Solve(long n)
    {
        ulong sum = 0;
        for (int k = 2; k <= 90; k++)
        {
            ulong f = Fibonacci.Get(k);
            ulong Sf = S(f);
            Console.WriteLine($"{f}: {Sf}");
            sum += Sf;
        }
        return (long)(sum % MOD);
    }

    private static Dictionary<ulong, ulong> DecSums = new Dictionary<ulong, ulong>() {
        {0,  0},
        {1,  1},
        {2,  11 },
        {3,  111 },
        {4,  1111 },
        {5,  11111 },
        {6,  111111 },
        {7,  1111111 },
        {8,  11111111 },
        {9,  111111111 },
        {10, 1111111111 },
        {11, 11111111111 },
        {12, 111111111111 },
        {13, 1111111111111 },
        {14, 11111111111111 },
        {15, 111111111111111 },
        {16, 1111111111111111 },
        {17, 11111111111111111 },
        {18, 111111111111111111 }
    };

    private ulong S(ulong n)
    {
        ulong P = n / 9;
        ulong decSum = P > 14 ? DecSums[14] : DecSums[P];
        ulong baseSum = 54 * decSum - 9 * P;

        ulong remSum = 0;
        ulong dec = P > 14 ? (ulong)1E14 : (ulong)Math.Pow(10, P);
        for (ulong i = 1; i <= n - 9 * P; i++)
            remSum += ((dec - 1) + i * dec);

        return (baseSum + remSum) % MOD;
    }
}
