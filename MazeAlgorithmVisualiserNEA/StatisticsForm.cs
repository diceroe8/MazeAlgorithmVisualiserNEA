using MazeAlgorithmVisualiserNEA.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeAlgorithmVisualiserNEA
{
    public partial class StatisticsForm : Form
    {
        // statistics 
        private MazeStatistics mazeStatistics;
        private PathfindingStatistics statistics1;
        private PathfindingStatistics statistics2;

        // UI

        private TabControl tabControl;
        private TabPage mazeStatisticsTab;
        private TabPage pathfindingStatisticsTab;
        private Label mazeStatisticsLabel;
        private Label pathfindingStatisticsLabel;

        public StatisticsForm(MazeStatistics mazeStatistics, PathfindingStatistics statistics1, PathfindingStatistics statistics2)
        {
            InitializeComponent();

            this.mazeStatistics = mazeStatistics;
            this.statistics1 = statistics1;
            this.statistics2 = statistics2;

            Text = "Statistics";
            Size = new Size(900, 600);

            InitialiseUI();
        }

        private void InitialiseUI()
        {
            tabControl = new TabControl();

            tabControl.Dock = DockStyle.Fill;

            mazeStatisticsTab = new TabPage("Maze Statistics");

            pathfindingStatisticsTab = new TabPage("Pathfinding Statistics");

            mazeStatisticsLabel = new Label();

            mazeStatisticsLabel.AutoSize = true;

            mazeStatisticsLabel.Location = new Point(20, 20);

            mazeStatisticsLabel.Text =
                $"Rows: {mazeStatistics.Rows}\n\n" +
                $"Columns: {mazeStatistics.Columns}\n\n" +
                $"Cells: {mazeStatistics.TotalCells}\n\n" +
                $"Walls: {mazeStatistics.Walls}\n\n" +
                $"Passages: {mazeStatistics.Passages}\n\n" +
                $"Dead Ends: {mazeStatistics.DeadEnds}\n\n" +
                $"Junctions: {mazeStatistics.Junctions}\n\n" +
                $"Branching Factor: {mazeStatistics.BranchingFactor:F2}\n\n" +
                $"Average Corridor Length: {mazeStatistics.AverageCorridorLength:F2}\n\n" +
                $"Density: {mazeStatistics.MazeDensity:F2}";

            mazeStatisticsTab.Controls.Add(mazeStatisticsLabel);

            pathfindingStatisticsLabel = new Label();

            pathfindingStatisticsLabel.AutoSize = true;

            pathfindingStatisticsLabel.Location = new Point(20, 20);

            pathfindingStatisticsLabel.Text =
                $"Algorithm 1: {statistics1.AlgorithmName}\n\n" +
                $"Path Found: {statistics1.PathFound}\n" +
                $"Path Length: {statistics1.PathLength}\n" +
                $"Nodes Explored: {statistics1.NodesExplored}\n" +
                $"Execution Time: {statistics1.ExecutionTime:F3} ms\n\n\n" +

                $"Algorithm 2: {statistics2.AlgorithmName}\n\n" +
                $"Path Found: {statistics2.PathFound}\n" +
                $"Path Length: {statistics2.PathLength}\n" +
                $"Nodes Explored: {statistics2.NodesExplored}\n" +
                $"Execution Time: {statistics2.ExecutionTime:F3} ms";

            pathfindingStatisticsTab.Controls.Add(pathfindingStatisticsLabel);

            tabControl.TabPages.Add(mazeStatisticsTab);

            tabControl.TabPages.Add(pathfindingStatisticsTab);

            Controls.Add(tabControl);
        }
    }
}
