﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=8
    /// 
    /// The four adjacent digits in the 1000-digit number that have the greatest product are 9 × 9 × 8 × 9 = 5832.
    /// {large number here}
    /// Find the thirteen adjacent digits in the 1000-digit number that have the greatest product.What is the value of this product?
    /// </summary>
    public class Problem008 : EulerProblemBase
    {
        public Problem008() : base(8, "Largest product in a series", 13, 23514624000) { }

        public override bool Test() => Solve(4) == 5832;

        #region data
        private string largeNumber =
            "7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843" +
            "8586156078911294949545950173795833195285320880551112540698747158523863050715693290963295227443043557" +
            "6689664895044524452316173185640309871112172238311362229893423380308135336276614282806444486645238749" +
            "3035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776" +
            "6572733300105336788122023542180975125454059475224352584907711670556013604839586446706324415722155397" +
            "5369781797784617406495514929086256932197846862248283972241375657056057490261407972968652414535100474" +
            "8216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586" +
            "1786645835912456652947654568284891288314260769004224219022671055626321111109370544217506941658960408" +
            "0719840385096245544436298123098787992724428490918884580156166097919133875499200524063689912560717606" +
            "0588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";
        #endregion

        public override long Solve(long n)
        {
            long largestProduct = 0;

            int len = largeNumber.Length;
            int pos = 0;
            
            while (pos <= len - (int)n)
            {
                // get the next sub-number
                string sub = largeNumber.Substring(pos, (int)n);

                // if it contains 0 it is certainly not the larges product, move on
                int zeroPos = sub.IndexOf('0');
                if (zeroPos != -1)
                    pos += (zeroPos + 1);
                else
                {
                    long product = sub.ToArray()
                        .Select((c) => long.Parse(c.ToString()))
                        .Aggregate(1L, (total, next) => total *= next);

                    if (product > largestProduct)
                        largestProduct = product;

                    pos++;
                }
            }

            return largestProduct;
        }
    }
}
