using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SudokuGame
{    
    public class Sudoku
    {
        #region Private Fields

        private int number = 0;
        private SudokuState state;        
        private SudokuCharacters characterSet;        
        private static Random rand = new Random();
        
        #endregion
        #region Public Properties

        /// <summary>
        /// Just some identifier / name for the Sudoko
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        
        /// <summary>
        /// Property to store the solution of the Sudoku. This is not automatically generated
        /// </summary>
        public Sudoku Solution { get; set; }

        /// <summary>
        /// Represents the internal state of the sudoku
        /// </summary>
        public SudokuState State { get { return state; } }

        /// <summary>
        /// Defines the layout incl. character set of the Sudoku
        /// </summary>
        public SudokuLayout Layout { get { return state.Layout; } }

        /// <summary>
        /// Set of characters
        /// </summary>
        public SudokuCharacters CharacterSet { get { return characterSet; } }
        
        /// <summary>
        /// Access to a single position of the Sudoku. 
        /// Value values are 0 for unset and 1-9 for set values.
        /// To set a value, use TrySet(), which ensures that only valid combinations can be set
        /// </summary>
        /// <param name="row">row id, 0-8</param>
        /// <param name="col">column id, 0-8</param>
        /// <returns></returns>
        public char this[int row, int col]
        {
            get { return characterSet[state[row * Layout.SideLength + col]]; }            
        }
                
        /// <summary>
        /// Number of clues (non-empty fields)
        /// </summary>
        public int ClueCount { get { return state.ClueCount; } }

        #endregion
        #region Constructors

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="copyFrom"></param>
        public Sudoku(Sudoku copyFrom)
        {
            this.Number = copyFrom.Number;
            this.characterSet = new SudokuCharacters(copyFrom.characterSet);
            this.state = new SudokuState(copyFrom.state);            
        }

        /// <summary>
        /// Constructor, creates an empty sudoku
        /// </summary>
        /// <param name="ID"></param>
        public Sudoku(SudokuLayout layout = null, SudokuCharacters charSet = null, int number = 0)
        {
            this.state = new SudokuState((layout == null ? SudokuLayout.Layout9x9 : layout));
            this.characterSet = (charSet == null ? SudokuCharacters.Sudoku9Set : charSet);
            this.number = number;

            // do some checks
            if (Layout.SideLength > CharacterSet.Count)
                throw new ArgumentException("The character set does not have enough characters for the given layout");
        }

        /// <summary>
        /// Constructor, creates a initialized sudoku
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="input">9x9 array of bytes ranging containing 0 (unset) or 1-9 for values set</param>
        public Sudoku(char[] input, SudokuLayout layout = null, SudokuCharacters charSet = null, int number = 0) : this(layout, charSet, number)
        {            
            if (input.Length != Layout.FieldCount)
                throw new ArgumentException("Error initializing Sudoku from char array, invalid dimensions");

            try
            {
                for (int i = 0; i < Layout.FieldCount; i++)
                    if (!state.TrySet(i, CharacterSet[input[i]]))
                        throw new InvalidOperationException("Data violates Sudoku rules");
            }
            catch(Exception e)
            {
                throw new ArgumentException("Error initializing Sudoku from character array", e);
            }
        }

        /// <summary>
        /// Constructor, creates an initialized sudoku from a number string
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="input">string of 81 characters from 0 (unset) to 1-9 (set), row by row</param>
        public Sudoku(string input, SudokuLayout layout = null, SudokuCharacters charSet = null, int number = 0) : 
            this(input.ToCharArray(), layout, charSet, number) { }

        #endregion
        #region Static Methods

        /// <summary>
        /// Reads the Sudokus from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IEnumerable<Sudoku> ReadFromFile(string fileName, SudokuLayout layout, SudokuCharacters charSet)
        {
            string dat = File.ReadAllText(fileName);
            string[] parts = dat.Split(new string[] { "Grid" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                string line = part.Replace("\n", "").Replace("\r", "").Trim();
                yield return new Sudoku(
                    layout: layout, 
                    charSet: charSet,
                    input: line.Remove(0, 2).Replace('0', charSet.EmptyCharacter),
                    number: int.Parse(line.Substring(0, 2))
                );
            }
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Creates a copy of the current state. Can be restored using RestoreState()
        /// </summary>
        /// <returns></returns>
        public SudokuState BackupState()
        {
            return new SudokuState(this.state);
        }

        /// <summary>
        /// Restores a state that has previously been backed up using BackupState()
        /// </summary>
        /// <param name="s"></param>
        public void RestoreState(SudokuState s)
        {
            this.state = new SudokuState(s);
            if (Layout.SideLength != CharacterSet.Count)
                throw new InvalidOperationException("Invalid state to restore: character set and layout do not match");
        }

        /// <summary>
        /// Clears the whole Sudoku
        /// </summary>
        public void Clear()
        {
            state.Clear();               
        }

        /// <summary>
        /// Clears the given position
        /// </summary>
        public void Clear(int row, int pos)
        {
            state.Clear(row, pos);
        }

        /// <summary>
        /// Tries to set position [row,col] to value. Returns false if this would result in an invalid
        /// Sudoku, true otherwise. If true, the value is also updated 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TrySet(int row, int col, char value)
        {
            return state.TrySet(row * Layout.SideLength + col, CharacterSet[value]);            
        }
        
        public void Shuffle(int numberOfTransformations = 100, bool shuffleCharacterSet = true)
        {
            if (shuffleCharacterSet)
                CharacterSet.Shuffle();
            
            for (int i = 0; i < numberOfTransformations; i++)
            {
                switch(rand.Next(1, Layout.IsBlockSquare ? 11 : 5))
                {                    
                    case 1:
                        int c1 = rand.Next(0, Layout.BlockDimension.Col - 1);
                        int c2 = rand.Next(c1 + 1, Layout.BlockDimension.Col);
                        int blockcol = rand.Next(0, Layout.BlockLayout.Col) * Layout.BlockDimension.Col;
                        State.SwapColumns(blockcol + c1, blockcol + c2);
                        break;
                    case 2:
                        int r1 = rand.Next(0, Layout.BlockDimension.Row - 1);
                        int r2 = rand.Next(r1 + 1, Layout.BlockDimension.Row);
                        int blockrow = rand.Next(0, Layout.BlockLayout.Row) * Layout.BlockDimension.Row;
                        State.SwapRows(blockrow + r1, blockrow + r2);
                        break;
                    case 3:
                        int bc1 = rand.Next(0, Layout.BlockLayout.Col - 1);
                        int bc2 = rand.Next(bc1 + 1, Layout.BlockLayout.Col);
                        State.SwapBlockColumns(bc1, bc2);
                        break;
                    case 4:
                        int br1 = rand.Next(0, Layout.BlockLayout.Row - 1);
                        int br2 = rand.Next(br1 + 1, Layout.BlockLayout.Row);
                        State.SwapBlockRows(br1, br2);
                        break;
                    case 5: State.FlipLeftRight(); break;
                    case 6: State.FlipTopDown(); break;
                    case 7: State.FlipDiagonalDown(); break;
                    case 8: State.FlipDiagonalUp(); break;
                    case 9: State.RotateLeft(); break;
                    case 10: State.RotateRight(); break;
                }
            }

        }

        /// <summary>
        /// Returns a formated Sudoku grid as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {            
            // define frame-characters
            var frame = new {
                // corners
                TopLeft = '\u250E', TopRight = '\u2512', BottomLeft = '\u2516', BottomRight = '\u251A',
                // horizontal / vertical lines
                HLine = '\u2500', VLine = '\u2503',
                // T-s
                DownT = '\u2530', UpT = '\u2538', LeftT = '\u2528', RightT = '\u2520', Cross = '\u2542', 
                // length of one space
                Space = " "                
            };

            var result = new StringBuilder();

            if (ClueCount == Layout.FieldCount)
                result.Append("Sudoku " + Number.ToString() + " (solved)\n");
            else
                result.Append("Sudoku " + Number.ToString() + " (" + ClueCount.ToString() + " clues)\n");

            string hSegment = new string(frame.HLine, (frame.Space.Length + 1) * Layout.BlockDimension.Col + frame.Space.Length);

            // top line
            result.Append(frame.TopLeft);
            for (int superCol = 0; superCol < Layout.BlockLayout.Col - 1; superCol++)
                result.Append(hSegment + frame.DownT);
            result.Append(hSegment + frame.TopRight + "\n");

            int row = 0;
            for (int superRow = 0; superRow < Layout.BlockLayout.Row; superRow++)
            {
                for (int subRow = 0; subRow < Layout.BlockDimension.Row; subRow++)
                {
                    result.Append(frame.VLine + frame.Space);
                    int col = 0;
                    for (int superCol = 0; superCol < Layout.BlockLayout.Col; superCol++)
                    {
                        for (int subCol = 0; subCol < Layout.BlockDimension.Col; subCol++)
                        {
                            result.Append(this[row, col].ToString() + frame.Space);
                            col++;
                        }
                        result.Append(frame.VLine + frame.Space);
                    }
                    result.Append("\n");
                    row++;
                }
                // horizontal line between two sub-rectangles
                if (superRow < Layout.BlockLayout.Row - 1)
                {
                    result.Append(frame.RightT);
                    for (int superCol = 0; superCol < Layout.BlockLayout.Col - 1; superCol++)
                        result.Append(hSegment + frame.Cross);
                    result.Append(hSegment + frame.LeftT + "\n");
                }
            }

            // bottom line
            result.Append(frame.BottomLeft);
            for (int superCol = 0; superCol < Layout.BlockLayout.Col - 1; superCol++)
                result.Append(hSegment + frame.UpT);
            result.Append(hSegment + frame.BottomRight + "\n");

            return result.ToString();
        }

        #endregion
    }
}
