using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory;

/// <summary>
/// Provides a fast way to (exactly) count the number of primes up to some limit.
/// 
/// Code based on https://codeforces.com/blog/entry/91632
/// 
/// Runs in O(n^(2/3))
/// 
/// Performance (on laptop):
/// 
///     CountPrimes(10^9)  ~ 17 ms
///     CountPrimes(10^10) ~ 58 ms
///     CountPrimes(10^11) ~ 311 ms
///     CountPrimes(10^12) ~ 1 s
///     CountPrimes(10^13) ~ 5 s
///     CountPrimes(10^14) ~ 24 s
///     
/// Works for N <= ~3.8*10^14
/// </summary>
public static class CountPrimes
{
    public const ulong MaxN = 381_189_048_534_338; // = (2^31) ^ (1/0.64) - 1

    private struct FenwickTree
    {
        private int[] tree;
        private int n;

        public FenwickTree(int N)
        {
            n = N;
            tree = new int[n];
        }

        public void Add(int i, int k)
        {
            for (; i < n; i = (i | (i + 1)))
                tree[i] += k;
        }

        public int Ask(int r)
        {
            int res = 0;
            for (; r >= 0; r = (r & (r + 1)) - 1)
                res += tree[r];
            return res;
        }
    };

    public static ulong Get(ulong N)
    {
        if (N > MaxN)
            throw new ArgumentOutOfRangeException($"N must by <= {MaxN}");

        long n = (long)N;

        List<int> primes = [];
        int[] mnprimes = [];
        long ans;
        long y;
        List<((long, int), int)> queries = [];

        // this y is actually n/y
        // also no logarithms, welcome to reality, this y is the best for n=10^12 or n=10^13
        y = (long)Math.Pow(n, 0.64);
        if (n < 100) 
            y = n;

        // linear sieve
        primes.Clear();
        mnprimes = Enumerable.Repeat(-1, (int)y + 1).ToArray();
        ans = 0;

        for (int i = 2; i <= y; ++i)
        {
            if (mnprimes[i] == -1)
            {
                mnprimes[i] = primes.Count;
                primes.Add(i);
            }
            for (int k = 0; k < primes.Count; ++k)
            {
                int j = primes[k];

                if (i * j > y) 
                    break;
                
                mnprimes[i * j] = k;
                
                if (i % j == 0) 
                    break;
            }
        }
        if (n < 100) 
            return (ulong)primes.Count;

        long s = n / y;

        foreach (long p in primes)
        {
            if (p > s) 
                break;
            ans++;
        }
        // pi(n / y)
        int ssz = (int)ans;

        // F with two pointers
        int ptr = primes.Count - 1;
        for (int i = ssz; i < primes.Count; ++i)
        {
            while (ptr >= i && (long)primes[i] * primes[ptr] > n)
                --ptr;
            
            if (ptr < i) 
                break;
            
            ans -= ptr - i + 1;
        }

        // phi, store all queries 
        Phi(n, ssz - 1, 1, primes, y, ref ans, ref queries);

        queries = queries.OrderBy(q => q).ToList();

        int ind = 2;
        int sz = primes.Count;

        // the order in fenwick will be reversed, because prefix sum in a fenwick is just one query
        var fw = new FenwickTree(sz);
        //for (auto[na, sign] : queries)
        foreach (var query in queries)
        {
            var (na, sign) = query;
            var (num, a) = na;
            while (ind <= num)
                fw.Add(sz - 1 - mnprimes[ind++], 1);
            ans += (fw.Ask(sz - a - 2) + 1) * sign;
        }
        queries.Clear();
        primes.Clear();

        return (ulong)(ans - 1);
    }

    private static void Phi(long n, int a, int sign, 
                           List<int> primes, long y, ref long ans, ref List<((long, int), int)> queries)
    {
        if (n == 0) 
            return;
        
        if (a == -1)
        {
            ans += n * sign;
            return;
        }
        if (n <= y)
        {
            queries.Add(((n, a), sign));
            return;
        }
        Phi(n, a - 1, sign, primes, y, ref ans, ref queries);
        Phi(n / primes[a], a - 1, -sign, primes, y, ref ans, ref queries);
    }
}
