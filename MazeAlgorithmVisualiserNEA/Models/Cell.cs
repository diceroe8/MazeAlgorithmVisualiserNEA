using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Models
{
    public class Cell
    {
        // stores where the cells are within the grid of the maze
        // no setter as the cells should not move
        public int Row { get; }
        public int Column { get; }

        // stores whether a wall adjacent to the cell exists in each respective direction
        // setter to allow for maze generation algorithms
        public bool TopWall { get; private set;  }
        public bool BottomWall { get; private set; }
        public bool LeftWall { get; private set; }
        public bool RightWall { get; private set; }

        // this is for algorithms
        public bool Visited { get; private set; }

        // identify specific cells as start and end, useful for rendering
        public bool IsStart {  get; private set; }
        public bool IsEnd {  get; private set; }


        // constructor
        // constructs initially with solid walls before maze generation modifies them
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;

            TopWall = true;
            BottomWall = true;
            LeftWall = true;
            RightWall = true;

            Visited = false;
            IsStart = false;
            IsEnd = false;
        }

        // removes walls, used for maze generation algorithms
        public void RemoveWall(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    TopWall = false;
                    break;
                case Direction.Bottom:
                    BottomWall = false;
                    break;
                case Direction.Left:
                    LeftWall = false;
                    break;
                case Direction.Right:
                    RightWall = false;
                    break;
            }
        }


    }
}
