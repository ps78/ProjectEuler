using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    /// <summary>
    /// Class representing a Sudoku character-set
    /// This also implements a mapping from characters to byte indexes used by the Sudoku
    /// </summary>
    public sealed class SudokuCharacters
    {
        #region Data fields

        private readonly List<char> characters;
        private string characterString;
        private char emptyCharacter;

        private Random rand = new Random();

        #endregion
        #region Default Character sets

        public static SudokuCharacters Sudoku4Set { get { return new SudokuCharacters("1234", '.'); } }
        public static SudokuCharacters Sudoku6Set { get { return new SudokuCharacters("123456", '.'); } }
        public static SudokuCharacters Sudoku8Set { get { return new SudokuCharacters("12345678", '.'); } }
        public static SudokuCharacters Sudoku9Set { get { return new SudokuCharacters("123456789", '.'); } }
        public static SudokuCharacters Sudoku10Set { get { return new SudokuCharacters("123456789A", '.'); } }
        public static SudokuCharacters Sudoku12Set { get { return new SudokuCharacters("123456789ABC", '.'); } }
        public static SudokuCharacters Sudoku14Set { get { return new SudokuCharacters("123456789ABCDE", '.'); } }
        public static SudokuCharacters Sudoku15Set { get { return new SudokuCharacters("123456789ABCDEF", '.'); } }
        public static SudokuCharacters Sudoku16Set { get { return new SudokuCharacters("123456789ABCDEFG", '.'); } }
        public static SudokuCharacters Sudoku18Set { get { return new SudokuCharacters("123456789ABCDEFGHI", '.'); } }
        public static SudokuCharacters Sudoku20Set { get { return new SudokuCharacters("123456789ABCDEFGHIJK", '.'); } }

        #endregion
        #region Public Properties

        /// <summary>
        /// returns character with the given index. 0 returns the empty character
        /// </summary>
        /// <returns></returns>
        public char this[byte idx]
        {
            get
            {
                if (idx == 0)
                    return emptyCharacter;
                else if (idx > characters.Count)
                    throw new IndexOutOfRangeException("invalid character index");
                else
                    return characters[idx - 1];
            }
        }

        /// <summary>
        /// returns the character 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public byte this[char ch]
        {
            get
            {
                if (ch == emptyCharacter)
                    return 0;
                else
                {
                    int i = characters.IndexOf(ch);
                    if (i == -1)
                        throw new IndexOutOfRangeException("invalid character, it's not part of the character set");
                    return (byte)(i + 1);
                }
            }
        }

        /// <summary>
        /// Number of characters in the character set, without the empty character
        /// </summary>
        public int Count { get { return characters.Count; } }

        /// <summary>
        /// The character representing 'empty', i.e. a field without content in the Sudoku
        /// </summary>
        public char EmptyCharacter { get { return emptyCharacter; } }

        /// <summary>
        /// set of characters (without the empty character)
        /// </summary>
        public string CharacterString { get { return characterString; } }

        #endregion
        #region Public Methods

        public SudokuCharacters(SudokuCharacters other)
        {
            this.characters = new List<char>(other.characters);
            this.characterString = other.characterString;
            this.emptyCharacter = other.emptyCharacter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="emptyChars"></param>
        public SudokuCharacters(string chars, char emptyChar) : this(chars.ToCharArray(), emptyChar) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="emptyChars"></param>
        public SudokuCharacters(char[] chars, char emptyChar)
        {
            if ((chars == null) || (chars.Length == 0))
                throw new ArgumentException("characters set must not be initialized without characters");
            if (chars.Contains(emptyChar))
                throw new ArgumentException("the empty character must not be part of the character set");

            characters = new List<char>(chars);
            characterString = new string(chars);            
            emptyCharacter = emptyChar;
        }

        /// <summary>
        /// Checks whether c is a valid character (empty or non-empty)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsValidChar(char c)
        {
            return (c == emptyCharacter) || characters.Contains(c);
        }

        /// <summary>
        /// Checks if idx is a valid character index. 0 corresponds to the empty character and
        /// the indexes 1..n where n is the number of non-empty characters are valid indexes
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool IsValidIndex(byte idx)
        {
            return idx <= characters.Count;
        }

        /// <summary>
        /// Checks if c is a valid non-empty character
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsValidNonEmptyChar(char c)
        {
            return characters.Contains(c);
        }

        /// <summary>
        /// Shuffles the characters randomly
        /// </summary>
        public void Shuffle()
        {
            var vec = new List<Tuple<char, double>>();
            foreach (var c in characters)
                vec.Add(new Tuple<char, double>(c, rand.NextDouble()));
            vec.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            this.characterString = "";
            for (int i = 0; i < vec.Count; i++)
            {
                this.characters[i] = vec[i].Item1;
                this.characterString += vec[i].Item1;
            }
        }

        public override string ToString()
        {
            return "{" + CharacterString + " | empty: " + EmptyCharacter + "}";
        }

        #endregion  
    }
}
