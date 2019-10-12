using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;

namespace ProjectEuler
{
    using Triple = Tuple<int, int, int>;

    /// <summary>
    /// https://projecteuler.net/problem=54
    /// The file, poker.txt, contains one-thousand random hands dealt to two players. 
    /// Each line of the file contains ten cards (separated by a single space): 
    /// the first five are Player 1's cards and the last five are Player 2's cards. 
    /// You can assume that all hands are valid (no invalid characters or repeated cards), 
    /// each player's hand is in no specific order, and in each hand there is a clear winner.
    /// How many hands does Player 1 win?
    /// </summary>
    public class Problem054 : EulerProblemBase
    {
        public Problem054() : base(54, "Poker hands", 0, 376) { }

        public override long Solve(long n)
        {
            var games = new List<Tuple<PokerHand, PokerHand>>();

            foreach (string line in File.ReadAllLines(Path.Combine(ResourcePath, "Problem054.txt")))
                games.Add(new Tuple<PokerHand, PokerHand>(new PokerHand(line.Substring(0, 14)), new PokerHand(line.Substring(14))));

            return games.Count(g => g.Item1.CompareTo(g.Item2) > 0);
        }

        public enum CardValue : byte
        {
            One = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
        }
        public enum CardSuit : byte
        {
            Clubs, Diamont, Hearts, Spades
        }
        public enum PokerHands : byte
        {
            HighCard,
            OnePair,
            TwoPairs,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }

        public class Card : IComparable<Card>
        {
            public CardSuit Suit { get; private set; } // (C)lubs, (D)iamont, (H)earts, (S)pades
            public CardValue Value { get; private set; } // 1-9, (T)en, (J)oker, (Q)ueen, (K)ing, (A)ce
            public Card(Card c) : this(c.Value, c.Suit) { }
            public Card(CardValue value, CardSuit suit)
            {
                this.Value = value;
                this.Suit = suit;
            }
            public Card(string c)
            {
                if (c.Length != 2)
                    throw new ArgumentException("Invalid card string: " + c);
                switch (c[0])
                {
                    case '1': Value = CardValue.One; break;
                    case '2': Value = CardValue.Two; break;
                    case '3': Value = CardValue.Three; break;
                    case '4': Value = CardValue.Four; break;
                    case '5': Value = CardValue.Five; break;
                    case '6': Value = CardValue.Six; break;
                    case '7': Value = CardValue.Seven; break;
                    case '8': Value = CardValue.Eight; break;
                    case '9': Value = CardValue.Nine; break;
                    case 'T': Value = CardValue.Ten; break;
                    case 'J': Value = CardValue.Jack; break;
                    case 'Q': Value = CardValue.Queen; break;
                    case 'K': Value = CardValue.King; break;
                    case 'A': Value = CardValue.Ace; break;
                    default: throw new ArgumentException("Invalid card value: " + c[0]);
                }
                switch(c[1])
                {
                    case 'C': Suit = CardSuit.Clubs; break;
                    case 'D': Suit = CardSuit.Diamont; break;
                    case 'H': Suit = CardSuit.Hearts; break;
                    case 'S': Suit = CardSuit.Spades; break;
                    default: throw new ArgumentException("Invalid suit value: " + c[1]);
                }
            }

            public int CompareTo(Card other)
            {
                return Value.CompareTo(other.Value);
            }
            public override string ToString()
            {
                var sb = new StringBuilder();
                char val = ((int)Value > 9 ? Value.ToString()[0] : ((int)Value).ToString()[0]);
                sb.Append(val);
                sb.Append(Suit.ToString()[0]);
                return sb.ToString();
            }
        }

        public class PokerHand : IComparable<PokerHand>
        {
            private Card[] cards = new Card[5];

            private CardValue[] HighestValues = null;

            public PokerHands Hand { get; private set; }
            
            public PokerHand(string fiveCards)
            {
                var crds = fiveCards.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (crds.Length != 5)
                    throw new ArgumentException("Invalid hands initializer, expected 5 cards: " + fiveCards);

                for (int i = 0; i < 5; i++)
                    cards[i] = new Card(crds[i]);

                DetectHand();
            }

            private void DetectHand()
            {
                var valueBin = new Dictionary<CardValue, int>();
                var suitBin = new Dictionary<CardSuit, int>();
                foreach (var c in cards)
                {
                    valueBin[c.Value] = valueBin.ContainsKey(c.Value) ? valueBin[c.Value] + 1 : 1;
                    suitBin[c.Suit] = suitBin.ContainsKey(c.Suit) ? suitBin[c.Suit] + 1 : 1;
                }

                CardValue maxVal = valueBin.Keys.Max();
                CardValue minVal = valueBin.Keys.Min();
                bool isFlush = suitBin.Values.Contains(5);
                bool isStraight = (valueBin.Values.Max() == 1) && ((byte)maxVal - (byte)minVal == 4);

                if (isFlush && isStraight && (maxVal == CardValue.Ace))
                {
                    Hand = PokerHands.RoyalFlush;
                    HighestValues = new CardValue[] { };
                }
                else if (isFlush && isStraight)
                {
                    Hand = PokerHands.StraightFlush;
                    HighestValues = new CardValue[] { maxVal };
                }
                else if (valueBin.Values.Max() == 4)
                {
                    Hand = PokerHands.FourOfAKind;
                    HighestValues = valueBin.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToArray();
                }
                else if ((valueBin.Values.Max() == 3) && (valueBin.Values.Min() == 2))
                {
                    Hand = PokerHands.FullHouse;
                    HighestValues = valueBin.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToArray();
                }
                else if (isFlush)
                {
                    Hand = PokerHands.Flush;
                    HighestValues = valueBin.Select(kv => kv.Key).OrderBy(v => -(int)v).ToArray();
                }
                else if (isStraight)
                {
                    Hand = PokerHands.Straight;
                    HighestValues = new CardValue[] { maxVal };
                }
                else if (valueBin.Values.Max() == 3)
                {
                    Hand = PokerHands.ThreeOfAKind;
                    HighestValues = valueBin.OrderBy(kv => -100 * kv.Value - (int)kv.Key).Select(kv => kv.Key).ToArray();
                }
                else if (valueBin.Values.Count(v => v == 2) == 2)
                {
                    Hand = PokerHands.TwoPairs;
                    HighestValues = valueBin.OrderBy(kv => -100 * kv.Value - (int)kv.Key).Select(kv => kv.Key).ToArray();
                }
                else if (valueBin.Values.Max() == 2)
                {
                    Hand = PokerHands.OnePair;
                    HighestValues = valueBin.OrderBy(kv => -100*kv.Value-(int)kv.Key).Select(kv => kv.Key).ToArray();
                }
                else
                {
                    Hand = PokerHands.HighCard;
                    HighestValues = valueBin.Select(kv => kv.Key).OrderBy(v => -(int)v).ToArray();
                }
            }

            public int CompareTo(PokerHand other)
            {
                if (this.Hand == other.Hand)
                {
                    for (int i = 0; i < HighestValues.Length; i++)
                        if (this.HighestValues[i] != other.HighestValues[i])
                            return this.HighestValues[i].CompareTo(other.HighestValues[i]);
                    return 0;
                }
                else
                    return this.Hand.CompareTo(other.Hand);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Hand.ToString() + ": ");
                foreach (var card in cards)
                    sb.Append(card.ToString() + " ");
                return sb.ToString();
            }
        }              
    }
}
