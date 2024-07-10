using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    public static class Extensions
    {
        public static ulong SetBit(this ulong i, byte bitPos)
        {
            ulong b = ((ulong)1) << bitPos;
            return i | b;
        }

        public static ulong ClearBit(this ulong i, byte bitPos)
        {
            ulong b = ulong.MaxValue ^ (((ulong)1) << bitPos);
            return i & b;
        }

        public static ulong FlipBit(this ulong i, byte bitPos)
        {
            ulong b = ((ulong)1) << bitPos;
            return i ^ b;
        }

        public static bool GetBit(this ulong i, byte bitPos)
        {
            ulong b = ((ulong)1) << bitPos;
            return (i & b) == b;
        }
        
        public static byte CountBits(this ulong i, int BitsToConsider = 64)
        {
            byte count = 0;            
            for (byte bitPos = 0; bitPos < BitsToConsider; bitPos++)
            {
                ulong b = ((ulong)1) << bitPos;
                if ((i & b) == b)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Returns an array with all bit positions that are set in i
        /// </summary>
        /// <param name="i"></param>
        /// <param name="BitsToConsider"></param>
        /// <returns></returns>
        public static byte[] GetBits(this ulong i, int BitsToConsider = 64)
        {
            byte[] tmp = new byte[BitsToConsider];
            int j = 0;
            for (byte bitPos = 0; bitPos < BitsToConsider; bitPos++)
            {
                ulong b = ((ulong)1) << bitPos;
                if ((i & b) == b)
                    tmp[j++] = bitPos;
            }
            byte[] result = new byte[j];
            for (int k = 0; k < j; k++)
                result[k] = tmp[k];

            return result;
        }
    }
}
