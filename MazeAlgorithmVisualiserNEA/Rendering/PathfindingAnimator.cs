using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Rendering
{
    public class PathfindingAnimator
    {
        private System.Windows.Forms.Timer timer;
        private PathfindingProcess process;

        private int currentIndex;
        private bool explorationFinished;
        private int cellSize = 20; // default

        public int CellSize
        {
            get { return cellSize; }
            set { cellSize = value; }
        }

        public int Interval
        {
            set { timer.Interval = value; }
        }

        public bool ExplorationFinished
        {
            get { return explorationFinished; }
        }

        public event EventHandler AnimationUpdated;
        public event EventHandler AnimationCompleted;

        public PathfindingAnimator()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
        }

        public void Animate(PathfindingProcess process)
        {
            timer.Stop();
            this.process = process;
            currentIndex = 0;
            explorationFinished = false;
            timer.Start();

            if (AnimationUpdated != null)
            {
                AnimationUpdated(this, EventArgs.Empty);
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (process == null)
            {
                timer.Stop();
                return;
            }

            if (currentIndex < process.VisitedNodes.Count)
            {
                currentIndex++;
                if (AnimationUpdated != null)
                {
                    AnimationUpdated(this, EventArgs.Empty);
                }
            }
            else
            {
                explorationFinished = true;
                timer.Stop();

                if (AnimationCompleted != null)
                {
                    AnimationCompleted(this, EventArgs.Empty);
                }
                if (AnimationUpdated != null)
                {
                    AnimationUpdated(this, EventArgs.Empty);
                }
            }
        }

        public void Draw(Graphics graphics, Color visitedColour, Color pathColour)
        {
            if (process == null)
            {
                return;
            }

            int limit = Math.Min(currentIndex, process.VisitedNodes.Count);
            using (Brush brush = new SolidBrush(visitedColour))
            {
                for (int i = 0; i < limit; i++)
                {
                    Point centre = GetCellCentre(process.VisitedNodes[i]);

                    graphics.FillRectangle(brush, centre.X - (cellSize / 4), centre.Y - (cellSize / 4), cellSize / 2, cellSize / 2);

                }
            }

            if (explorationFinished)
            {
                

                if (process.Path != null)
                {
                    using (Pen pen = new Pen(pathColour, 5))
                    {
                        for (int i = 0; i < process.Path.Count - 1; i++)
                        {
                            GraphNode current = process.Path[i];
                            GraphNode next = process.Path[i + 1];

                            Point start = GetCellCentre(current);
                            Point end = GetCellCentre(next);

                            graphics.DrawLine(pen, start, end);
                        }
                    }
                }
            }
        }

        public Point GetCellCentre(GraphNode node)
        {
            int x = (node.Cell.Column * cellSize) + (cellSize / 2);
            int y = (node.Cell.Row * cellSize) + (cellSize / 2);

            return new Point(x, y);
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            timer.Stop();
            process = null;
            currentIndex = 0;
            explorationFinished = false;

            if (AnimationUpdated != null)
            {
                AnimationUpdated(this, EventArgs.Empty);
            }
        }
    }
}




