using MazeAlgorithmVisualiserNEA.Models;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;


    namespace MazeAlgorithmVisualiserNEA.Rendering
    {
        public class MazeRenderer
        {
            private int cellSize = 30;


            public void Draw(Graphics graphics, Maze maze)
            {
                for (int row = 0; row < maze.Rows; row++)
                {
                    for (int column = 0; column < maze.Columns; column++)
                    {
                        Cell cell = maze.GetCell(row, column);

                        DrawCell(graphics, cell);
                    }
                }
            }


            private void DrawCell(Graphics graphics, Cell cell)
            {
                int x = cell.Column * cellSize;
                int y = cell.Row * cellSize;


                if (cell.TopWall)
                {
                    graphics.DrawLine(Pens.Black, x, y, x + cellSize, y);
                }


                if (cell.BottomWall)
                {
                    graphics.DrawLine(Pens.Black, x, y + cellSize, x + cellSize, y + cellSize);
                }


                if (cell.LeftWall)
                {
                    graphics.DrawLine(Pens.Black, x, y, x, y + cellSize);
                }


                if (cell.RightWall)
                {
                    graphics.DrawLine(Pens.Black, x + cellSize, y, x + cellSize, y + cellSize);
                }
            }
        }
    }

