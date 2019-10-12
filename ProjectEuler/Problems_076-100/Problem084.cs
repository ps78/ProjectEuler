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
    /// https://projecteuler.net/problem=84
    /// A player starts on the GO square and adds the scores on two 6-sided dice to determine the number of 
    /// squares they advance in a clockwise direction.Without any further rules we would expect to visit each 
    /// square with equal probability: 2.5%. However, landing on G2J(Go To Jail), CC(community chest), 
    /// and CH(chance) changes this distribution.
    /// 
    /// In addition to G2J, and one card from each of CC and CH, that orders the player to go directly to jail, 
    /// if a player rolls three consecutive doubles, they do not advance the result of their 3rd roll. 
    /// Instead they proceed directly to jail.
    /// 
    /// At the beginning of the game, the CC and CH cards are shuffled.When a player lands on CC or CH they 
    /// take a card from the top of the respective pile and, after following the instructions, it is returned 
    /// to the bottom of the pile.There are sixteen cards in each pile, but for the purpose of this problem we 
    /// are only concerned with cards that order a movement; any instruction not concerned with movement will 
    /// be ignored and the player will remain on the CC/CH square.
    /// 
    /// Community Chest (2/16 cards):
    /// Advance to GO
    /// Go to JAIL
    /// Chance (10/16 cards):
    /// Advance to GO
    /// Go to JAIL
    /// Go to C1
    /// Go to E3
    /// Go to H2
    /// Go to R1
    /// Go to next R (railway company)
    /// Go to next R
    /// Go to next U (utility company)
    /// Go back 3 squares.
    /// The heart of this problem concerns the likelihood of visiting a particular square. That is, the probability 
    /// of finishing at that square after a roll. For this reason it should be clear that, with the exception 
    /// of G2J for which the probability of finishing on it is zero, the CH squares will have the lowest 
    /// probabilities, as 5/8 request a movement to another square, and it is the final square that the player 
    /// finishes at on each roll that we are interested in. We shall make no distinction between "Just Visiting" 
    /// and being sent to JAIL, and we shall also ignore the rule about requiring a double to "get out of jail", 
    /// assuming that they pay to get out on their next turn.
    /// 
    /// By starting at GO and numbering the squares sequentially from 00 to 39 we can concatenate these two - digit
    /// numbers to produce strings that correspond with sets of squares.
    /// 
    /// Statistically it can be shown that the three most popular squares, in order, are JAIL(6.24 %) = Square 10, 
    /// E3(3.18 %) = Square 24, and GO(3.09 %) = Square 00.So these three most popular squares can be listed 
    /// with the six - digit modal string: 102400.
    /// 
    /// If, instead of using two 6 - sided dice, two 4 - sided dice are used, find the six - digit modal string.
    /// </summary>
    public class Problem084 : EulerProblemBase
    {        
        #region Fields

        private Random rand = new Random();
        private List<CardActionDelegate> CommunityChest = new List<CardActionDelegate>();
        private List<CardActionDelegate> Chance = new List<CardActionDelegate>();

        #endregion

        private delegate int CardActionDelegate(int currentPosition);

        #region Public Methods

        public Problem084() : base(84, "Monopoly odds", 0, 101524) { }

        public override long Solve(long n)
        {
            var board = new long[40];
            CommunityChest.AddRange(new CardActionDelegate[16] { AdvanceToGo, GoToJail, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            Chance.AddRange(new CardActionDelegate[16] { AdvanceToGo, GoToJail, GoToC1, GoToE3, GoToH2, GoToR1, GoToNextRailway, GoToNextRailway, GoToNextUtility, GoBack3, null, null, null, null, null, null });

            CommunityChest.Shuffle();
            Chance.Shuffle();

            // run montecarlo simulation
            long simulationCount = 1000000;
            int diceSides = 4;
            int curPos = 0;
            int curCC = 0; // index of next community chest card
            int curCH = 0; // index of next chance card
            for (long sims = 0; sims < simulationCount; sims++)
            {
                // roll the dice
                int dice = RollTwoDice(out bool isDouble, diceSides);
                curPos = (curPos + dice) % board.Length;
                if (curPos == 30)
                    curPos = GoToJail(curPos);
                else
                {
                    // roll dice again if it was double
                    if (isDouble)
                    {
                        dice = RollTwoDice(out isDouble, diceSides);
                        curPos = (curPos + dice) % board.Length;
                    }
                    if (curPos == 30)
                        curPos = GoToJail(curPos);
                    else
                    {
                        // and a third time
                        if (isDouble)
                        {
                            dice = RollTwoDice(out isDouble, diceSides);
                            if (isDouble)
                                curPos = GoToJail(curPos); // goto jail if rolled 3 doubles
                            else
                                curPos = (curPos + dice) % board.Length;
                        }
                    }
                }

                // community chest?
                if (curPos == 2 || curPos == 17 || curPos == 33)
                {
                    var ccCard = CommunityChest[curCC];
                    if (ccCard != null)
                        curPos = ccCard(curPos);
                    curCC = (curCC + 1) % CommunityChest.Count;
                }

                // chance?
                if (curPos == 7 || curPos == 22 || curPos == 36)
                {
                    var chCard = Chance[curCH];
                    if (chCard != null)
                        curPos = chCard(curPos);
                    curCH = (curCH + 1) % Chance.Count;
                }

                board[curPos]++;
            }

            //for (int i = 0; i < 40; i++)
                //Console.WriteLine("{0,2:N0} : {1:N3}%", i, (double)100*board[i] / simulationCount);

            //Console.WriteLine("Jail  : {0:N3}%", (double)100 * board[10] / simulationCount);
            //Console.WriteLine("E3    : {0:N3}%", (double)100 * board[24] / simulationCount);
            //Console.WriteLine("Start : {0:N3}%", (double)100 * board[0] / simulationCount);

            var probabilities = new List<Tuple<int, double>>();
            for (int i = 0; i < board.Length; i++)
                probabilities.Add(new Tuple<int, double>(i, 100.0 * board[i] / simulationCount));

            probabilities.Sort((x, y) => -x.Item2.CompareTo(y.Item2));

            //foreach (var p in probabilities)
            //    Console.WriteLine("{0,2:N0} : {1:N3}%", p.Item1, p.Item2);

            return (probabilities[0].Item1 * 10000 + probabilities[1].Item1 * 100 + probabilities[2].Item1);
        }

        #endregion
        #region Private Method

        private int RollTwoDice(out bool IsDouble, int Sides = 4)
        {
            int dice1 = rand.Next(1, Sides + 1);
            int dice2 = rand.Next(1, Sides + 1);
            IsDouble = (dice1 == dice2);
            return dice1 + dice2;
        }

        private int GoToJail(int currentPos) => 10;
        private int AdvanceToGo(int currentPos) => 0;
        private int GoToC1(int currentPos) => 11;
        private int GoToE3(int currentPos) => 24;
        private int GoToH2(int currentPos) => 39;
        private int GoToR1(int currentPos) => 5;
        private int GoBack3(int currentPos) => currentPos >= 3 ? currentPos - 3 : currentPos + 37;
        private int GoToNextUtility(int currentPos) => currentPos >= 12 && currentPos < 28 ? 28 : 12;
        private int GoToNextRailway(int currentPos)
        {
            if (currentPos >= 5 && currentPos < 15) return 15;
            if (currentPos >= 15 && currentPos < 25) return 25;
            if (currentPos >= 25 && currentPos < 35) return 35;
            return 5;
        }
        

        #endregion
    }
}
