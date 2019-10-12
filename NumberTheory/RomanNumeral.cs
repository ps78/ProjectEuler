using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class RomanNumeral
    {
        #region Private Fields

        private int value = 0;
        private string numeral = string.Empty;
        private static readonly Dictionary<char, int> NumeralValues = new Dictionary<char, int>
        {
            {'I', 1 },
            {'V', 5 },
            {'X', 10 },
            {'L', 50 },
            {'C', 100 },
            {'D', 500 },
            {'M', 1000 }
        };

        public int Value
        {
            get => this.value;
            set
            {
                this.value = value;
                this.numeral = IntToNumeral(value);
            }
        }

        public string Numeral
        {
            get => this.numeral;
            set
            {
                this.value = NumeralToInt(value);
                this.numeral = IntToNumeral(this.value);
            }
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Constructor, creates the value 0
        /// </summary>
        public RomanNumeral() { }

        /// <summary>
        /// Constructor to initialize with a string-numeral
        /// </summary>
        /// <param name="s"></param>
        public RomanNumeral(string s) => Numeral = s;

        /// <summary>
        /// Constructor to initialize with an integer
        /// </summary>
        /// <param name="i"></param>
        public RomanNumeral(int i) => Value = i;

        public override string ToString() => this.Numeral;

        #endregion
        #region Private Methods

        /// <summary>
        /// Converts a given integer value to a roman numeral
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string IntToNumeral(int value)
        {
            string total = "";

            int rest = value;

            while (rest >= 1000) { total += "M"; rest -= 1000; }
            if (rest >= 900) { total += "XM"; rest -= 900; }
            while (rest >= 500) { total += "D"; rest -= 500; }
            if (rest >= 400) { total += "CD"; rest -= 400; }
            while (rest >= 100) { total += "C"; rest -= 100; }
            if (rest >= 90) { total += "XC"; rest -= 90; }
            while (rest >= 50) { total += "L"; rest -= 50; }
            if (rest >= 40) { total += "XL"; rest -= 40; }
            while (rest >= 10) { total += "X"; rest -= 10; }
            if (rest >= 9) { total += "IX"; rest -= 9; }
            while (rest >= 5) { total += "V"; rest -= 5; }
            if (rest >= 4) { total += "IV"; rest -= 4; }
            while (rest >= 1) { total += "I"; rest -= 1; }

            return total;
        }

        /// <summary>
        /// Converts a given roman numeral to an integer representation
        /// </summary>
        /// <param name="numeral"></param>
        /// <returns></returns>
        private static int NumeralToInt(string numeral)
        {
            try
            {
                int total = 0;
                string s = numeral.ToUpper();
                int i = 0;
                while (i < s.Length)
                {
                    int curVal = NumeralValues[s[i]];
                    int nextVal = (i >= s.Length - 1 ? 0 : NumeralValues[s[i + 1]]);

                    if (nextVal <= curVal)
                    {
                        total += curVal;
                        i++;
                    }
                    else
                    {
                        total += (nextVal - curVal);
                        i += 2;
                    }
                }
                return total;
            }
            catch (Exception)
            {
                throw new InvalidDataException("The given numeral is not valid");
            }
        }

        #endregion
    }
}
