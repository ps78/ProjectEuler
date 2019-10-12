using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NumberTheory
{
    public static class IOExtensions
    {
        public static int[,] ReadMatrix(this string FileName)
        {
            try
            {
                var rows = File.ReadAllLines(FileName);

                int nRows = rows.Length;
                int nCols = rows[0].Split(new char[] { ',' }).Length;

                var result = new int[nRows, nCols];
                int rowIdx = 0;
                foreach (string row in rows)
                {
                    var values = row.Split(new char[] { ',' });
                    int colIdx = 0;
                    foreach (var val in values)
                    {
                        result[rowIdx, colIdx] = int.Parse(val);
                        colIdx++;
                    }
                    rowIdx++;
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Error reading matrix from file", ex);
            }
        }

    }
}
