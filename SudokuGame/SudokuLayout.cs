using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    /// <summary>
    /// Defines a Sudoku layout together with a matching character set
    /// 
    /// This class is immutable!
    /// </summary>
    public sealed class SudokuLayout
    {
        #region Fields
        
        private readonly int sideLength;
        private readonly Int2D blockDimension;
        private readonly Int2D blockLayout;
        private readonly int fieldCount;
        private readonly int blockFieldCount;
        private readonly int blockCount;

        #endregion
        #region Default layouts

        public static readonly SudokuLayout Layout4x4 = new SudokuLayout(4, new Int2D(2, 2), new Int2D(2, 2));        
        public static readonly SudokuLayout Layout6x6l = new SudokuLayout(6, new Int2D(2, 3), new Int2D(3, 2));
        public static readonly SudokuLayout Layout6x6p = new SudokuLayout(6, new Int2D(3, 2), new Int2D(2, 3));
        public static readonly SudokuLayout Layout8x8l = new SudokuLayout(8, new Int2D(2, 4), new Int2D(4, 2));
        public static readonly SudokuLayout Layout8x8p = new SudokuLayout(8, new Int2D(4, 2), new Int2D(2, 4));
        public static readonly SudokuLayout Layout9x9 = new SudokuLayout(9, new Int2D(3, 3), new Int2D(3, 3));
        public static readonly SudokuLayout Layout10x10l = new SudokuLayout(10, new Int2D(2, 5), new Int2D(5, 2));
        public static readonly SudokuLayout Layout10x10p = new SudokuLayout(10, new Int2D(5, 2), new Int2D(2, 5));
        public static readonly SudokuLayout Layout12x12l = new SudokuLayout(12, new Int2D(3, 4), new Int2D(4, 3));
        public static readonly SudokuLayout Layout12x12p = new SudokuLayout(12, new Int2D(4, 3), new Int2D(3, 4));
        public static readonly SudokuLayout Layout14x12l = new SudokuLayout(14, new Int2D(2, 7), new Int2D(7, 2));
        public static readonly SudokuLayout Layout14x12p = new SudokuLayout(14, new Int2D(7, 2), new Int2D(2, 7));
        public static readonly SudokuLayout Layout15x15l = new SudokuLayout(15, new Int2D(3, 5), new Int2D(5, 3));
        public static readonly SudokuLayout Layout15x15p = new SudokuLayout(15, new Int2D(5, 3), new Int2D(3, 5));
        public static readonly SudokuLayout Layout16x16 = new SudokuLayout(16, new Int2D(4, 4), new Int2D(4, 4));
        public static readonly SudokuLayout Layout18x18l = new SudokuLayout(18, new Int2D(2, 9), new Int2D(9, 2));
        public static readonly SudokuLayout Layout18x18p = new SudokuLayout(18, new Int2D(9, 2), new Int2D(2, 9));
        public static readonly SudokuLayout Layout20x20l = new SudokuLayout(20, new Int2D(4, 5), new Int2D(5, 4));
        public static readonly SudokuLayout Layout20x20p = new SudokuLayout(20, new Int2D(5, 4), new Int2D(4, 5));

        #endregion
        #region Public Methods / Properties

        /// <summary>
        /// Returns true if the blocks are square
        /// </summary>
        public bool IsBlockSquare { get { return BlockDimension.Row == BlockDimension.Col; } }

        /// <summary>
        /// Total number of fields
        /// </summary>
        public int FieldCount { get { return fieldCount; } }

        /// <summary>
        /// Number of fields within one sub-rectangle
        /// </summary>
        public int BlockFieldCount { get { return blockFieldCount; } }

        /// <summary>
        /// Number of sub-rectangles in the Sudoku
        /// </summary>
        public int BlockCount { get { return blockCount; } }

        /// <summary>
        /// Overall rows x columns of the Sudoku (9x9 for the standard sudoku)
        /// </summary>
        public int SideLength { get { return sideLength; } }

        /// <summary>
        /// rows x columns of one sub-rectangle of the Sudoku (3x3 for the standard sudoku)
        /// </summary>
        public Int2D BlockDimension { get { return blockDimension; } }

        /// <summary>
        /// Arrangement of the sub-rectangles of the Sudoku, number of rows and columns (3x3 for the standard sudoku)
        /// </summary>
        public Int2D BlockLayout { get { return blockLayout; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public SudokuLayout(int sideLen, Int2D blockDimension, Int2D blockLayout)
        {
            this.sideLength = sideLen;
            this.blockDimension = blockDimension;
            this.blockLayout = blockLayout;
            this.fieldCount = sideLen * sideLen;
            this.blockFieldCount = blockDimension.Product;
            this.blockCount = blockLayout.Product;

            // do some checks
            if ((sideLen <= 0) || BlockDimension.IsUndefined || blockLayout.IsUndefined)
                throw new ArgumentException("Invalid layout definition. Sidelenght, block dimension and layout must all be positive");
            if ((sideLen != BlockDimension.Row * BlockLayout.Row) || (sideLen != BlockDimension.Col * BlockLayout.Col))
                throw new ArgumentException("Invalid layout definition, the side length and block dimension / layout do not match");            
        }

        public override string ToString()
        {
            return string.Format("{0}x{0} ({1}x{2} blocks of size {3}x{4})", SideLength, BlockLayout.Row, BlockLayout.Col, BlockDimension.Row, BlockDimension.Col);
        }

        #endregion
    }
}
