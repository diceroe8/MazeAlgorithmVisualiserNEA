using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.Rendering;
using MazeAlgorithmVisualiserNEA.Algorithms;
using MazeAlgorithmVisualiserNEA.Algorithms.Generation;
using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Converters;
using MazeAlgorithmVisualiserNEA.Algorithms.Solving;

namespace MazeAlgorithmVisualiserNEA
{
    public partial class MainForm : Form
    {
        private Maze maze;
        private MazeRenderer mazeRenderer;
        private ImperfectMazeGenerator generator;
        private Button generateButton;
        private GraphRenderer graphRenderer;
        private Graph graph;
        private MazeToGraphConverter converter;
        private PathRenderer pathRenderer;
        private DFS DFSsolver;
        private BFS BFSsolver;
        private PathfindingProcess DFSsolution;
        private PathfindingProcess BFSsolution;

        public MainForm()
        {
            InitializeComponent();

            // creates maze, generator, and renderer objects
            maze = new Maze(32, 32);
            generator = new ImperfectMazeGenerator();
            mazeRenderer = new MazeRenderer();
            graphRenderer = new GraphRenderer();
            converter = new MazeToGraphConverter();
            pathRenderer = new PathRenderer();
            DFSsolver = new DFS();
            BFSsolver = new BFS();


            //generate initial maze
            // generator.Generate(maze);

            // create button
            generateButton = new Button();
            generateButton.Text = "Generate maze";
            generateButton.Width = 150;
            generateButton.Dock = DockStyle.Right;
            generateButton.Click += GenerateButton_Click;

            // add button to form
            this.Controls.Add(generateButton);

            // paint event
            this.Paint += MainForm_Paint;


            Random rnd = new Random();


        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // regenerate maze

            Random rnd = new Random();
            maze = new Maze(32, 32);
            generator.Generate(maze);

            // choose start and end cells
            Cell start = maze.GetCell(rnd.Next(0, 32), rnd.Next(0, 32));
            Cell end = maze.GetCell(rnd.Next(0, 32), rnd.Next(0, 32));

            start.SetStart(true);
            end.SetEnd(true);

            graph = converter.Convert(maze);



            DFSsolution = DFSsolver.Solve(graph);
            BFSsolution = BFSsolver.Solve(graph);



            this.Invalidate();
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            int offsetX = 500;
            int offsetY = 100;

            e.Graphics.TranslateTransform(offsetX, offsetY);
            mazeRenderer.Draw(e.Graphics, maze);

            if (DFSsolution != null || BFSsolution != null)
            {
                pathRenderer.Draw(e.Graphics, BFSsolution.Path, Color.LimeGreen);
                pathRenderer.Draw(e.Graphics, DFSsolution.Path, Color.Gold);
                
            }
            


        }
    }
}



