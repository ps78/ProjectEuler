using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SudokuGame;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=092
    /// A number chain is created by continuously adding the square of the digits in a number to form a new number until it has been seen before.
    /// 
    /// For example,
    /// 
    /// 44 → 32 → 13 → 10 → 1 → 1
    /// 85 → 89 → 145 → 42 → 20 → 4 → 16 → 37 → 58 → 89
    /// 
    /// Therefore any chain that arrives at 1 or 89 will become stuck in an endless loop.What is most amazing is 
    /// that EVERY starting number will eventually arrive at 1 or 89.
    /// 
    /// How many starting numbers below ten million will arrive at 89?
    /// </summary>
    public class Problem092 : EulerProblemBase
    {
        public Problem092() : base(92, "Square digit chains", 10_000_000, 8581146) { }

        private bool[] endsInEightyNine = new bool[568];
        private bool[] endsInOne = new bool[568];

        public override long Solve(long n)
        {            
            int counter = 0;
            endsInOne[1] = true;
            endsInEightyNine[89] = true;

            // max number after first square and sum is 7*9^2 for 9'999'999 which is 567
            // i.e. we can precompute the end values for all numbers from 1 to 567
            for (int i = 1; i <= 567; i++)
                if (EndsIn89(i))
                    counter++;

            for (int i = 568; i < 10_000_000; i++)
                if (EndsIn89Fast(i))
                    counter++;
            
            return counter;
        }

        // does all iterations
        public bool EndsIn89(int n)
        {
            int i = n;
            bool result = true;
            while(true)
            {
                if (i <= 567)
                {
                    if (endsInEightyNine[i])
                        break;
                    else if (endsInOne[i])
                    {
                        result = false;
                        break;
                    }
                }

                int sum = 0;
                while (i > 0)
                {
                    sum += (i % 10) * (i % 10);
                    i = i / 10;
                }
                i = sum;

                if (i == 89)
                    break;
                else if (i == 1)
                {
                    result = false;
                    break;
                }
            }

            if (n <= 567)
            {
                if (result)
                    endsInEightyNine[n] = true;
                else
                    endsInOne[n] = true;
            }
            return result;
        }

        // only does one iteration, works for n > 567 if the largest n can be 1e7
        public bool EndsIn89Fast(int n)
        {
            int sum = 0;            
            while (n > 0)
            {
                sum +=  (n % 10) * (n % 10);
                n = n / 10;
            }
            
            if (endsInEightyNine[sum])
                return true;
            else if (endsInOne[sum])
                return false;
            else
                throw new InvalidOperationException();
        }
    }
}
