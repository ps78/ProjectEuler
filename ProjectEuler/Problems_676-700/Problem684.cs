using NumberTheory;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=684
/// </summary>
public class Problem684 : EulerProblemBase
{
    private static readonly long MOD = 7919; //1_000_000_007;
    
    public Problem684() : base(684, "Inverse Digit Sum", 0, 0) { }

    public override long Solve(long n)
    {
        var nums = new Dictionary<long, int>();
        for (int p = 1; p <= MOD/2; p++)
        {
            BigInteger x = BigInteger.Parse(new String('1', p));
            var m = (long)(x % MOD);
            if (nums.ContainsKey(m))
                nums[m]++;
            else
                nums[m] = 1;
        }
        nums
            .OrderBy(x => x.Key)
            .Select(x => $"{x.Key}: {x.Value}")
            .ToList()
            .ForEach(x => Console.WriteLine(x));

        //for (long i = 1; i <= 59; i++)
        //    Console.WriteLine($"{i} : {S(i,1009)}");

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

    /// <summary>
    /// calculates S(n) MOD M
    /// </summary>
    private long S(long n, long M)
    {        
        long P = ((n - 1) / 9);
        long r = (n - 9 * P);
        long D = PowerTenSum(P, M);
        long PowTen = PowerTen(P, M);

        P = P % M;
        r = r % M;
        
        long A = (54 * D - 9 * P) % M;

        long C = (r * (r + 1) / 2) % M;
        C = (C * PowTen) % M;
        C = C + (r * (PowTen - 1) % M);

        return (A + C) % M;
    }

    /// <summary>
    /// Computes 10^p mod M for large p
    /// </summary>
    public static long PowerTen(long exp, long modulus)
    {
        if (modulus == 1)
            return 0;
        else if (exp == 0)
            return 1;
        else if (exp == 1)
            return 10 % modulus;

        long result = 1;
        long bas = 10 % modulus;
        long ex = exp;
        while (ex > 0)
        {
            if (ex % 2 == 1)
                result = (result * bas) % modulus;
            ex = ex >> 1;
            if (ex > 0)
                bas = (bas * bas) % modulus;
        }

        return result;
    }

    /// <summary>
    /// Computes Sum(10^i) % M for i = 0..p-1
    /// Sum(10^i), i=0..p-1
    /// corresonds to the number consisting of p 1's
    /// 
    /// the largest P for the given problem is 320007466041201791 (3.2*10^17)
    /// </summary>
    private long PowerTenSum(long P, long M)
    {
        //return Enumerable.Range(0, (int)P).Select(p => PowerTen(p,M)).Sum() % M;
        return P == 0 ? 0 : long.Parse(new String('1', (int)P)) % M;

        // TODO: now to solve this for very large P??
        // We know PowerTenSums(P,M) is cyclic with cycle M (or even M/2)
        // Hence computing the sum for one cycle and then multipyling it 
        // cuts down the computation effort, but it's still too large as M~10^9
    }
}
