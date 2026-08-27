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
        private Button setStartButton;
        private Button setEndButton;
        private Button setRandomStartEndButton;


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
        private int mazeRows = 32;
        private int mazeColumns = 32;
        bool settingStart = false;
        bool settingEnd = false;

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

            // generate maze button
            generateButton = new Button();
            generateButton.Text = "Generate maze";
            generateButton.Size = new Size(120, 30);
            generateButton.Location = new Point(ClientRectangle.Width - 140, 20);
            generateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // set start button
            setStartButton = new Button();
            setStartButton.Text = "Set Start";
            setStartButton.Size = new Size(120, 30);
            setStartButton.Location = new Point(generateButton.Left, generateButton.Bottom + 10);
            setStartButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // set end button
            setEndButton = new Button();
            setEndButton.Text = "Set End";
            setEndButton.Size = new Size(100, 30);
            setEndButton.Location = new Point(setStartButton.Left, setStartButton.Bottom + 10);
            setEndButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // random start end button
            setRandomStartEndButton = new Button();
            setRandomStartEndButton.Text = "Random Start/End";
            setRandomStartEndButton.Size = new Size(120, 30);
            setRandomStartEndButton.Location = new Point(setEndButton.Left, setEndButton.Bottom + 10);
            setRandomStartEndButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // solve button
            solveButton = new Button();
            solveButton.Text = "Solve maze";
            solveButton.Size = new Size(100, 30);
            solveButton.Location = new Point(setRandomStartEndButton.Left, setRandomStartEndButton.Bottom + 10);
            solveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            solveButton.Enabled = false;

            // results label
            resultsLabel = new Label();
            resultsLabel.Text = "";
            resultsLabel.Size = new Size(150, 100);
            resultsLabel.Location = new Point(solveButton.Left, solveButton.Bottom + 30);
            resultsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resultsLabel.Font = new Font("Arial", 9, FontStyle.Regular);

            Controls.Add(generateButton);
            Controls.Add(solveButton);
            Controls.Add(resultsLabel);
            Controls.Add(setStartButton);
            Controls.Add(setEndButton);
            Controls.Add(setRandomStartEndButton);

        }

        private void InitialiseEvents()
        {
            generateButton.Click += GenerateButton_Click;

            setStartButton.Click += SetStartButton_Click;
            setEndButton.Click += SetEndButton_Click;
            setRandomStartEndButton.Click += RandomStartEndButton_Click;

            solveButton.Click += SolveButton_Click;

            Paint += MainForm_Paint;
            MouseClick += MainForm_MouseClick;

            animator1.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
            animator2.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
        }
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // reset previous animations
            animator1.Reset();
            animator2.Reset();

            // clear results
            if (resultsLabel != null)
            {
                resultsLabel.Text = "";
            }

            // stop selecting start end
            settingStart = false;
            settingEnd = false;

            // fits maze to size screen
            FitMazeToScreen();
            mazeRenderer.CellSize = cellSize;

            // generates maze
            GenerateMaze(mazeRows, mazeColumns);

            solveButton.Enabled = false; // start end has not been decided yet
            Invalidate();

        }

        private void SetStartButton_Click(object sender, EventArgs e)
        {
            settingStart = true;
            settingEnd = false;
        }

        private void SetEndButton_Click(object sender, EventArgs e)
        {
            settingStart = false;
            settingEnd = true;
        }

        private void RandomStartEndButton_Click(object sender, EventArgs e)
        {
            maze.SetRandomStartAndEnd(random);
            settingStart = false;
            settingEnd = false;
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

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            // clicks dont do anything if not selecting start end
            if (settingStart == false && settingEnd == false)
            {
                return;
            }

            // position of maze on form
            int offsetX = 20;
            int offsetY = 20;

            // convert mouse cordinates into relative maze coords
            int mazeX = e.X - offsetX;
            int mazeY = e.Y - offsetY;

            // make sure click is inside the maze
            if (mazeX < 0 || mazeY < 0 || mazeX >= maze.Columns * cellSize || mazeY >= maze.Rows * cellSize)
            {
                return;
            }

            // convert pixel coords into maze cell coords
            int column = mazeX / cellSize;
            int row = mazeY / cellSize;

            Cell selectedCell = maze.GetCell(row, column);

            if (settingStart)
            {
                if (selectedCell.IsEnd == false)
                {
                    maze.SetStart(selectedCell);
                }
                else
                {
                    return;
                }
            }

            else if (settingEnd)
            {
                if (selectedCell.IsStart == false)
                {
                    maze.SetEnd(selectedCell);
                }
                else
                {
                    return;
                }
            }

            // stop selecting
            settingStart = false;
            settingEnd = false;

            // enable solve
            solveButton.Enabled = maze.HasStartAndEnd();

            Invalidate();
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



