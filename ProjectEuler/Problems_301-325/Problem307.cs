using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=307
    /// 
    /// k defects are randomly distributed amongst n integrated-circuit chips produced by a factory (any number of defects may be found on a chip and each defect is independent of the other defects).
    /// Let p(k,n) represent the probability that there is a chip with at least 3 defects.
    /// For instance p(3,7) ≈ 0.0204081633.
    /// Find p(20 000, 1 000 000) and give your answer rounded to 10 decimal places in the form 0.abcdefghij
    /// </summary>
    public class Problem307 : EulerProblemBase
    {
        public Problem307() : base(307, "Chip Defects", 1_000_000, 7311720251) { }

        public override bool Test()
        {
            return p(8, 6) == BruteForce(8, 6) && p(7, 3) == BruteForce(7, 3) && p(7, 4) == BruteForce(7, 4);
        }

        /// <summary>
        /// 
        /// p(k,n)  = N(k,n) / n^k
        /// 
        /// N(k,n)  = number of defect constellations with at leat one chip with 3 or more defects
        /// n^k     = total number of possible defect constellations
        /// 
        /// p(k,n)  = 1 - [N1(k,n) + N2(k,n)] / n^k
        /// N1(k,n) = Number of constellations where every chip has max 1 defect
        /// N2(k,n) = Number of constellations where no chip has more than 2 defects and at least one has exactly 2 defects
        ///             n!
        /// N1(k,n) = ──────
        ///           (n-k)!
        ///           
        /// N2(k,n) = Sum [N2j(k,n)] for j = 1 .. floor(k/2)
        /// 
        ///                       j-1               
        ///              n!      ─────  (k-2m)!     
        /// N2j(k,n)= ──────────  │ │  ──────────   
        ///           j!(n-k+j)!  │ │  2 (k-2m-2)!  
        ///                       m=0              
        /// 
        /// this yields the following formula:
        /// 
        ///                              k/2                    j-1               
        ///                 n!         ──────        n!        ─────  (k-2m)!     
        /// p(k,n) = 1 - ────────── -  >      ───────────────   │ │  ──────────   
        ///              n^k (n-k)!    ────── n^k j! (n-k+j)!   │ │  2 (k-2m-2)!  
        ///                              j=1                    m=0              
        /// 
        /// The first term after the minus sign can be transformed to Product(i/n) for i = n-k+1 .. n, which is nearly zero for n=10^6 / k=2*10^5 and can be ignored                
        /// </summary>
        /// <returns></returns>
        public override long Solve(long n)
        {                        
            long k = 20_000;

            double result = p(n, k);

            string s = result.ToString("0.0000000000");
            s = s.Substring(s.IndexOf('.') + 1);
            return long.Parse(s.Substring(0, Math.Min(10, s.Length)));
        }

        private double p(long n, long k, double abortThreshold = 1e-12)
        {
            var sums = new double[k / 2 + 1];
            double prevProd = 0.0;
            for (long j = 1; j <= k/ 2; j++)
            {
                var nums = GetNumerator(n, k, j);
                var dens = GetDenominator(n, k, j);
                double prod = 1.0; int numIdx = 0, denIdx = 0;
                while (true)
                {
                    bool outOfNums = (numIdx == nums.Length);
                    bool outOfDens = (denIdx == dens.Length);

                    if (!outOfDens && (prod > 1 || outOfNums))
                        prod /= dens[denIdx++];
                    else if (!outOfNums && (prod <= 1 || outOfDens))
                        prod *= nums[numIdx++];
                    else
                        break;
                }
                // the products start small and get bigger, then get samller again. Once we reach very small values, we can abort
                if (prod < prevProd && prod < abortThreshold) 
                    break;

                sums[j] += prod;
                prevProd = prod;
            }
            return Math.Round(1.0 - GetN1Term(n, k) - sums.Sum(), 10);
        }

        /// <summary>
        /// Numerator of the sum within p(k,n). Can be transformed to (product(i) for i=n-k+j+1 .. n) x (product(i) for i=k-2j+1 .. k)
        /// </summary>        
        private long[] GetNumerator(long n, long k, long j)
        {
            var result = new long[k + j]; int idx = 0;
            for (long i = n; i >= n - k + j + 1; i--)
                result[idx++] = i;
            for (long i = k; i >= k - 2 * j + 1; i--)
                result[idx++] = i;
            return result;
        }

        /// <summary>
        /// Denominator of the sum within p(k,n). Can be transformed to 2^j * n^k
        /// </summary>        
        private long[] GetDenominator(long n, long k, long j)
        {
            var result = new long[k + j]; int idx = 0;
            for (long i = 0; i < k; i++) // n^k
                result[idx++] = n;
            for (long i = j; i >= 1; i--) // 2^j * j!
                result[idx++] = 2 * i; 
            return result;
        }

        private double GetN1Term(long n, long k)
        {
            if (n > 100) // for big N, this term is virtually zero
                return 0;

            double result = 1.0;
            for (long i = n; i >= n- k + 1; i--)
                result *= ((double)i) / n;
            return result;
        }

        private double BruteForce(long n, long k)
        {
            var defect = new List<long[]>((int)Math.Pow(n, k));
            var cur = new long[k];
            bool stop = false;
            for (long i = 0; i < (long)Math.Pow(n, k); i++)
            {
                defect.Add(cur.ToArray());
                cur[k - 1]++;
                if (cur[k - 1] == n)
                {
                    for (long p = k - 1; p >= 0; p--)
                    {
                        if (cur[p] < n)
                            break;
                        if (p == 0)
                        {
                            stop = true;
                            break;
                        }
                        cur[p] = 0;
                        cur[p - 1]++;
                    }
                    if (stop) break;
                }
            }

            long count = 0;
            foreach (var def in defect)
            {
                foreach (int d in def.Distinct())
                {
                    var dc = def.Count(x => x == d);
                    if (dc >= 3)
                    {
                        count++;            
                        break;
                    }
                }
            }
            
            return Math.Round(((double)count) / n.Power(k), 10);
        }
        
    }
}
