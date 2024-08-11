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

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=106
    /// 
    /// Let S(A) represent the sum of elements in set A of size n. 
    /// We shall call it a special sum set if for any two non-empty disjoint subsets, B and C, 
    /// the following properties are true:
    /// 
    ///     1. S(B) != S(C); that is, sums of subsets cannot be equal.
    ///     2. If B contains more elements than C then S(B) > S(C)
    ///     
    /// For this problem we shall assume that a given set contains n 
    /// strictly increasing elements and it already satisfies the second rule.
    /// 
    /// Surprisingly, out of the 25 possible subset pairs that can be obtained from a set 
    /// for which n = 4, only 1 of these pairs need to be tested for equality (first rule). 
    /// Similarly, when n = 7, only 70 out of the 966 subset pairs need to be tested.
    /// 
    /// For n = 12, how many of the 261625 subset pairs that can be obtained need to be tested for equality?
    /// 
    /// NOTE: This problem is related to Problem 103 and Problem 105.
    /// </summary>
    public class Problem106 : EulerProblemBase
    {
        public Problem106() : base(106, "Special Subset Sums: Meta-testing", 12, 21384) { }

        public override bool Test() => Solve(4) == 1 && Solve(7) == 70;

        public override long Solve(long n)
        {
            var set = Enumerable.Range(0, (int)n).ToArray();

            // we only need to test set pairs (x, x) of size x for x=2..n/2
            int[] setSizesToTest = Enumerable.Range(2, (int)n / 2 - 1).ToArray();

            int testCount = 0;
            foreach (int setSize in setSizesToTest)
            {
                var sets = Combination.Create(set, setSize).ToArray();
                for (int set1Idx = 0; set1Idx < sets.Length; set1Idx++)
                {
                    var set1 = sets[set1Idx];
                    for (int set2Idx = set1Idx + 1; set2Idx < sets.Length; set2Idx++)
                    {
                        var set2 = sets[set2Idx];
                        if (AreDisjoint(set1, set2) && NeedTest(set1, set2))
                            testCount++;
                    }
                }
            }

            return testCount;
        }

        private bool AreDisjoint(IList<int> set1, IList<int> set2)
        {
            var s = new HashSet<int>(set1);
            foreach (int i in set2)
                if (s.Contains(i))
                    return false;
            return true;
        }

        private bool NeedTest(IList<int> set1, IList<int> set2)
        {
            for (int i = 0; i < set1.Count; i++)
                if (set1[i] > set2[i])
                    return true;
            return false;
        }
    }
}
