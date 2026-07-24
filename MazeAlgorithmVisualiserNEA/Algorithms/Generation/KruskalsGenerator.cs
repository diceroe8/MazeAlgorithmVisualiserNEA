using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public class KruskalsGenerator : IMazeGenerator
    {
        private Random random = new Random();

        public void Generate(Maze maze)
        {
            int totalCells = maze.Rows * maze.Columns;
            DisjointSet ds = new DisjointSet(totalCells);

            List<(Cell a, Cell b)> walls = new List<(Cell a, Cell b)>();

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int columns = 0; columns < maze.Columns; columns++)
                {
                    Cell cell = maze.GetCell(row, columns);

                    Cell? right = maze.GetNeighbour(cell, Direction.Right); // COULD BE NULL
                    if (right != null)
                    {
                        walls.Add((cell, right));
                    }

                    Cell? bottom = maze.GetNeighbour(cell, Direction.Bottom);
                    if (bottom != null)
                    {
                        walls.Add((cell, bottom));
                    }

                }
            }

            Shuffle(walls);

            foreach (var (a, b) in walls)
            {
                int idA = a.Row * maze.Columns + a.Column;
                int idB = b.Row * maze.Columns + b.Column;


                if (ds.Find(idA) != ds.Find(idB))
                {
                    maze.RemoveWallBetween(a, b);
                    ds.Union(idA, idB);

                }
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
