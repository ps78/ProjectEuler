using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory;

public static class Combination
{
    /// <summary>
    /// The number of ways to pick k elements from a set of n elements
    /// (without repetitions). Order does not matter
    /// 
    /// Calculates n! / (k! (n-k)!)
    /// </summary>
    public static long Count(int n, int k)
    {
        if (k < 0 || k > n)
            return 0;

        int mn = Math.Min(k, n - k);

        // Multiplicative formula: after step i, result = C(n - mn + i, i), so every division is exact.
        // The intermediate product result * (n - mn + i) is kept in Int128 to avoid premature overflow;
        // the results are increasing in i, so checking the final value suffices.
        Int128 result = 1;
        for (int i = 1; i <= mn; i++)
            result = result * (n - mn + i) / i;

        return checked((long)result);
    }

    public static IEnumerable<IList<T>> Create<T>(IList<T> set, int k)
    {
        int n = set.Count;
        if (k < 0 || k > n)
            yield break;

        var setIdx = new int[k];
        for (int i = 0; i < k; i++)
            setIdx[i] = i;

        var workingSet = new T[k];

        while (true)
        {
            for (int i = 0; i < k; i++)
                workingSet[i] = set[setIdx[i]];

            yield return (IList<T>)workingSet.Clone();

            int idxToIncrease = -1;
            for (int j = k - 1; j >= 0; j--)
                if (setIdx[j] < n - k + j)
                {
                    idxToIncrease = j;
                    break;
                }

            if (idxToIncrease == -1)
                yield break;

            setIdx[idxToIncrease]++;
            for (int j = idxToIncrease + 1; j < k; j++)
            {
                int nextIdx = setIdx[idxToIncrease] + j - idxToIncrease;
                if (nextIdx > n - 1)
                    yield break;
                setIdx[j] = nextIdx;
            }
        }
    }

    /// <summary>
    /// The number of k-combinations from n elements with repetition allowed.
    /// Formula: C(n + k - 1, k)
    /// </summary>
    public static long CountWithRepetition(int n, int k)
    {
        if (k < 0 || n < 0)
            return 0;
        // The empty multiset is the only 0-combination, even for n == 0 (where the formula would give C(-1, 0)).
        if (k == 0)
            return 1;
        // If n == 0 and k > 0 there are no combinations; the formula below will return 0 via Count.
        return Count(n + k - 1, k);
    }

    /// <summary>
    /// Generate k-combinations from the given set allowing repetitions.
    /// Each combination is in non-decreasing order (indices may repeat).
    /// Example for set {a,b,c}, k=2: aa, ab, ac, bb, bc, cc
    /// </summary>
    public static IEnumerable<IList<T>> CreateWithRepetition<T>(IList<T> set, int k)
    {
        int n = set.Count;
        if (k < 0)
            yield break;
        if (n == 0)
        {
            if (k == 0)
                yield return Array.Empty<T>();
            yield break;
        }

        var idx = new int[k];
        for (int i = 0; i < k; i++)
            idx[i] = 0;

        var working = new T[k];

        while (true)
        {
            for (int i = 0; i < k; i++)
                working[i] = set[idx[i]];

            yield return (IList<T>)working.Clone();

            int pos = -1;
            for (int j = k - 1; j >= 0; j--)
            {
                if (idx[j] < n - 1)
                {
                    pos = j;
                    break;
                }
            }

            if (pos == -1)
                yield break;

            idx[pos]++;
            // set all following positions to the same value to keep non-decreasing order
            for (int j = pos + 1; j < k; j++)
                idx[j] = idx[pos];
        }
    }
}
