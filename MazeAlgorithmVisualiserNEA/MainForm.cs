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

        // buttons and labels
        private Button generateButton;
        private Button solveButton;
        private Label resultsLabel;

        // generators
        private IMazeGenerator mazeGenerator;
        private ImperfectMazeGenerator generator;
        private KruskalsGenerator kruskalsGenerator;
        private PrimsGenerator primsGenerator;
        private RecursiveBackTracker recursiveBackTrackerGenerator;
        private RecursiveDivision recursiveDivisionGenerator;

        // solvers and animators
        private IMazeSolver solver1;
        private IMazeSolver solver2;
        private PathfindingProcess solution1;
        private PathfindingProcess solution2;
        private PathfindingAnimator animator1;
        private PathfindingAnimator animator2;


        // renderers
        private MazeRenderer mazeRenderer;
        private GraphRenderer graphRenderer;
        private PathRenderer pathRenderer;

        // converters
        private MazeToGraphConverter converter;

        // other
        private Random random;
        private int cellSize;
        private int mazeRows = 128;
        private int mazeColumns = 128;

        // constructor
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // stops lagging
            InitialiseObjects();
            InitialiseUI();
            InitialiseEvents();
        }

        private void InitialiseObjects()
        {
            // number generator
            random = new Random();

            // maze
            maze = new Maze(10, 10);

            // maze generator
            mazeGenerator = new ImperfectMazeGenerator();

            // pathfinding 
            solver1 = new BFS();
            solver2 = new AStar(); // CHANGE THESE TO CHANGE THE PATHFINDING ALGORITHM

            // converter
            converter = new MazeToGraphConverter();
            
            // renderers
            mazeRenderer = new MazeRenderer();
            graphRenderer = new GraphRenderer();
            pathRenderer = new PathRenderer();

            // animators
            animator1 = new PathfindingAnimator();
            animator2 = new PathfindingAnimator();

            // animation speed
            animator1.Interval = 1;
            animator2.Interval = 1;
        }

        private void InitialiseUI()
        {
            generateButton = new Button();
            generateButton.Text = "Generate maze";
            generateButton.Size = new Size(100, 30);
            generateButton.Location = new Point(ClientRectangle.Width - 140, 20);
            generateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            solveButton = new Button();
            solveButton.Text = "Solve maze";
            solveButton.Size = new Size(100, 30);
            solveButton.Location = new Point(generateButton.Left, generateButton.Bottom + 10);
            solveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            solveButton.Enabled = false; 

            resultsLabel = new Label();
            resultsLabel.Text = ""; 
            resultsLabel.Size = new Size(150, 100); 
            resultsLabel.Location = new Point(generateButton.Left, solveButton.Bottom + 30);
            resultsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resultsLabel.Font = new Font("Arial", 9, FontStyle.Regular);

            Controls.Add(generateButton);
            Controls.Add(solveButton);
            Controls.Add(resultsLabel);
        }

        private void InitialiseEvents()
        {
            generateButton.Click += GenerateButton_Click;
            solveButton.Click += SolveButton_Click;

            Paint += MainForm_Paint;

            animator1.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
            animator2.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
        }
        private void GenerateButton_Click(object sender, EventArgs e)
        {

            animator1.Reset();
            animator2.Reset();
            
            if (resultsLabel != null)
            {
                resultsLabel.Text = "";
            }

            FitMazeToScreen();
            mazeRenderer.CellSize = cellSize;

            GenerateMaze(mazeRows, mazeColumns);
            maze.SetRandomStartAndEnd(random);

            solveButton.Enabled = true;
            Invalidate();
            

        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            // reset animators 
            animator1.Reset();
            animator2.Reset();

            animator1.CellSize = cellSize;
            animator2.CellSize = cellSize;

            // run conversion and pathfinding
            ConvertMazeToGraph();
            RunPathfindingAlgorithms();

            // start animation
            StartAnimation(solution1, animator1);
            StartAnimation(solution2, animator2);

            solveButton.Enabled = false;
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
            solution1 = solver1.Solve(graph);
            solution2 = solver2.Solve(graph);
        }

        private void StartAnimation(PathfindingProcess process, PathfindingAnimator animator)
        {
            animator.Animate(process);
        }

        private void PathfindingAnimator_AnimationUpdated(object sender, EventArgs e)
        {
            Invalidate();

            if (animator1 != null && animator2 != null)
            {
                if (animator1.ExplorationFinished && animator2.ExplorationFinished)
                {
                    // make sure path exists
                    int dfsLength = solution1 != null && solution1.Path != null ? solution1.Path.Count : 0;
                    int aStarLength = solution2 != null && solution2.Path != null ? solution2.Path.Count : 0;

                    // stats display
                    resultsLabel.Text = "END FOUND!\n\n" + $"A1 Path Length: {dfsLength}\n" + $"A2 Path Length: {aStarLength}";
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            int mazeWidth = maze.Columns * cellSize; // calculate total maze width

            int paddingRight = 20;
            int paddingDown = 20;

            int offsetX = 20;
            // int offsetX = ClientRectangle.Width - generateButton.Width - mazeWidth - paddingRight;
            int offsetY = paddingDown;

            e.Graphics.TranslateTransform(offsetX, offsetY);
            mazeRenderer.Draw(e.Graphics, maze);

            if (animator1 != null)
            {
                animator1.Draw(e.Graphics, Color.Black, Color.Red);
            }

            if (animator2 != null)
            {
                animator2.Draw(e.Graphics, Color.DarkBlue, Color.Green);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FitMazeToScreen()
        {
            int rightSideBuffer = 180;
            int topBottomBuffer = 40;

            int availableWidth = ClientRectangle.Width - rightSideBuffer;
            int availableHeight = ClientRectangle.Height - topBottomBuffer;

            int cellWidth = availableWidth / mazeColumns;
            int cellHeight = availableHeight / mazeColumns;

            cellSize = Math.Min(cellWidth, cellHeight);

            if (cellSize < 5)
            {
                cellSize = 5;
            }
             
        }

        
    }
}



