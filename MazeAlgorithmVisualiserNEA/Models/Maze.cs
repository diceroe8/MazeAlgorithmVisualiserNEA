using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        // HELPER METHODS


        // allows other classes to access a specific cell

        public Cell GetCell(int row, int column)
        {
            if (!IsWithinBounds(row, column)) // makes sure a cell is within the maze before returning
            {
                throw new ArgumentOutOfRangeException("Cell position is outside the maze");
            }

            return cells[row, column];
        }

        // to check whether a position is within the maze boundaries
        public bool IsWithinBounds(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        // gets neighbour of a cell depending on direction
        public Cell GetNeighbour(Cell cell, Direction direction)
        {
            int row = cell.Row;
            int column = cell.Column;

            switch (direction)
            {
                case Direction.Left:
                    column--;
                    break;
                case Direction.Right:
                    column++;
                    break;
                case Direction.Bottom:
                    row++;
                    break;
                case Direction.Top:
                    row--;
                    break;
            }

            return GetCell(row, column);
        }

        // removes both walls between cells to make removing walls easier
        public void RemoveWallBetween(Cell first, Cell second)
        {
            if (first.Row == second.Column)
            {
                if (first.Column < second.Column)
                {
                    first.RemoveWall(Direction.Right); // walls must be removed from both directions
                    second.RemoveWall(Direction.Left);
                }
                else
                {
                    first.RemoveWall(Direction.Right);
                    second.RemoveWall(Direction.Left);
                }
            }
            else if (first.Column == second.Column)
            {
                if (first.Row < second.Row)
                {
                    first.RemoveWall(Direction.Bottom);
                    second.RemoveWall(Direction.Top);
                }
                else
                {
                    first.RemoveWall(Direction.Top);
                    second.RemoveWall(Direction.Bottom);
                }
            }
        }

        // gets a list of cell neighbours
        // useful for pathfinding algorithms
        public List<Cell> GetNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                Cell neighbour = GetNeighbour(cell, direction);

                neighbours.Add(neighbour);
            }

            return neighbours;
        }

    }
}
