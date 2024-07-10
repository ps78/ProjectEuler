using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SudokuGame
{
    public class ExcelWriter
    {
        public static void WriteToFile(IEnumerable<Sudoku> sudokus, string filename, int columns)
        {
            var file = new FileInfo(filename);
            if (file.Exists)
                file.Delete();
            
            using (var package = new ExcelPackage(file))
            {
                var sudokuSheet = CreateWorksheet(package, "Sudokus");
                var solutionSheet = CreateWorksheet(package, "Solutions");

                int curColumn = 0;
                int row = 1;
                foreach (var sudoku in sudokus)
                {
                    int size = sudoku.Layout.SideLength;

                    WriteSingleSudokuToWorksheet(sudokuSheet, sudoku, row, 2 + curColumn * (size + 1));
                    if (sudoku.Solution != null)
                        WriteSingleSudokuToWorksheet(solutionSheet, sudoku.Solution, row, 2 + curColumn * (size + 1));

                    curColumn = (curColumn + 1) % columns;
                    row += (curColumn != 0 ? 0 : size + 1);
                }
                

                package.Save();
            }   
        }

        /// <summary>
        /// Creates a new worksheet with standard formats
        /// </summary>
        /// <param name="package"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private static ExcelWorksheet CreateWorksheet(ExcelPackage package, string title)
        {
            var ws = package.Workbook.Worksheets.Add(title);

            ws.View.ShowGridLines = false;

            ws.Column(1).Width = 2;
            ws.PrinterSettings.FitToWidth = 1;
            ws.PrinterSettings.FitToHeight = 0;
            ws.PrinterSettings.FitToPage = true;

            return ws;
        }

        /// <summary>
        /// Writes a single Sudoku at position (row,col) into the worksheet
        /// </summary>
        private static void WriteSingleSudokuToWorksheet(ExcelWorksheet worksheet, Sudoku sudoku, int row, int col)
        {
            int size = sudoku.Layout.SideLength;

            // write and format the title
            var titleCell = worksheet.Cells[row, col];
            titleCell.Value = "Sudoku " + sudoku.Number.ToString();
            titleCell.Style.Font.Size = 20;
            worksheet.Row(row).Height = 35;

            // write the number of clues
            var topRightCell = worksheet.Cells[row, col + size - 1];
            topRightCell.Value = sudoku.ClueCount.ToString() + " clues";
            topRightCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            // write the actual Sudoku
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                    if (sudoku[r, c] != sudoku.CharacterSet.EmptyCharacter)
                    {
                        byte chVal = (byte)(sudoku[r, c]);                        
                        if ((chVal >= 48) && (chVal <= 57))
                            worksheet.Cells[row + r + 1, col + c].Value = chVal - 48;
                        else
                            worksheet.Cells[row + r + 1, col + c].Value = sudoku[r, c].ToString();
                    }

                // draw horizontal lines
                if (r < size - 1)
                    worksheet.Cells[row + r + 1, col, row + r + 1, col + size - 1].Style.Border.Bottom.Style =
                        ((r + 1) % sudoku.Layout.BlockDimension.Row == 0) ? ExcelBorderStyle.Medium : ExcelBorderStyle.Thin;

                // set the row heights
                worksheet.Row(row + r + 1).Height = 28;
            }
            // draw the vertical lines
            for (int c = 0; c < size - 1; c++)
                worksheet.Cells[row + 1, col + c, row + size, col + c].Style.Border.Right.Style =
                    ((c + 1) % sudoku.Layout.BlockDimension.Col == 0) ? ExcelBorderStyle.Medium : ExcelBorderStyle.Thin;

            // set the column widths
            for (int c = 0; c < size; c++)
                worksheet.Column(col + c).Width = 5;

            // format the whole Sudoku area
            var sudokuRange = worksheet.Cells[row + 1, col, row + size, col + size - 1];            
            sudokuRange.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            sudokuRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sudokuRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sudokuRange.Style.Font.Size = 20;
        }
    }
}
