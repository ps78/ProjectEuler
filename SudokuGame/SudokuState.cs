using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    /// <summary>
    /// Represents the internal state of a Sudoku
    /// Used as component of the Sudoku class, not to be used individually
    /// </summary>
    public sealed class SudokuState
    {
        #region Private Fields
        
        private readonly byte[] data; // the actual data, aligned row by row. Must never be directly accessed, use this[] instead!        

        // data available as public property
        private readonly SudokuLayout layout;
        private readonly int[] blockIndex;
        private int clueCount;
        private readonly ulong[] rowStates;
        private readonly ulong[] colStates;
        private readonly ulong[] blockStates;
        
        #endregion
        #region Public Properties

        /// <summary>
        /// Defines the layout incl. character set of the Sudoku
        /// </summary>
        public SudokuLayout Layout { get { return layout; } }

        /// <summary>
        /// Direct access to the sudoku values. Index is linear, row-by-row and zero based
        /// No rule checks in the set-method. Use TrySet() 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public byte this[int idx]
        {
            get
            {
                if ((idx < 0) || (idx >= data.Length))
                    throw new IndexOutOfRangeException("Trying to access position with invalid index " + idx.ToString());

                return data[idx];
            }
            private set
            {
                byte curValue = data[idx];
                if (curValue != value)
                {
                    if ((value < 0) || (value > Layout.SideLength))
                        throw new ArgumentException("'" + value + "' is not a valid character index");

                    byte bitPos;
                    int row = idx / layout.SideLength;
                    int col = idx % layout.SideLength;
                    // clear current bits
                    if (curValue != 0)
                    {
                        bitPos = (byte)(curValue - 1);
                        rowStates[row] = rowStates[row].ClearBit(bitPos);
                        colStates[col] = colStates[col].ClearBit(bitPos);
                        blockStates[blockIndex[idx]] = blockStates[blockIndex[idx]].ClearBit(bitPos);
                        if (value == 0)
                            clueCount--;
                    }
                    // set new bits
                    if (value != 0)
                    {
                        bitPos = (byte)(value - 1);
                        rowStates[row] = rowStates[row].SetBit(bitPos);
                        colStates[col] = colStates[col].SetBit(bitPos);
                        blockStates[blockIndex[idx]] = blockStates[blockIndex[idx]].SetBit(bitPos);
                        if (curValue == 0)
                            clueCount++;
                    }

                    data[idx] = value;
                }
            }
        }

        /// <summary>
        /// Access to a single position of the Sudoku. 
        /// Value values are 0 for unset and 1-9 for set values
        /// </summary>
        /// <param name="row">row id, 0-8</param>
        /// <param name="col">column id, 0-8</param>
        /// <returns></returns>
        public byte this[int row, int col]
        {
            get { return this[row * layout.SideLength + col]; }
            private set { this[row * layout.SideLength + col] = value; }
        }

        /// <summary>
        /// Number of clues (non-empty fields)
        /// </summary>
        public int ClueCount { get { return clueCount; } }

        /// <summary>
        /// For each row i, the RowState[i] shows which symbols are present in the row
        /// bit 0 corresponds to the first symbol, etc.
        /// </summary>
        public ulong[] RowStates { get { return rowStates; } }

        /// <summary>
        /// For each column i, the ColState[i] shows which symbols are present in the column
        /// bit 0 corresponds to the first symbol, etc.
        /// </summary>
        public ulong[] ColStates { get { return colStates; } }

        /// <summary>
        /// For each block i, the BlockState[i] shows which symbols are present in the block
        /// bit 0 corresponds to the first symbol, etc.
        /// Blocks are indexed row-by-row, starting at 0
        /// </summary>
        public ulong[] BlockStates { get { return blockStates; } }

        /// <summary>
        /// Maps the data-index to the block-index
        /// I.e. for each index i in data[], BlockIndex[i] returns the index of the corresponding block
        /// Blocks are indexed row-by-row, starting at 0
        /// </summary>
        public int[] BlockIndex { get { return blockIndex; } }

        #endregion
        #region Constructors

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="copyFrom"></param>
        public SudokuState(SudokuState other)
        {
            this.clueCount = other.clueCount;
            this.layout = other.layout;

            this.data = new byte[layout.FieldCount];
            other.data.CopyTo(this.data, 0);

            this.rowStates = new ulong[layout.SideLength];
            other.rowStates.CopyTo(this.rowStates, 0);

            this.colStates = new ulong[layout.SideLength];
            other.colStates.CopyTo(this.colStates, 0);

            this.blockStates = new ulong[layout.BlockCount];
            other.blockStates.CopyTo(this.blockStates, 0);

            this.blockIndex = new int[layout.FieldCount];
            other.blockIndex.CopyTo(this.blockIndex, 0);
        }

        /// <summary>
        /// Constructor, creates an empty sudoku
        /// </summary>
        /// <param name="ID"></param>
        public SudokuState(SudokuLayout layout)
        {
            if (layout == null)
                throw new ArgumentNullException("layout");

            this.layout = layout;
            this.data = new byte[Layout.FieldCount];
            this.rowStates = new ulong[Layout.SideLength];
            this.colStates = new ulong[Layout.SideLength];
            this.blockStates = new ulong[Layout.BlockCount];
            this.blockIndex = new int[Layout.FieldCount];

            // calculate the block-indexes
            for (int pos = 0; pos < Layout.FieldCount; pos++)
                this.blockIndex[pos] = ((pos / Layout.SideLength) / Layout.BlockDimension.Row) * Layout.BlockLayout.Col + (pos % Layout.SideLength) / Layout.BlockDimension.Col;

            Clear();
        }

        #endregion
        #region Public Methods
        
        /// <summary>
        /// Clears the whole Sudoku
        /// </summary>
        public void Clear()
        {
            clueCount = 0;
            for (int i = 0; i < data.Length; i++)
                data[i] = 0;
            for (int i = 0; i < rowStates.Length; i++)
                rowStates[i] = 0;
            for (int i = 0; i < colStates.Length; i++)
                colStates[i] = 0;
            for (int i = 0; i < blockStates.Length; i++)
                blockStates[i] = 0;
        }

        /// <summary>
        /// Clears the field at the given position
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Clear(int row, int col)
        {
            this[row, col] = 0;
        }

        /// <summary>
        /// clear the field at the given index
        /// </summary>
        /// <param name="idx"></param>
        public void Clear(int idx)
        {
            this[idx] = 0;
        }

        /// <summary>
        /// Tries to set position [row,col] to value. Returns false if this would result in an invalid
        /// Sudoku, true otherwise. If true, the value is also updated 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TrySet(int row, int col, byte value) 
        {
            return TrySet(row * layout.SideLength + col, value);
        }

        /// <summary>
        /// Tries to set positio nidx to value. Returns false if this would result in an invalid
        /// Sudoku, true otherwise. If true, the value is also updated 
        /// </summary>
        /// <returns></returns>
        public bool TrySet(int idx, byte value)
        {
            if (value == this[idx])
                return true;

            if (value != 0) // 'empty' will always be set
            {
                byte bitPos = (byte)(value - 1);
                if (rowStates[idx / Layout.SideLength].GetBit(bitPos) ||
                    colStates[idx % Layout.SideLength].GetBit(bitPos) ||
                    blockStates[blockIndex[idx]].GetBit(bitPos))
                    return false;
            }
            this[idx] = value;

            return true;
        }

        /// <summary>
        /// Swaps two rows. The rows must be within the same block-row!
        /// </summary>
        public void SwapRows(int row1, int row2)
        {            
            if (row1 / Layout.BlockDimension.Row != row2 / Layout.BlockDimension.Row)
                throw new ArgumentException("Cannot swap rows that are not within the same block row");

            DirectSwapRows(row1, row2);            
        }

        /// <summary>
        /// Swaps two columns. The columns must be within the same block-column!
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public void SwapColumns(int col1, int col2)
        {         
            if (col1 / Layout.BlockDimension.Col != col2 / Layout.BlockDimension.Col)
                throw new ArgumentException("Cannot swap columns that are not within the same block column");

            DirectSwapColumns(col1, col2);
        }

        /// <summary>
        /// Swaps a whole row-block, i.e. all rows belonging to one block with another
        /// </summary>
        /// <param name="blockRow1"></param>
        /// <param name="blockRow2"></param>
        public void SwapBlockRows(int blockRow1, int blockRow2)
        {
            int s = Layout.SideLength;

            if ((blockRow1 < 0) || (blockRow2 < 0) || (blockRow1 >= Layout.BlockLayout.Row) || (blockRow2 >= Layout.BlockLayout.Row))
                throw new ArgumentException("Block row indexes are invalid");            

            // swap rows
            for (int k = 0; k < Layout.BlockDimension.Row; k++)
                DirectSwapRows(blockRow1 * Layout.BlockDimension.Row + k, blockRow2 * Layout.BlockDimension.Row + k);

            // update BlockStates
            for (int k = 0; k < Layout.BlockLayout.Col; k++)
            {
                ulong t = BlockStates[blockRow1 * Layout.BlockLayout.Col + k];
                BlockStates[blockRow1 * Layout.BlockLayout.Col + k] = BlockStates[blockRow2 * Layout.BlockLayout.Col + k];
                BlockStates[blockRow2 * Layout.BlockLayout.Col + k] = t;
            }
        }

        /// <summary>
        /// Swaps a whole column-block, i.e. all columns belonging to one block with another
        /// </summary>
        public void SwapBlockColumns(int blockCol1, int blockCol2)
        {
            int s = Layout.SideLength;

            if ((blockCol1 < 0) || (blockCol2 < 0) || (blockCol1 >= Layout.BlockLayout.Col) || (blockCol2 >= Layout.BlockLayout.Col))
                throw new ArgumentException("Block column indexes are invalid");
            
            // swap columns
            for (int k = 0; k < Layout.BlockDimension.Col; k++)
                DirectSwapColumns(blockCol1 * Layout.BlockDimension.Col + k, blockCol2 * Layout.BlockDimension.Col + k);

            // update BlockStates
            for (int k = 0; k < Layout.BlockLayout.Row; k++)
            {
                ulong t = BlockStates[k * Layout.BlockLayout.Col + blockCol1];
                BlockStates[k * Layout.BlockLayout.Col + blockCol1] = BlockStates[k * Layout.BlockLayout.Col + blockCol2];
                BlockStates[k * Layout.BlockLayout.Col + blockCol2] = t;
            }
        }

        /// <summary>
        /// Flips the Sudoku horizontally
        /// </summary>
        public void FlipLeftRight()
        {
            int s = Layout.SideLength;

            // swap columns
            for (int c = 0; c < s / 2; c++)
                DirectSwapColumns(c, s - c - 1);

            // swap block states
            for (int bc = 0; bc < Layout.BlockLayout.Col / 2; bc++)
                for (int br = 0; br < Layout.BlockLayout.Row; br++)
                {
                    int bc2 = Layout.BlockLayout.Col - bc - 1;
                    ulong t = BlockStates[br * Layout.BlockLayout.Col + bc];
                    BlockStates[br * Layout.BlockLayout.Col + bc] = BlockStates[br * Layout.BlockLayout.Col + bc2];
                    BlockStates[br * Layout.BlockLayout.Col + bc2] = t;
                }
        }

        /// <summary>
        /// Flips the Sudoku vertically
        /// </summary>
        public void FlipTopDown()
        {
            int s = Layout.SideLength;

            // swap rows
            for (int r = 0; r < s / 2; r++)
                DirectSwapRows(r, s - r - 1);

            // swap block states
            for (int br = 0; br < Layout.BlockLayout.Row / 2; br++)
                for (int bc = 0; bc < Layout.BlockLayout.Col; bc++)
                {
                    int br2 = Layout.BlockLayout.Row - br - 1;
                    ulong t = BlockStates[br * Layout.BlockLayout.Col + bc];
                    BlockStates[br * Layout.BlockLayout.Col + bc] = BlockStates[br2 * Layout.BlockLayout.Col + bc];
                    BlockStates[br2 * Layout.BlockLayout.Col + bc] = t;
                }
        }

        /// <summary>
        /// Flips the Sudoku along the diagonal from bottom left to top right.
        /// Only allowed for Sudokus with square blocks
        /// </summary>
        public void FlipDiagonalUp()
        {
            RotateRight();
            FlipTopDown();
        }

        /// <summary>
        /// Flips the Sudoku along the diagonal from top left to bottom right.
        /// Only allowed for Sudokus with square blocks
        /// </summary>
        public void FlipDiagonalDown()
        {
            RotateLeft();
            FlipTopDown();
        }

        /// <summary>
        /// Rotates the Sudoko right by 90°. Only allowed for Sudokus with square blocks
        /// </summary>
        public void RotateRight()
        {
            int s = Layout.SideLength;

            if (Layout.BlockDimension.Row != Layout.BlockDimension.Col)
                throw new InvalidOperationException("Can not rotate a Sudoku with non-square blocks");

            // copy the current data
            byte[] dat = new byte[Layout.FieldCount];
            data.CopyTo(dat, 0);

            Clear();

            for (int newRow = 0; newRow < Layout.SideLength; newRow++)
                for (int newCol = 0; newCol < Layout.SideLength; newCol++)
                {
                    int oldRow = s - newCol - 1;
                    int oldCol = newRow;
                    int oldIdx = oldRow * s + oldCol;
                    this[newRow, newCol] = dat[oldIdx];
                }
        }

        /// <summary>
        /// Rotates the Sudoko left by 90°. Only allowed for Sudokus with square blocks
        /// </summary>
        public void RotateLeft()
        {
            int s = Layout.SideLength;

            if (Layout.BlockDimension.Row != Layout.BlockDimension.Col)
                throw new InvalidOperationException("Can not rotate a Sudoku with non-square blocks");

            // copy the current data
            byte[] dat = new byte[Layout.FieldCount];
            data.CopyTo(dat, 0);

            Clear();

            for (int newRow = 0; newRow < Layout.SideLength; newRow++)
                for (int newCol = 0; newCol < Layout.SideLength; newCol++)
                {
                    int oldRow = newCol;
                    int oldCol = s - newRow - 1;
                    int oldIdx = oldRow * s + oldCol;
                    this[newRow, newCol] = dat[oldIdx];
                }
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Swaps two rows, manipulating directly the data-fields.
        /// NOTE: RowStates are updated, but the BlockStates are not
        /// So, if row1 and row2 are not within the same block row, you need to update the Blockstates seperately!!
        /// </summary>
        private void DirectSwapRows(int row1, int row2)
        {
            int s = Layout.SideLength;

            if ((row1 < 0) || (row2 < 0) || (row1 >= s) || (row2 >= s))
                throw new ArgumentException("Row-indexes are invalid");

            if (row1 == row2)
                return;

            // swap the data
            for (int i = 0; i < s; i++)
            {
                byte tmp = data[row1 * s + i];
                data[row1 * s + i] = data[row2 * s + i];
                data[row2 * s + i] = tmp;
            }

            // swap the row states
            ulong t = RowStates[row1];
            RowStates[row1] = RowStates[row2];
            RowStates[row2] = t;            
        }

        /// <summary>
        /// Swaps two columns, manipulating directly the data-fields.
        /// NOTE: ColumnStates are updated, but the BlockStates are not
        /// So, if col1 and col2 are not within the same block column, you need to update the Blockstates seperately!!
        /// </summary>
        private void DirectSwapColumns(int col1, int col2)
        {
            int s = Layout.SideLength;

            if ((col1 < 0) || (col2 < 0) || (col1 >= s) || (col2 >= s))
                throw new ArgumentException("Column-indexes are invalid");

            if (col1 == col2)
                return;

            // swap the data
            for (int i = 0; i < s; i++)
            {
                byte tmp = data[i * s + col1];
                data[i * s + col1] = data[i * s + col2];
                data[i * s + col2] = tmp;
            }

            // swap the column states
            ulong t = ColStates[col1];
            ColStates[col1] = ColStates[col2];
            ColStates[col2] = t;
        }

        #endregion
    }
}
