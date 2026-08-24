using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Statistics
{
    public class MazeAnalyser
    {
        public MazeStatistics Analyse(Maze maze)
        {
            int totalCells = CalculateTotalCells(maze);
            
            int deadEnds = CountDeadEnds(maze);

            int junctions = CountJunctions(maze);

            int passages = CalculatePassages(maze);

            int walls = CalculateWalls(maze);

            double branchingFactor = CalculateBranchingFactor(maze);

            double corridorLength = CalculateAverageCorridorLength(maze);

            double density = CalculateDensity(walls, passages);

            return new MazeStatistics(maze.Rows, maze.Columns, walls, totalCells, deadEnds, junctions, branchingFactor, corridorLength, density, passages);
        }

        private int CalculateTotalCells(Maze maze)
        {
            return maze.Rows * maze.Columns;
        }

        private int GetConnections(Cell cell)
        {
            int connections = 0;

            if (!cell.TopWall)
            {
                connections++;
            }

            if (!cell.BottomWall)
            {
                connections++;
            }

            if (!cell.LeftWall)
            {
                connections++;
            }

            if (!cell.RightWall)
            {
                connections++;
            }

            return connections;
        }

        private int CountDeadEnds(Maze maze)
        {
            int deadEnds = 0;

            for (int row = 0; row <  maze.Rows; row++)
            {
                for (int col = 0; col < maze.Columns; col++)
                {
                    Cell cell = maze.GetCell(row, col);
                    if (GetConnections(cell) == 1)
                    {
                        deadEnds++;
                    }
                }
            }

            return deadEnds;
        }

        private int CountJunctions(Maze maze)
        {
            int junctions = 0;

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int col = 0; col < maze.Columns; col++)
                {
                    Cell cell = maze.GetCell(row, col);
                    
                    if (GetConnections(cell) >= 3)
                    {
                        junctions++;
                    }
                }
            }
            return junctions;
        }

        private int CalculatePassages(Maze maze)
        {
            int passages = 0;

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int column = 0; column < maze.Columns; column++)
                {
                    Cell cell = maze.GetCell(row, column);

                    Cell? right = maze.GetNeighbour(cell, Direction.Right);

                    if (right != null && !cell.RightWall)
                    {
                        passages++;
                    }

                    // Only check bottom connections
                    // to avoid counting the same passage twice
                    Cell? bottom = maze.GetNeighbour(cell, Direction.Bottom);

                    if (bottom != null && !cell.BottomWall)
                    {
                        passages++;
                    }
                }
            }

            return passages;
        }


        private int CalculateWalls(Maze maze)
        {
            int walls = 0;

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int column = 0; column < maze.Columns; column++)
                {
                    Cell cell = maze.GetCell(row, column);

                    // Outer top wall
                    if (row == 0 && cell.TopWall)
                    {
                        walls++;
                    }

                    // Outer left wall
                    if (column == 0 && cell.LeftWall)
                    {
                        walls++;
                    }

                    // Internal/right wall
                    Cell? right = maze.GetNeighbour(cell, Direction.Right);

                    if (right != null && cell.RightWall)
                    {
                        walls++;
                    }

                    // Internal/bottom wall
                    Cell? bottom = maze.GetNeighbour(cell, Direction.Bottom);

                    if (bottom != null && cell.BottomWall)
                    {
                        walls++;
                    }

                    // Outer bottom wall
                    if (row == maze.Rows - 1 && cell.BottomWall)
                    {
                        walls++;
                    }

                    // Outer right wall
                    if (column == maze.Columns - 1 && cell.RightWall)
                    {
                        walls++;
                    }
                }
            }

            return walls;
        }


        private double CalculateBranchingFactor(Maze maze)
        {
            int totalBranches = 0;
            int junctions = 0;

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int column = 0; column < maze.Columns; column++)
                {
                    Cell cell = maze.GetCell(row, column);

                    int connections = GetConnections(cell);

                    if (connections >= 3)
                    {
                        totalBranches += connections;
                        junctions++;
                    }
                }
            }

            if (junctions == 0)
            {
                return 0;
            }

            return (double)totalBranches / junctions;
        }


        private double CalculateAverageCorridorLength(Maze maze)
        {
            HashSet<Cell> visited = new HashSet<Cell>();

            List<int> corridorLengths = new List<int>();

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int column = 0; column < maze.Columns; column++)
                {
                    Cell start = maze.GetCell(row, column);

                    // A corridor consists of cells with exactly
                    // two possible directions.
                    if (GetConnections(start) != 2)
                    {
                        continue;
                    }

                    if (visited.Contains(start))
                    {
                        continue;
                    }

                    int length = TraceCorridor(maze, start, visited);

                    if (length > 0)
                    {
                        corridorLengths.Add(length);
                    }
                }
            }

            if (corridorLengths.Count == 0)
            {
                return 0;
            }

            int totalLength = 0;

            foreach (int length in corridorLengths)
            {
                totalLength += length;
            }

            return (double)totalLength / corridorLengths.Count;
        }


        // Follows a corridor until it reaches a junction,
        // dead end, or an already visited corridor cell.
        private int TraceCorridor(
            Maze maze,
            Cell start,
            HashSet<Cell> visited)
        {
            Cell current = start;
            Cell? previous = null;

            int length = 0;

            while (current != null &&
                   GetConnections(current) == 2 &&
                   !visited.Contains(current))
            {
                visited.Add(current);

                length++;

                List<Cell> neighbours = GetOpenNeighbours(maze, current);

                Cell? next = null;

                foreach (Cell neighbour in neighbours)
                {
                    if (neighbour != previous)
                    {
                        next = neighbour;
                        break;
                    }
                }

                previous = current;
                current = next;
            }

            return length;
        }


        // Gets only the cells which can actually be reached
        // through an open passage.
        private List<Cell> GetOpenNeighbours(Maze maze, Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            if (!cell.TopWall)
            {
                Cell? neighbour = maze.GetNeighbour(cell, Direction.Top);

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            if (!cell.BottomWall)
            {
                Cell? neighbour = maze.GetNeighbour(cell, Direction.Bottom);

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            if (!cell.LeftWall)
            {
                Cell? neighbour = maze.GetNeighbour(cell, Direction.Left);

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            if (!cell.RightWall)
            {
                Cell? neighbour = maze.GetNeighbour(cell, Direction.Right);

                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }


        // Calculates the proportion of walls compared
        // with walls + passages.
        private double CalculateDensity(int walls, int passages)
        {
            int total = walls + passages;

            if (total == 0)
            {
                return 0;
            }

            return (double)walls / total;
        }
    }
}

