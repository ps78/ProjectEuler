using NumberTheory;
using System.Numerics;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=1013
///
/// The number 13521270961 is the first number that has exactly 25 divisors and the sum of those
/// divisors is divisible by 25.
/// Find the sum of all numbers less than 10^15 that have exactly 25 divisors and the sum of those
/// divisors is divisible by 25.
///
/// REASONING
///
/// 1) Shape of n
///    For n = p1^e1 * ... * pk^ek the number of divisors is d(n) = (e1+1)*...*(ek+1).
///    The only ways to write 25 as such a product are 25 and 5*5, hence
///        n = p^24        or        n = p^4 * q^4  (p &lt; q primes)
///
/// 2) Divisor sum modulo 25
///    sigma is multiplicative with sigma(p^e) = 1 + p + ... + p^e.
///    Let S(x) = 1 + x + x^2 + x^3 + x^4. For any integer x:
///      a) x = 1 (mod 5): write x = 1 + 5k. Then x^i = 1 + 5ki (mod 25), so
///         S(x) = 5 + 5k*(0+1+2+3+4) = 5 + 50k = 5 (mod 25)
///         i.e. S(x) is divisible by 5, but not by 25.
///      b) x = 0 (mod 5): S(x) = 1 (mod 5).
///      c) otherwise x^5 = x (mod 5) (Fermat), so (x-1)*S(x) = x^5 - 1 = x - 1 (mod 5), and since
///         x-1 is invertible mod 5, S(x) = 1 (mod 5).
///    So S(x) contributes exactly one factor 5 if x = 1 (mod 5), and none otherwise.
///
///    - n = p^4 q^4: sigma(n) = S(p) * S(q). This is divisible by 25 iff both p and q are 1 (mod 5).
///    - n = p^24:    sigma(n) = (p^25-1)/(p-1) = S(p) * S(p^5), using
///                   x^25-1 = (x-1) * (1+x+...+x^4) * (1+x^5+...+x^20).
///                   Since p^5 = p (mod 5), both factors carry a 5 iff p = 1 (mod 5).
///
///    In both cases: 25 | sigma(n)  &lt;=&gt;  every prime factor of n is 1 (mod 5).
///
/// 3) Size of the search space
///    The smallest prime that is 1 (mod 5) is 11.
///    - p^24: 11^24 ~ 9.8*10^24 is far beyond 10^15, so this case contributes nothing for the actual
///      problem size. It is still handled in the code for completeness.
///    - p^4 q^4 = (pq)^4 &lt; N  &lt;=&gt;  pq &lt;= M, where M is the largest integer with M^4 &lt; N.
///      For N = 10^15, M = 5623. Since p >= 11, q &lt;= M/11 = 511, so we only need the primes up to 511.
///
///    Sanity check: the two smallest primes that are 1 (mod 5) are 11 and 31, and (11*31)^4 = 341^4 =
///    13521270961, which is exactly the number given in the problem statement.
///
/// The solution is thus a double loop over a few dozen primes and runs in microseconds.
/// Test() verifies the mod-5 argument against a brute-force version that computes sigma(n)
/// explicitly for all numbers with 25 divisors, using all primes.
/// </summary>
public class Problem1013 : EulerProblemBase
{
    /// <summary>
    /// The smallest prime p with p = 1 (mod 5), i.e. the smallest possible prime factor of a solution
    /// </summary>
    private const long SmallestPrime1Mod5 = 11;

    public Problem1013() : base(1013, "Sum 25 Divisors", 1_000_000_000_000_000, 6_991_971_159_155_793) { }

    public override bool Test()
    {
        // the example from the problem statement is the smallest such number (and "less than" is strict)
        if (Solve(13_521_270_961) != 0 || Solve(13_521_270_962) != 13_521_270_961)
            return false;

        // cross-check the mod-5 shortcut with explicitly computed divisor sums for various limits
        for (long limit = 10; limit <= ProblemSize; limit *= 10)
            if (Solve(limit) != SolveBruteForce(limit))
                return false;

        return true;
    }

    /// <summary>
    /// Sum of all numbers below n with exactly 25 divisors whose divisor sum is divisible by 25,
    /// i.e. (see reasoning above) the sum of all p^24 and (pq)^4 below n with p, q = 1 (mod 5)
    /// </summary>
    public override long Solve(long n)
    {
        long maxPQ = MaxBaseBelow(n, 4);   // (pq)^4 < n  <=>  pq <= maxPQ
        long maxP24 = MaxBaseBelow(n, 24); // p^24 < n    <=>  p <= maxP24

        // the larger prime q of p*q is at most maxPQ / 11 as p >= 11
        long primeLimit = Math.Max(maxPQ / SmallestPrime1Mod5, maxP24);
        var primes = new SieveOfEratosthenes((ulong)primeLimit)
            .GetPrimes()
            .Where(p => p % 5 == 1 && (long)p <= primeLimit)
            .Select(p => (long)p)
            .ToArray();

        long sum = 0;
        checked
        {
            // n = p^24
            foreach (var p in primes.TakeWhile(p => p <= maxP24))
                sum += p.Power(24);

            // n = (pq)^4 with p < q
            for (int i = 0; i < primes.Length && primes[i] * primes[i] < maxPQ; i++)
                for (int j = i + 1; j < primes.Length && primes[i] * primes[j] <= maxPQ; j++)
                    sum += (primes[i] * primes[j]).Power(4);
        }
        return sum;
    }

    /// <summary>
    /// Reference implementation used to verify Solve(): enumerates all numbers below n with exactly
    /// 25 divisors (p^24 and p^4 q^4 for *all* primes) and checks 25 | sigma explicitly.
    /// Does not rely on the mod-5 argument.
    /// </summary>
    private static long SolveBruteForce(long n)
    {
        long maxPQ = MaxBaseBelow(n, 4);
        long maxP24 = MaxBaseBelow(n, 24);

        // here the smallest prime factor is 2, so q <= maxPQ / 2
        var primes = new SieveOfEratosthenes((ulong)Math.Max(maxPQ / 2, maxP24))
            .GetPrimes()
            .Select(p => (long)p)
            .ToArray();

        long sum = 0;
        checked
        {
            foreach (var p in primes.TakeWhile(p => p <= maxP24))
                if (SigmaOfPrimePower(p, 24) % 25 == 0)
                    sum += p.Power(24);

            for (int i = 0; i < primes.Length; i++)
                for (int j = i + 1; j < primes.Length && primes[i] * primes[j] <= maxPQ; j++)
                    if (SigmaOfPrimePower(primes[i], 4) * SigmaOfPrimePower(primes[j], 4) % 25 == 0)
                        sum += (primes[i] * primes[j]).Power(4);
        }
        return sum;
    }

    /// <summary>
    /// sigma(p^e) = 1 + p + ... + p^e = (p^(e+1) - 1) / (p - 1)
    /// </summary>
    private static BigInteger SigmaOfPrimePower(long p, int e) => (BigInteger.Pow(p, e + 1) - 1) / (p - 1);

    /// <summary>
    /// Returns the largest integer m >= 0 with m^k &lt; n, i.e. the largest base whose k-th power
    /// is still below the (exclusive) limit n. Starts from the floating point estimate and
    /// corrects rounding errors with exact arithmetic.
    /// </summary>
    private static long MaxBaseBelow(long n, int k)
    {
        long m = (long)Math.Pow(n, 1.0 / k);
        while (m > 0 && BigInteger.Pow(m, k) >= n)
            m--;
        while (BigInteger.Pow(m + 1, k) < n)
            m++;
        return m;
    }
}
