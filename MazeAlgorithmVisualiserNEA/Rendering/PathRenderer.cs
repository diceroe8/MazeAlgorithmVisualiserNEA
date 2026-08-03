using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.DataStructures;

namespace MazeAlgorithmVisualiserNEA.Rendering
{
    public class PathRenderer
    {
        private int cellSize = 20;

        public void Draw(Graphics graphics, List<GraphNode> path, Color pathColour )
        {
            if (path == null)
            {
                return;
            }

            using (Pen pen = new Pen(pathColour, 5))
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    GraphNode current = path[i];
                    GraphNode next = path[i + 1];

                    Point start = GetCellCentre(current);
                    Point end = GetCellCentre(next);

                    graphics.DrawLine(pen, start, end);
                }
            }
        }

        public Point GetCellCentre(GraphNode node)
        {
            int x = (node.Cell.Column * cellSize) + (cellSize / 2);
            int y = (node.Cell.Row * cellSize) + (cellSize / 2);

            return new Point(x, y);
        }

    }
}
