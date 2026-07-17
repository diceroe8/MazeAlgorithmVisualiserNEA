using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Models
{
    public class Maze
    {
        // stores the dimension of the mazes
        // other classes can read the size but not change it
        public int Rows { get; }
        public int Columns { get; }


        // 2D array to contain all the cells
        private Cell[,] cells;

        // constructor
        public Maze(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            cells = new Cell [ Rows, Columns ]; // creates the array
            CreateCells(); // creates the cells
        }

        // creates cells and places them in the grid
        // private as only the constructor should call it
        private void CreateCells()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    cells[row, column] = new Cell(row, column);
                }
            }
        }

        // allows other classes to access a specific cell

        public Cell GetCell(int row, int column)
        {
            return cells[row, column];
        }

    }
}
