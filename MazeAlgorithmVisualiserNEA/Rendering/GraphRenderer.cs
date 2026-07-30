using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.DataStructures;

namespace MazeAlgorithmVisualiserNEA.Rendering
{
    public class GraphRenderer
    {

        private int cellSize;
        private int nodeRadius;

        public GraphRenderer(int cellSize = 20, int nodeRadius = 1)
        {
            this.cellSize = cellSize;
            this.nodeRadius = nodeRadius;
        }

        public void Draw(Graphics graphics, Graph graph)
        {
            DrawEdges(graphics, graph);
            DrawNodes(graphics, graph);
        }

        public void DrawNodes(Graphics graphics, Graph graph)
        {
            foreach (GraphNode node in graph.Nodes)
            {
                int x = node.Cell.Column * cellSize + cellSize / 2;
                int y = node.Cell.Row * cellSize + cellSize / 2;

                graphics.FillEllipse(Brushes.Red, x - nodeRadius, y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
            }
        }

        public void DrawEdges(Graphics graphics, Graph graph)
        {
            foreach (GraphNode node in graph.Nodes)
            {
                int x1 = node.Cell.Column * cellSize + cellSize / 2;
                int y1 = node.Cell.Row * cellSize + cellSize / 2;

                foreach (GraphEdge edge in node.Edges)
                {
                    GraphNode destination = edge.Destination;

                    // Prevent drawing the same edge twice
                    if (destination.Cell.Row < node.Cell.Row)
                        continue;

                    if (destination.Cell.Row == node.Cell.Row &&
                        destination.Cell.Column < node.Cell.Column)
                        continue;

                    int x2 = destination.Cell.Column * cellSize + cellSize / 2;
                    int y2 = destination.Cell.Row * cellSize + cellSize / 2;

                    graphics.DrawLine(Pens.DodgerBlue, x1, y1, x2, y2);
                }
            }
        }
    }
}
