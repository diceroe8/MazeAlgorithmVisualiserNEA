using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public class ImperfectMazeGenerator : IMazeGenerator
    {
        private Random random = new Random();

        private double loopPercentage = 0.30;

        public void Generate(Maze maze)
        {
            KruskalsGenerator kruskal = new KruskalsGenerator();
            kruskal.Generate(maze);


            List<(Cell a, Cell b)> remainingWalls = new List<(Cell a, Cell b)>();

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int col = 0; col < maze.Columns; col++)
                {
                    Cell cell = maze.GetCell(row, col);

                    Cell? right = maze.GetNeighbour(cell, Direction.Right);

                    if (right != null && maze.HasWallBetween(cell, right))
                    {
                        remainingWalls.Add((cell, right));
                    }

                    Cell? bottom = maze.GetNeighbour(cell, Direction.Bottom);

                    if (right != null && maze.HasWallBetween(cell, bottom))
                    {
                        remainingWalls.Add((cell, bottom));
                    }

                }
            }

            Shuffle(remainingWalls);

            int wallsToRemove = (int)(remainingWalls.Count * loopPercentage);

            for (int i = 0; i < wallsToRemove; i++)
            {
                var (a, b) = remainingWalls[i];
                maze.RemoveWallBetween(a, b);
            }
        }


        private void Shuffle<T>(List<T> list)
        {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            (list[i], list[j]) = (list[j], list[i]);
        }
        }
    }
}


