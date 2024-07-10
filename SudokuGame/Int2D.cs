using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    /// <summary>
    /// Represents a tuple of two integers, named Row and Col
    /// </summary>
    public struct Int2D
    {        
        #region Properties & public static fields

        public int Row;

        public int Col;

        /// <summary>
        /// Represents (-1,-1)
        /// </summary>
        public static Int2D Undefined = new Int2D(-1, -1);

        /// <summary>
        /// Represents (0,0)
        /// </summary>
        public static Int2D Zero = new Int2D(0, 0);

        /// <summary>
        /// Returns true if either col or row are negative
        /// </summary>
        public bool IsUndefined { get { return ((Row < 0) || (Col < 0)); } }

        /// <summary>
        /// Returns Row * Col
        /// </summary>
        public int Product { get { return Row * Col; } }

        #endregion
        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Int2D(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
           
        #endregion
        #region Operators & overriden basic functionality

        public static bool operator ==(Int2D a, Int2D b)
        {
            return ((a.Row == b.Row) && (a.Col == b.Col));
        }

        public static bool operator !=(Int2D a, Int2D b)
        {
            return ((a.Row != b.Row) || (a.Col != b.Col));
        }

        public static Int2D operator +(Int2D a, Int2D b)
        {
            return new Int2D(a.Row + b.Row, a.Col + b.Col);
        }

        public static Int2D operator -(Int2D a, Int2D b)
        {
            return new Int2D(a.Row - b.Row, a.Col - b.Col);
        }

        public override bool Equals(object obj)
        {
            if (obj is Int2D)
            {
                Int2D other = (Int2D)obj;
                return (this.Row == other.Row) && (this.Col == other.Col);
            }
            else
                return false;
        }

        public override string ToString()
        {
            if ((Row == -1) && (Col == -1))
                return "(undefined)";
            else
                return "(" + Row.ToString() + ", " + Col.ToString() + ")";
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() | Col.GetHashCode();
        }

        #endregion
    }
}
