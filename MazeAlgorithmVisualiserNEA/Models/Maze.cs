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
        public Cell? GetNeighbour(Cell cell, Direction direction)
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
            if (IsWithinBounds(row, column))
            {
                return GetCell(row, column);
            }
            return null;

        }

        // removes both walls between cells to make removing walls easier
        public void RemoveWallBetween(Cell first, Cell second)
        {
            if (first.Row == second.Row)
            {
                if (first.Column < second.Column)
                {
                    first.RemoveWall(Direction.Right); // walls must be removed from both directions
                    second.RemoveWall(Direction.Left);
                }
                else
                {
                    first.RemoveWall(Direction.Left);
                    second.RemoveWall(Direction.Right);
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

        public void AddWallBetween(Cell first, Cell second) // mainly for recursive division
        {
            if (first.Row == second.Row)
            {
                if (first.Column < second.Column)
                {
                    first.AddWall(Direction.Right);
                    second.AddWall(Direction.Left);
                }
                else
                {
                    first.AddWall(Direction.Left);
                    second.AddWall(Direction.Right);
                }
            }
            else if (first.Column == second.Column)
            {
                if (first.Row < second.Row)
                {
                    first.AddWall(Direction.Bottom);
                    second.AddWall(Direction.Top);
                }
                else
                {
                    first.AddWall(Direction.Top);
                    second.AddWall(Direction.Bottom);
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
                Cell? neighbour = GetNeighbour(cell, direction);

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        // gets a list of unvisited neighbours
        public List<Cell> GetUnvisitedNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                Cell neighbour = GetNeighbour(cell, direction);

                if (neighbour!= null && neighbour.Visited == false)
                {
                    neighbours.Add(neighbour);
                }
            }
            
            return neighbours;
        }
        
        public List<Cell> GetVisitedNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                Cell? neighbour = GetNeighbour(cell, direction);
                if (neighbour != null && neighbour.Visited == true)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        public void ClearAllWalls()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    cells[row, col].ClearWall();
                }
            }
        }




    }
}
