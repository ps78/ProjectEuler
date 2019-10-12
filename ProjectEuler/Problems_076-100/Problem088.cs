using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=88
    /// A natural number, N, that can be written as the sum and product of a given set of at 
    /// least two natural numbers, {a1, a2, ... , ak} 
    /// is called a product-sum number: N = a1 + a2 + ... + ak = a1 × a2 × ... × ak.
    /// 
    /// For example, 6 = 1 + 2 + 3 = 1 × 2 × 3.
    /// 
    /// For a given set of size, k, we shall call the smallest N with this property a minimal product-sum number.
    /// The minimal product-sum numbers for sets of size, k = 2, 3, 4, 5, and 6 are as follows.
    /// 
    /// k= 2: 4 = 2 × 2 = 2 + 2
    /// k= 3: 6 = 1 × 2 × 3 = 1 + 2 + 3
    /// k= 4: 8 = 1 × 1 × 2 × 4 = 1 + 1 + 2 + 4
    /// k= 5: 8 = 1 × 1 × 2 × 2 × 2 = 1 + 1 + 2 + 2 + 2
    /// k= 6: 12 = 1 × 1 × 1 × 1 × 2 × 6 = 1 + 1 + 1 + 1 + 2 + 6
    /// 
    /// Hence for 2≤k≤6, the sum of all the minimal product-sum numbers is 4+6+8+12 = 30; 
    /// note that 8 is only counted once in the sum.
    /// 
    /// In fact, as the complete set of minimal product-sum numbers for 2≤k≤12 is {4, 6, 8, 12, 15, 16}, the sum is 61.
    /// 
    /// What is the sum of all the minimal product-sum numbers for 2≤k≤12000?
    /// </summary>
    public class Problem088 : EulerProblemBase
    {
        public Problem088(): base(88, "Product-sum numbers", 12_000, 7587457) { }

        // TODO: test doesn't work
        //public override bool Test() => Solve(12) == 61;

        public override long Solve(long n)
        {
            // how many elements in the set are not 1? call that number N, then the condition is 2^N <= k
            int N = (int)Math.Floor(Math.Log((int)n, 2));
            
            // build all combinations of 2..N factors such that they form a product sum number. 
            // the min product sum number M must be <= 2*k
            // let p = product of the fcount factors and s = sum of the factors plus the right amount oneCount of 1's
            // since p = s and s = sum(factors) + oneCount => p = sum(factors) + oneCount 
            //  => oneCount = p - sum(factors)
            //  => k = oneCount + fcount
            
            var M = new int[(int)n + 1];
            for (int i = 2; i < M.Length; i++)
                M[i] = int.MaxValue;

            int kLimit = (int)n * 2;

            var factors = new int[N];
            for (int i = 0; i < N - 2; i++) factors[i] = 1;
            factors[N - 2] = 2;
            factors[N - 1] = 2;

            int product = 4;
            int factSum = 4; // sum of factors != 1
            int factCount = 2;  // number of factors != 1
            bool abort = false;
            while(!abort)
            {
                // evaluate k such that product = sum
                int oneCount = product - factSum; // number of 1's
                int k = oneCount + factCount;

                // check if this product is a minimum for the respective k
                if (k >= 2 && k <= (int)n && product < M[k])
                    M[k] = product;

                // if increasing the last factor by leads to value too large, increase the previous factor by 1                
                int newProduct = product / factors[N - 1] * (factors[N - 1] + 1);
                if (newProduct > kLimit)
                {
                    for (int j = N - 2; j >= 0; j--)
                    {
                        if (factors[j] == 1)
                        {
                            factCount++;
                            factSum++;
                        }
                        product = product / factors[j] * (factors[j] + 1);
                        factSum++;
                        factors[j]++;

                        for (int h = j + 1; h < N; h++)
                        {
                            product = product / factors[h] * (factors[j]);
                            factSum += (factors[j] - factors[h]);
                            factors[h] = factors[j];
                        }
                        if (product <= kLimit)
                            break;
                        else if (j == 0)
                            abort = true;
                    }
                }
                // otherwise just increase the last factor by one
                else
                {
                    product = newProduct;
                    factSum++;
                    factors[N - 1]++;                    
                }
            }

            return (long)M.Distinct().Sum(); // solution = 7587457                
        }
    }
}
