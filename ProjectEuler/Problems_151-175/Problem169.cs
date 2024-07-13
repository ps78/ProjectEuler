using NumberTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=169
    /// 
    /// Define f(0)=1 and f(n) to be the number of different ways n can be expressed as a 
    /// sum of integer powers of 2 using each power no more than twice.
    /// 
    /// For example, f(10)=5 since there are five different ways to express 10:
    ///  2 + 8
    ///  2 + 4 + 4
    ///  1 + 1 + 2 + 2 + 4
    ///  1 + 1 + 4 + 4
    ///  1 + 1 + 8
    ///  
    /// What is f(10^25)?
    /// 
    /// </summary>
    public class Problem169 : EulerProblemBase
    {
        public Problem169() : base(169, "Sums of Powers of Two", 25, 178653872807) { }
        
        public override bool Test() => Solve(1) == 5;
       
        /// <summary>
        /// n means 10^n
        /// </summary>
        public override long Solve(long n)
        {
            // create binary representation of n and get the positions of '1'
            // where position 0 corresponds to the least significant one (2^0)
            string binary = BigInteger.Pow(10, (int)n).ToBase(2);
            var powersOf2 = binary.Reverse().IndicesOf("1").ToArray();

            // gaps are the number of positions from one '1' index in powersOf2 to the next 
            var gaps = new int[powersOf2.Length];
            for (int i = 0; i < powersOf2.Length-1; i++)
                gaps[i] = powersOf2[i+1] - powersOf2[i];

            // the actual algorithm, initialize with first 'block'
            long blockSum2StepBack = powersOf2[0];
            long blockSum1StepBack = powersOf2[0] + 1;
            long finishedBlockSum = 0;
            for (int idx = 0; idx < powersOf2.Length-1; idx++)
            {
                finishedBlockSum = (idx == 0 ? 1 : gaps[idx - 1] - 1) * blockSum2StepBack + finishedBlockSum;
                long newBlockSum = gaps[idx] * blockSum1StepBack + finishedBlockSum;
                blockSum2StepBack = blockSum1StepBack;
                blockSum1StepBack = newBlockSum;
            }

            return blockSum1StepBack;
        }
        
    }
}
