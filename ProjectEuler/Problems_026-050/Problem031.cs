using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=31
    /// 
    /// In England the currency is made up of pound, £, and pence, p, and there are eight coins in general circulation:
    /// 
    /// 1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).
    /// It is possible to make £2 in the following way:
    /// 
    /// 1×£1 + 1×50p + 2×20p + 1×5p + 1×2p + 3×1p
    /// How many different ways can £2 be made using any number of coins?
    /// </summary>
    public class Problem031 : EulerProblemBase
    {
        public Problem031() : base(31, "Coin sums", 200, 73682) { }

        private int[] Coins = new int[] { 200, 100, 50, 20, 10, 5, 2, 1 };

        public override bool Test() => Solve(6) == 5;

        public override long Solve(long n)
        {
            Coins = (new[] { 200, 100, 50, 20, 10, 5, 2, 1 }).Where(c => c <= (int)n).ToArray();

            return CountVariants((int)n, 0, 0);
        }

        private int CountVariants(int targetSum, int currentSum, int coinIdx)
        {   
            if (coinIdx == Coins.Length - 1)
                return 1;
            else
            {
                var coinVal = Coins[coinIdx];
                int count = 0;
                for (int i = 0; i <= (targetSum - currentSum) / coinVal; i++)
                    count += CountVariants(targetSum, currentSum + i * coinVal, coinIdx + 1);
                return count;
            }
        }
    }
}
