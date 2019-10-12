using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=76
    /// It is possible to write five as a sum in exactly six different ways:
    /// 
    /// 4 + 1
    /// 3 + 2
    /// 3 + 1 + 1
    /// 2 + 2 + 1
    /// 2 + 1 + 1 + 1
    /// 1 + 1 + 1 + 1 + 1
    /// 
    /// How many different ways can one hundred be written as a sum of at least two positive integers?
    /// </summary>
    public class Problem076 : EulerProblemBase
    {
        public Problem076() : base(76, "Counting summations", 100, 190569291) { }

        public override bool Test() => Solve(5) == 6;

        public override long Solve(long n)
        {
            return (long)PartitionMath.Compute((ulong)n) - 1;
        }        
    }
}
