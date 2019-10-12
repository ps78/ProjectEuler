using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;
using System.Numerics;
using System.Collections.Concurrent;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=121
    /// A bag contains one red disc and one blue disc.In a game of chance a player takes a disc at random and its colour is noted.
    /// After each turn the disc is returned to the bag, an extra red disc is added, and another disc is taken at random.
    /// The player pays £1 to play and wins if they have taken more blue discs than red discs at the end of the game.
    /// If the game is played for four turns, the probability of a player winning is exactly 11/120, and so the maximum prize 
    /// fund the banker should allocate for winning in this game would be £10 before they would expect to incur a loss. 
    /// Note that any payout will be a whole number of pounds and also includes the original £1 paid to play the game, 
    /// so in the example given the player actually wins £9.
    /// 
    /// Find the maximum prize fund that should be allocated to a single game in which fifteen turns are played.
    /// </summary>
    public class Problem121 : EulerProblemBase
    {
        public Problem121() : base(121, "Disc game prize fund", 0, 2269) { }

        public override long Solve(long n)
        {
            long maxTurns = 15;
            long fee = 1; // fee for playing
            long wins = CountWinningCases(maxTurns);
            long denominator = (maxTurns + 1).Factorial();

            // payout = floor(fee * denominator / wins)
            return (long)Math.Floor(fee * (double)denominator / wins);
        }
        

        private long CountWinningCases(long maxTurns, long redsInBag = 1, long redsDrawn = 0, long multiplier = 1)
        {
            if (redsInBag == maxTurns + 1)
                return 2 * redsDrawn < maxTurns ? multiplier : 0;
            else
                return
                    CountWinningCases(maxTurns, redsInBag + 1, redsDrawn + 1, multiplier * redsInBag) + /* draw red */
                    CountWinningCases(maxTurns, redsInBag + 1, redsDrawn, multiplier); /* draw blue */                
        }
    }
}
