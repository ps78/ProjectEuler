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
        if (k > n)
            return 0;
        else if (k == n)
            return 1;
        else
        {
            long mx = Math.Max(k, n - k);
            long mn = Math.Min(k, n - k);

            long numerator = n;
            for (long i = n - 1; i > mx; i--)
                numerator *= i;

            long denominator = mn;
            for (long i = mn - 1; i > 1; i--)
                denominator *= i;

            return numerator / denominator;
        }
    }

    public static IEnumerable<IList<T>> Create<T>(IList<T> set, int k)
    {
        int n = set.Count;
        if (k > n)
            yield break;

        var setIdx = new int[k];
        for (int i = 0; i < k; i++)
            setIdx[i] = i;

        var minIdx = new int[k];
        for (int i = 0; i < k; i++)
            minIdx[i] = 0;

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
}
