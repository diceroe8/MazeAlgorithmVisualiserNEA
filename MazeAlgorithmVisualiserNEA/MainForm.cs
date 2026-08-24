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
        // maze
        private Maze maze;
        private Graph graph;


        // buttons
        private Button generateButton;

        // generators

        private IMazeGenerator mazeGenerator;
        private ImperfectMazeGenerator generator;
        private KruskalsGenerator kruskalsGenerator;
        private PrimsGenerator primsGenerator;
        private RecursiveBackTracker recursiveBackTrackerGenerator;
        private RecursiveDivision recursiveDivisionGenerator;

        // solvers
        private IMazeSolver dfsSolver;
        private IMazeSolver bfsSolver;
        private IMazeSolver dijkstraSolver;
        private IMazeSolver aStarSolver;


        // pathfinding results
        private PathfindingProcess dfsSolution;
        private PathfindingProcess bfsSolution;
        private PathfindingProcess dijkstraSolution;
        private PathfindingProcess aStarSolution;




        // renderers
        private MazeRenderer mazeRenderer;
        private GraphRenderer graphRenderer;
        private PathRenderer pathRenderer;
        private PathfindingAnimator pathfindingAnimator;

        // converters
        private MazeToGraphConverter converter;

        // other
        private Random random;




        // constructor
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitialiseObjects();
            InitialiseUI();
            InitialiseEvents();
        }

        private void InitialiseObjects()
        {
            // number generator
            random = new Random();


            // maze
            maze = new Maze(32, 32);


            // maze generator
            mazeGenerator = new ImperfectMazeGenerator();


            // pathfinding 
            dfsSolver = new DFS();
            bfsSolver = new BFS();
            dijkstraSolver = new Dijkstra();
            aStarSolver = new AStar();


            // converter
            converter = new MazeToGraphConverter();


            // renderers
            mazeRenderer = new MazeRenderer();
            graphRenderer = new GraphRenderer();
            pathRenderer = new PathRenderer();


            // animator
            pathfindingAnimator = new PathfindingAnimator();

            // animation speed
            pathfindingAnimator.Interval = 50;
        }

        private void InitialiseUI()
        {
            generateButton = new Button();
            generateButton.Text = "Generate maze";
            generateButton.Width = 150;
            generateButton.Dock = DockStyle.Right;

            Controls.Add(generateButton);
        }

        private void InitialiseEvents()
        {
            generateButton.Click += GenerateButton_Click;

            Paint += MainForm_Paint;

            pathfindingAnimator.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
        }


        private void GenerateButton_Click(object sender, EventArgs e)
        {

            pathfindingAnimator.Reset();
            GenerateMaze(16, 16);
            maze.SetRandomStartAndEnd(random);
            ConvertMazeToGraph();
            RunPathfindingAlgorithms();
            StartAnimation(aStarSolution);
        }

        private void GenerateMaze(int rows, int columns)
        {
            maze = new Maze(rows, columns);
            mazeGenerator.Generate(maze);
        }

        private void ConvertMazeToGraph()
        {
            graph = converter.Convert(maze);
        }

        private void RunPathfindingAlgorithms()
        {
            dfsSolution = dfsSolver.Solve(graph);

            bfsSolution = bfsSolver.Solve(graph);

            dijkstraSolution = dijkstraSolver.Solve(graph);

            aStarSolution = aStarSolver.Solve(graph);
        }

        private void StartAnimation(PathfindingProcess process)
        {
            pathfindingAnimator.Animate(process);
        }

        private void PathfindingAnimator_AnimationUpdated(object sender, EventArgs e)
        {
            Invalidate();
        }



        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            int offsetX = 500;
            int offsetY = 100;

            e.Graphics.TranslateTransform(offsetX, offsetY);
            mazeRenderer.Draw(e.Graphics, maze);

            if (pathfindingAnimator != null)
            {
                pathfindingAnimator.Draw(e.Graphics, Color.LightBlue, Color.Red);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}



