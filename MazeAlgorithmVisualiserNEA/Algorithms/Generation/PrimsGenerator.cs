using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public class PrimsGenerator : IMazeGenerator
    {
        private Random random = new Random();
        public void Generate(Maze maze)
        {
            Cell start = maze.GetCell(random.Next(maze.Rows), random.Next(maze.Columns));
            start.SetVisited(true);

            List<Cell> frontier = new List<Cell>();

            AddFrontierCells(maze, start, frontier);

            while (frontier.Count > 0)
            {
                // picks random frontier cell
                Cell current = frontier[random.Next(frontier.Count)];

                // finds visited neighbours
                List<Cell> visitedNeighbours = maze.GetVisitedNeighbours(current);

                if (visitedNeighbours.Count > 0)
                {
                    // connects to a random visited neighbour
                    Cell neighbour = visitedNeighbours[random.Next(visitedNeighbours.Count)];
                    maze.RemoveWallBetween(current, neighbour);

                }
                current.SetVisited(true);
                frontier.Remove(current);
                AddFrontierCells(maze, current, frontier);
            }
        }

        private void AddFrontierCells(Maze maze, Cell cell, List<Cell> frontier)
        {
            foreach (Cell neighbour in maze.GetUnvisitedNeighbours(cell))
            {
                if (frontier.Contains(neighbour) == false){
                    frontier.Add(neighbour);
                }
            }
        }
    }
}
