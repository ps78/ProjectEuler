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
    /// https://projecteuler.net/problem=68
    /// 
    /// Consider the following "magic" 3-gon ring, filled with the numbers 1 to 6, and each line adding to nine.
    /// Working clockwise, and starting from the group of three with the numerically lowest external node(4,3,2 in this example), 
    /// each solution can be described uniquely.For example, the above solution can be described by the set: 4,3,2; 6,2,1; 5,1,3.
    /// 
    /// It is possible to complete the ring with four different totals: 9, 10, 11, and 12. There are eight solutions in total.
    /// 
    /// Total Solution Set
    /// 9	4,2,3; 5,3,1; 6,1,2
    /// 9	4,3,2; 6,2,1; 5,1,3
    /// 10	2,3,5; 4,5,1; 6,1,3
    /// 10	2,5,3; 6,3,1; 4,1,5
    /// 11	1,4,6; 3,6,2; 5,2,4
    /// 11	1,6,4; 5,4,2; 3,2,6
    /// 12	1,5,6; 2,6,4; 3,4,5
    /// 12	1,6,5; 3,5,4; 2,4,6
    /// By concatenating each group it is possible to form 9-digit strings; the maximum string for a 3-gon ring is 432621513.
    /// 
    /// Using the numbers 1 to 10, and depending on arrangements, it is possible to form 16- and 17-digit strings.
    /// What is the maximum 16-digit string for a "magic" 5-gon ring?

    /// </summary>
    public class Problem068 : EulerProblemBase
    { 
        public Problem068() : base(68, "Magic 5-gon ring", 0, 6531031914842725) { }
        
        public override long Solve(long n)
        {
            // since strings must be 16 digits, 10 must be in the outer position. 

                        
            var initialState = new int[] { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var guessingPositions = new int[] { 1, 3, 5, 7 };
            var available = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<ulong> solutions = new List<ulong>();
            for (int targetSum = 14; targetSum <= 19; targetSum++)
            {
                foreach (var sol in FindSolutions(initialState, guessingPositions, available, targetSum).ToList())
                {
                    solutions.Add(Normalize(sol));
                    //Console.WriteLine("Sum = {0} / [ {1} ] / {2}", targetSum, sol.ToString<int>(), Normalize(sol));
                }                    
            }

            return (long)solutions.Max();
        }
        
        private IEnumerable<int[]> FindSolutions(int[] state, int[] guessPos, int[] available, int targetSum, int iteration = 0)
        {
            if ((iteration > 4) || (available.Length == 0))
                throw new InvalidOperationException();

            if ((iteration == 4) && (available.Length == 1))
            {
                int diff = targetSum - state[1] - state[8];
                if (diff == available[0])
                {
                    state[state.Length - 1] = available[0];
                    yield return new List<int>(state).ToArray();
                    state[state.Length - 1] = 0;
                }
            }
            else
            {                
                for (int i = 0; i < available.Length; i++)
                {
                    int b = available[i];
                    int diff = 0;
                    int diffPos = -1;
                    switch(iteration)
                    {
                        case 0:
                            diff = targetSum - state[0] - b;
                            diffPos = 2;
                            break;
                        case 1:
                            diff = targetSum - state[2] - b;
                            diffPos = 4;
                            break;
                        case 2:
                            diff = targetSum - state[4] - b;
                            diffPos = 6;
                            break;
                        case 3:
                            diff = targetSum - state[6] - b;
                            diffPos = 8;
                            break;
                    }

                    if ((diff != b) && available.Contains(diff))
                    {                                                
                        state[guessPos[iteration]] = b;
                        state[diffPos] = diff;
                        foreach (var sol in FindSolutions(state, guessPos, available.Where((el) => (el != b) && (el != diff)).ToArray(), targetSum, iteration + 1))
                            yield return sol;                        
                        state[guessPos[iteration]] = 0;
                        state[diffPos] = 0;
                    }
                }
            }

        }

        private ulong Normalize(int[] x)
        {
            var vec = new int[15] { x[0], x[1], x[2], x[3], x[2], x[4], x[5], x[4], x[6], x[7], x[6], x[8], x[9], x[8], x[1] };

            int min = int.MaxValue;
            int minIdx = -1;
            for (int i = 0; i < 15; i+=3)
            {
                if (vec[i] < min)
                {
                    min = vec[i];
                    minIdx = i;
                }
            }

            var v2 = new int[15];
            for (int i = 0; i < 15; i++)
                v2[i] = vec[(minIdx + i) % 15];

            return ulong.Parse(v2.ToString<int>(""));
        }

    }
}
