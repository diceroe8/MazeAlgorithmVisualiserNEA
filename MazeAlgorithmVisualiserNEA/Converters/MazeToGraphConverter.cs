using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Converters
{
    public class MazeToGraphConverter 
    {
        public Graph Convert(Maze maze)
        {
            Graph graph = new Graph();

            Dictionary<Cell, GraphNode> lookup = new Dictionary<Cell, GraphNode>();

            // FIRST PASS THROUGH MAZE
            // CREATES EACH NODE

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int col = 0; col < maze.Columns; col++)
                {
                    Cell cell = maze.GetCell(row, col);

                    GraphNode node = new GraphNode(cell);

                    graph.Nodes.Add(node);

                    lookup.Add(cell, node);

                    if (cell.IsStart)
                    {
                        graph.Start = node;
                    }

                    if (cell.IsEnd)
                    {
                        graph.End = node;
                    }
                }
            }

            // SECOND PASS THROUGH MAZE
            // CREATES EDGES BETWEEN NODES

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int col = 0; col < maze.Columns; col++)
                {
                    Cell currentCell = maze.GetCell(row, col);

                    GraphNode currentNode = lookup[currentCell];

                    if (!currentCell.TopWall && row > 0)
                    {
                        Cell neighbour = maze.GetCell(row - 1, col);

                        currentNode.AddEdge(lookup[neighbour]);
                    }

                    if (!currentCell.BottomWall && row < maze.Rows - 1)
                    {
                        Cell neighbour = maze.GetCell(row + 1, col);

                        currentNode.AddEdge(lookup[neighbour]);
                    }

                    if (!currentCell.LeftWall && col > 0)
                    {
                        Cell neighbour = maze.GetCell(row, col - 1);

                        currentNode.AddEdge(lookup[neighbour]);
                    }

                    if (!currentCell.RightWall && col < maze.Columns - 1)
                    {
                        Cell neighbour = maze.GetCell(row, col + 1);

                        currentNode.AddEdge(lookup[neighbour]);
                    }

                }
            }
            return graph;
        }
    }
}
