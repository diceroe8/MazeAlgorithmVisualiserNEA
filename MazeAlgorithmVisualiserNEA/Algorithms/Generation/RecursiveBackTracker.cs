using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.DataStructures;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public class RecursiveBackTracker : IMazeGenerator
    {
        private Random random;

        public RecursiveBackTracker()
        {
            random = new Random();
        }
        public void Generate(Maze maze)
        {


            CustomStack<Cell> stack = new CustomStack<Cell>();

            Cell start = maze.GetCell(0, 0);

            start.SetVisited(true);

            stack.Push(start);

            while (stack.IsEmpty() == false)
            {
                Cell current = stack.Peek();

                List<Cell> neighbours = maze.GetUnvisitedNeighbours(current);

                if (neighbours.Count > 0)
                {
                    Cell focus = neighbours[random.Next(neighbours.Count)];

                    maze.RemoveWallBetween(current, focus);

                    focus.SetVisited(true);

                    stack.Push(focus);
                }
                else
                {
                    stack.Pop();
                }
            }

        }


    }
}
