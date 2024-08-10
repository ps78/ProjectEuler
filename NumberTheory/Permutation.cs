using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory;

public static class Permutation
{
    /// <summary>
    /// The number of ways to arrange elements in a set of n elements
    /// without repetitions
    /// 
    /// this is simply n!
    /// </summary>
    public static ulong Count(int n) => ((ulong)n).Factorial();
    
    /// <summary>
    /// Creates all permutations of the given set
    /// </summary>
    public static IEnumerable<S> Create<T, S>(S set) where S: IList<T>
    {
        return new PermutationEnumerable<T, S>(set);
    }
}

public class PermutationEnumerable<T, S> : IEnumerable<S> where S : IList<T>
{
    private readonly S Arr;

    public PermutationEnumerable(S arr) => Arr = arr;

    public IEnumerator<S> GetEnumerator() => new PermutationEnumerator<T, S>(Arr);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PermutationEnumerator<T, S> : IEnumerator<S> where S: IList<T>
{
    // lenght of set
    private readonly int N;

    // The input array for permutation
    private readonly T[] Arr;

    // Index array to store indexes of input array
    private readonly int[] Indexes;

    // The index of the first "increase"
    // in the Index array which is the smallest
    // i such that Indexes[i] < Indexes[i + 1]
    private int Increase;

    // Constructor
    public PermutationEnumerator(S arr)
    {
        Arr = arr.ToArray();
        N = Arr.Length;
        Indexes = new int[N];
        Increase = -1;
    }

    public S Current
    {
        get
        {
            if (Increase == -1)
                return GetFirst();
            else
                return GetNext(); // should throw an InvalidOperationException if past end of array
        }
    }

    object IEnumerator.Current => Current;

    // Initialize and output
    // the first permutation
    private S GetFirst()
    {
        // Initialize the values in Index array
        // from 0 to n - 1
        for (int i = 0; i < Indexes.Length; ++i)
        {
            Indexes[i] = i;
        }

        // Set the Increase to 0
        // since Indexes[0] = 0 < Indexes[1] = 1
        Increase = 0;

        // Output the first permutation
        return Output();
    }

    // Function that returns true if it is
    // possible to generate the next permutation
    private bool HasNext()
    {
        // When Increase is in the end of the array,
        // it is not possible to have next one
        return Increase != (N - 1);
    }

    // Output the next permutation
    private S GetNext()
    {
        // Increase is at the very beginning
        if (Increase == 0)
        {
            // Swap Index[0] and Index[1]
            Swap(Increase, Increase + 1);

            // Update Increase
            Increase += 1;
            while (Increase < N - 1 && Indexes[Increase] > Indexes[Increase + 1])
            {
                ++Increase;
            }
        }
        else
        {
            // Value at Indexes[Increase + 1] is greater than Indexes[0]
            // no need for binary search,
            // just swap Indexes[Increase + 1] and Indexes[0]
            if (Indexes[Increase + 1] > Indexes[0])
            {
                Swap(Increase + 1, 0);
            }
            else
            {
                // Binary search to find the greatest value
                // which is less than Indexes[Increase + 1]
                int start = 0;
                int end = Increase;
                int mid = (start + end) / 2;
                int tVal = Indexes[Increase + 1];
                while (!(Indexes[mid] < tVal && Indexes[mid - 1] > tVal))
                {
                    if (Indexes[mid] < tVal)
                    {
                        end = mid - 1;
                    }
                    else
                    {
                        start = mid + 1;
                    }
                    mid = (start + end) / 2;
                }

                // Swap
                Swap(Increase + 1, mid);
            }

            // Invert 0 to Increase
            for (int i = 0; i <= Increase / 2; ++i)
            {
                this.Swap(i, Increase - i);
            }

            // Reset Increase
            Increase = 0;
        }
        return Output();
    }

    // Function to output the input array
    private S Output()
    {
        IList<T> result = new T[N];
        for (int i = 0; i < N; ++i)
        {
            // Indexes of the input array
            // are at the Indexes array
            result[i] = Arr[Indexes[i]];
        }
        return (S)result;
    }

    // Swap two values in the Indexes array
    private void Swap(int p, int q)
    {
        int tmp = Indexes[p];
        Indexes[p] = Indexes[q];
        Indexes[q] = tmp;
    }

    public void Dispose() { }
    
    public bool MoveNext()
    {
        if (!HasNext())
            return false;
        else
            return true;
    }

    public void Reset()
    {
        for (int i = 0; i < Indexes.Length; i++)
            Indexes[i] = 0;

        Increase = -1;
    }
}