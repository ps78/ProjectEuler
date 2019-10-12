using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=148
    /// 
    /// We can easily verify that none of the entries in the first seven rows of Pascal's triangle are divisible by 7:
    ///
    /// 	 	 	 	 	 	 1
    /// 	 	 	 	 	 1	 	 1
    /// 	 	 	  	 1	 	 2	 	 1
    /// 	 	 	 1	 	 3	 	 3	 	 1
    ///      	 1	 	 4	 	 6	 	 4	 	 1
    ///       1	 	 5	 	10	 	10	 	 5	 	 1
    ///   1	 	 6	 	15	 	20	 	15	 	 6	 	 1
    ///   
    ///  However, if we check the first one hundred rows, we will find that only 2361 of the 5050 entries are not divisible by 7.
    ///  Find the number of entries which are not divisible by 7 in the first one billion(10^9) rows of Pascal's triangle.
    /// </summary>
    public class Problem148 : EulerProblemBase
    {
        public Problem148() : base(148, "Exploring Pascal's triangle", 1_000_000_000, 2129970655314432) { }

        public override bool Test() => Solve(100) == 2361;

        public override long Solve(long n)
        {
            return RecursiveCount(n);
        }

        private long CountFullTriangle(uint level)
        {
            if (level == 0)
                return 28;
            else
                return 28 * CountFullTriangle(level - 1);            
        }

        private long RecursiveCount(long maxRowNumber)
        {
            uint level = (uint)Math.Floor(Math.Log(maxRowNumber) / Math.Log(7));

            if (level == 0)            
                return maxRowNumber * (maxRowNumber + 1) / 2;
            else 
            {
                // count the elements that are part of complete triangles
                long completeSubLayers = maxRowNumber / SevenPower(level);
                long count = completeSubLayers * (completeSubLayers + 1) / 2 * CountFullTriangle(level - 1);

                // all elements from nextRow on need yet to be counted
                long nextRow = completeSubLayers * SevenPower(level) + 1;

                long multiplier = nextRow / SevenPower(level) + 1;
                count += multiplier * RecursiveCount(maxRowNumber - nextRow + 1);
                
                return count;
            }            
        }

        /// <summary>
        /// computes 7^exp
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static long SevenPower(uint exp)
        {
            long result = 1;
            for (int i = 0; i < exp; i++)
                result *= 7;
            return result;
        }        
    }
}
