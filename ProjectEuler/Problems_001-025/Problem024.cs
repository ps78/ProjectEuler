using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=24
    /// 
    /// A permutation is an ordered arrangement of objects.For example, 3124 is one possible permutation 
    /// of the digits 1, 2, 3 and 4. If all of the permutations are listed numerically or alphabetically, 
    /// we call it lexicographic order.The lexicographic permutations of 0, 1 and 2 are:
    /// 012   021   102   120   201   210
    /// What is the millionth lexicographic permutation of the digits 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9?
    /// </summary>
    public class Problem024 : EulerProblemBase
    {
        public Problem024() : base(24, "Lexicographic permutations", 1_000_000, 2783915460) { }

        public override bool Test()
        {
            var tmp = digits;
            digits = new byte[]{ 0, 1, 2 };
            bool result = Solve(6) == 210;
            digits = tmp;
            return result;
        }

        private byte[] digits = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public override long Solve(long n)
        {
            /*
            var result = new byte[10];
            int n = (int)ProblemSize;
            var faculty = new int[10] { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };

            // there are a total of 10! = 3'628'800 permutations

            // the first digit is increasing by one every 9! = 362'880 permutations            
            for (int i = 0; i < 10; i++)
            {
                result[i] = (byte)((n / faculty[9 - i] + 1) % 10);
                n %= faculty[9 - i];
            }

            return ulong.Parse(result.Select((b) => b.ToString()).Aggregate((str, chr) => str += chr));
            */
            int counter = 0;
            var vector = new byte[10];
            permute(ref vector, new List<byte>(digits), ref counter, (int)n);
            return long.Parse(vector.Select((b) => b.ToString()).Aggregate((str, chr) => str += chr));
        }

        private void permute(ref byte[] vector, List<byte> digits, ref int curCounter, int stopCounter)
        {
            if (digits.Count == 0)
            {
                curCounter++;                
                return;                
            }            

            int nextPos = vector.Length - digits.Count;
            for (byte i = 0; i < digits.Count; i++)
            {                
                vector[nextPos] = digits[i];
                var remainingDigits = new List<byte>(digits);
                remainingDigits.RemoveAt(i);
                permute(ref vector, remainingDigits, ref curCounter, stopCounter);
                if (curCounter == stopCounter)
                    return;
            }
        }
    }
}
