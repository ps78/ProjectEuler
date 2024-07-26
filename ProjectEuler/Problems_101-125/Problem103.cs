using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;
using System.Numerics;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Xml.Linq;
using System.Net.WebSockets;
using System.Windows.Markup;
using System.Runtime.InteropServices.Marshalling;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=103
    /// 
    /// Let S(A) represent the sum of elements in set A of size n. We shall call it a special sum set if for any 
    /// two non-empty disjoint subsets B and C the following properties are true:
    /// 
    ///     1. S(B) != S(C); that is, sums of subsets cannot be equal.
    ///     2. If B contains more elements than C then S(B) > S(C)
    ///     
    /// For example {81, 88, 75, 42, 87, 84, 86, 65} is not a special sum set because 65+87+88=75+81+84,
    /// whereas {157,150,164,119,79,159,161,139,158} satisfies both rules for all possible subset pair 
    /// combinations and S(A)=1286.
    /// 
    /// Using sets.txt (right click and "Save Link/Target As..."), a 4K text file with one-hundred sets 
    /// containing seven to twelve elements (the two examples given above are the first two sets in the file), 
    /// identify all the special sum sets, A1, A2, .. Ak and find the value of S(A1)+S(A2)+ .. +S(Ak).
    /// </summary>
    public class Problem103 : EulerProblemBase
    {
        public Problem103() : base(103, "Special Subset Sums: Optimum", 7, 20313839404245) { }

        public override bool Test() => 
            Solve(3)==234 && 
            Solve(4)==3567 && 
            Solve(5)==69111213 &&
            Solve(6)==111819202225;
        
        public override long Solve(long n)
        {
            var set = new int[n];

            // assume that for  n=7 the first element should be larger than the 11 (the first element of n=6)
            set[0] = n <= 6 ? 1 : (int)n+13;

            // set heuristic upper limit
            int upperLimit = (int)(n * n);

            var specialSumSets = new List<int[]>();

            RecursiveConstruction(set, 0, upperLimit, ref specialSumSets);

            var optimalSet = specialSumSets.OrderBy(s => S(s)).First();

            long result = long.Parse(string.Join("", optimalSet.Select(x => x.ToString())));

            return result;
        }

        private void RecursiveConstruction(int[] set, int indexPos, 
                                           int upperLimit,
                                           ref List<int[]> specialSumSets)
        {
            // terminate recursion once the current element is above the limit
            if (set[indexPos] > upperLimit)
                return;

            var setCopy = (int[])set.Clone();

            int lowerLimit = indexPos == 1 ? setCopy[0]*3/2 : setCopy[indexPos];

            for (int i = lowerLimit; i <= upperLimit; i++)
            {
                set[indexPos] = i;
                for (int j = indexPos + 1; j < set.Length; j++)
                    set[j] = set[j - 1] + 1;

                //Console.WriteLine(string.Join(',', set));

                if (TestRule1(set) && TestRule2(set))
                    specialSumSets.Add((int[])set.Clone());

                if (indexPos < set.Length - 1)
                    RecursiveConstruction(set, indexPos + 1, upperLimit, ref specialSumSets);
            }

            Array.Copy(setCopy, set, set.Length);
        }

        private static int S(IEnumerable<int> set) => set.Sum();

        /// <summary>
        /// set must be sorted!
        /// </summary>
        private static bool TestRule1(int[] set)
        {
            var setArray = set.ToArray();
            int n = set.Length;

            // we only need to test set pairs (x, x) of size x for x=2..n/2
            int[] setSizesToTest = set.Length <= 1 ? Array.Empty<int>() : Enumerable.Range(2, n / 2 - 1).ToArray();

            foreach (int setSize in setSizesToTest)
            {
                var sets = Combination.Create(setArray, setSize).ToArray();
                for (int set1Idx = 0; set1Idx < sets.Length; set1Idx++)
                    for (int set2Idx = set1Idx + 1; set2Idx < sets.Length; set2Idx++)
                        if (S(sets[set1Idx]) == S(sets[set2Idx]))
                            return false;
            }

            return true;
        }

        /// <summary>
        /// For every possible set sets A, B, with [A]<=[B] (A has fewer elements) we could compare, 
        /// we actually only need to consider the biggest possible sub set A vs. the smallest 
        /// possible sub set B of the respective size (in terms of the S() function).
        /// To find the biggest / smallest sub set of 
        /// a given size is trivial, as set is sorted.
        /// 
        /// set must be sorted!
        /// </summary>
        private static bool TestRule2(int[] set)
        {
            // A is the smaller set, B the larger (i.e. with more elements)
            int upperBound = (set.Length % 2 == 0) ? set.Length / 2 - 1 : set.Length / 2;
            
            for (int setSizeA = 1; setSizeA <= upperBound; setSizeA++)
            {
                int biggestSetA = S(set.TakeLast(setSizeA));
                int smallestSetB = S(set.Take(setSizeA + 1));

                if (biggestSetA >= smallestSetB)
                    return false;
            }

            return true;
        }
    }
}
