using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public class RecursiveDivision
    {
        Random rand = new Random();

        public void Generate(Maze maze)
        {
            maze.ClearAllWalls();

            Divide(maze, 0, 0, maze.Rows, maze.Columns);
        }

        private void Divide(Maze maze, int startRow, int startCol, int numRows, int numCols)
        {
            if (numRows <= 1  || numCols <= 1)
            {
                return;
            }

            bool DivideHorizontally = ChooseOrientation(numRows, numCols);

            if (DivideHorizontally)
            {
                int wallRow = rand.Next(startRow, startRow + numRows - 1);
                int passageCol = rand.Next(startCol, startCol + numCols);

                for (int col = startCol; col < startCol + numCols; col++)
                {
                    if (col != passageCol)
                    {
                        Cell topCell = maze.GetCell(wallRow, col);
                        Cell bottomCell = maze.GetCell(wallRow + 1, col);
                        maze.AddWallBetween(topCell, bottomCell);
                    }
                }
                Divide(maze, startRow, startCol, wallRow - startRow + 1, numCols);
                Divide(maze, wallRow + 1, startCol, startRow + numRows - (wallRow + 1), numCols);
            }
            else
            {
                int wallCol = rand.Next(startCol, startCol + numCols - 1);
                int passageRow = rand.Next(startRow, startRow + numRows);

                for (int row = startRow; row < startRow + numRows; row++)
                {
                    if (row != passageRow)
                    {
                        Cell leftCell = maze.GetCell(row, wallCol);
                        Cell rightCell = maze.GetCell(row, wallCol + 1);

                        maze.AddWallBetween(leftCell, rightCell);
                    }
                }

                Divide(maze, startRow, startCol, numRows, wallCol - startCol + 1);
                Divide(maze, startRow, wallCol + 1, numRows, startCol + numCols - (wallCol + 1));
            }
        }


        private bool ChooseOrientation(int numRows, int numCols)
        {
            if (numRows > numCols)
            {
                return true;  // cut horizontally if taller
            }
            else if (numCols > numRows)
            {
                return false; // cut vertically if wider
            }
            else
            {
                return rand.Next(2) == 0; // random if square
            }
        }

            
        
    }
}
