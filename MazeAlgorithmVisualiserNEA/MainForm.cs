using MazeAlgorithmVisualiserNEA.Algorithms;
using MazeAlgorithmVisualiserNEA.Algorithms.Generation;
using MazeAlgorithmVisualiserNEA.Algorithms.Solving;
using MazeAlgorithmVisualiserNEA.Converters;
using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.Rendering;
using Microsoft.VisualBasic.Devices;

namespace MazeAlgorithmVisualiserNEA
{
    public partial class MainForm : Form
    {
        // maze
        private Maze maze;
        private Graph graph;

        // UI
        private Button generateButton;
        private Button solveButton;
        private Button setStartButton;
        private Button setEndButton;
        private Button setRandomStartEndButton;
        private Button editWallsButton;
        private Button pauseButton;
        private Button stepButton;
        private Button resetAnimationButton;
        private Label mazeSizeLabel;
        private Label resultsLabel;
        private Label generationAlgorithmLabel;
        private Label algorithm1Label;
        private Label algorithm2Label;
        private Label rowsLabel;
        private Label columnsLabel;
        private Label tickSpeedInputLabel;
        private Label algorithm1InfoLabel;
        private Label algorithm2InfoLabel;

        private ComboBox generationAlgorithmComboBox;
        private ComboBox algorithm1ComboBox;
        private ComboBox algorithm2ComboBox;
        private NumericUpDown rowsInput;
        private NumericUpDown columnsInput;
        private NumericUpDown tickSpeedInput;

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
        bool editingWalls = false;


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

            // pathfinding 
            solver1 = null;
            solver2 = null; // CHANGE THESE TO CHANGE THE PATHFINDING ALGORITHM

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
            // generation algorithm label
            generationAlgorithmLabel = new Label();
            generationAlgorithmLabel.Text = "Generation Algorithm:";
            generationAlgorithmLabel.AutoSize = true;
            generationAlgorithmLabel.Location = new Point(ClientRectangle.Width - 260, 20);
            generationAlgorithmLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            generationAlgorithmLabel.Font = new Font("Arial", 10, FontStyle.Bold);

            // generation algorithm drop down
            generationAlgorithmComboBox = new ComboBox();
            generationAlgorithmComboBox.Size = new Size(220, 30);
            generationAlgorithmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            generationAlgorithmComboBox.Items.Add("Imperfect Maze");
            generationAlgorithmComboBox.Items.Add("Kruskal's");
            generationAlgorithmComboBox.Items.Add("Prim's");
            generationAlgorithmComboBox.Items.Add("Recursive Backtracker");
            generationAlgorithmComboBox.Items.Add("Recursive Division");
            generationAlgorithmComboBox.SelectedIndex = 0;
            generationAlgorithmComboBox.Location = new Point(generationAlgorithmLabel.Left, generationAlgorithmLabel.Bottom + 5);
            generationAlgorithmComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // maze size label
            mazeSizeLabel = new Label();
            mazeSizeLabel.Text = "Select maze size";
            mazeSizeLabel.AutoSize = true;
            mazeSizeLabel.Location = new Point(generationAlgorithmComboBox.Left, generationAlgorithmComboBox.Bottom + 12);
            mazeSizeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            mazeSizeLabel.Font = new Font("Arial", 10, FontStyle.Bold);

            // rows label
            rowsLabel = new Label();
            rowsLabel.Text = "Rows:";
            rowsLabel.AutoSize = true;
            rowsLabel.Location = new Point(mazeSizeLabel.Left, mazeSizeLabel.Bottom + 6);
            rowsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // rows input
            rowsInput = new NumericUpDown();
            rowsInput.Minimum = 5;
            rowsInput.Maximum = 128;
            rowsInput.Value = 32;
            rowsInput.Size = new Size(220, 30);
            rowsInput.Location = new Point(rowsLabel.Left, rowsLabel.Bottom + 3);
            rowsInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // columns label
            columnsLabel = new Label();
            columnsLabel.Text = "Columns:";
            columnsLabel.AutoSize = true;
            columnsLabel.Location = new Point(rowsInput.Left, rowsInput.Bottom + 8);
            columnsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // columns input
            columnsInput = new NumericUpDown();
            columnsInput.Minimum = 5;
            columnsInput.Maximum = 128;
            columnsInput.Value = 32;
            columnsInput.Size = new Size(220, 30);
            columnsInput.Location = new Point(columnsLabel.Left, columnsLabel.Bottom + 3);
            columnsInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // generate maze button
            generateButton = new Button();
            generateButton.Text = "Generate maze";
            generateButton.Size = new Size(220, 30);
            generateButton.Location = new Point(columnsInput.Left, columnsInput.Bottom + 12);
            generateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // algorithm 1 label
            algorithm1Label = new Label();
            algorithm1Label.Text = "Algorithm 1:";
            algorithm1Label.AutoSize = true;
            algorithm1Label.Location = new Point(generateButton.Left, generateButton.Bottom + 15);
            algorithm1Label.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            algorithm1Label.Font = new Font("Arial", 10, FontStyle.Bold);

            // algorithm 1 selection dropdown
            algorithm1ComboBox = new ComboBox();
            algorithm1ComboBox.Size = new Size(220, 30);
            algorithm1ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithm1ComboBox.Items.Add("BFS");
            algorithm1ComboBox.Items.Add("DFS");
            algorithm1ComboBox.Items.Add("Dijkstra");
            algorithm1ComboBox.Items.Add("A*");
            algorithm1ComboBox.SelectedIndex = 0;
            algorithm1ComboBox.Location = new Point(generateButton.Left, algorithm1Label.Bottom + 3);
            algorithm1ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // algorithm 2 label
            algorithm2Label = new Label();
            algorithm2Label.Text = "Algorithm 2:";
            algorithm2Label.AutoSize = true;
            algorithm2Label.Location = new Point(generateButton.Left, algorithm1ComboBox.Bottom + 10);
            algorithm2Label.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            algorithm2Label.Font = new Font("Arial", 10, FontStyle.Bold);

            // algorithm 2 selection dropdown
            algorithm2ComboBox = new ComboBox();
            algorithm2ComboBox.Size = new Size(220, 30);
            algorithm2ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithm2ComboBox.Items.Add("BFS");
            algorithm2ComboBox.Items.Add("DFS");
            algorithm2ComboBox.Items.Add("Dijkstra");
            algorithm2ComboBox.Items.Add("A*");
            algorithm2ComboBox.SelectedIndex = 3;
            algorithm2ComboBox.Location = new Point(generateButton.Left, algorithm2Label.Bottom + 3);
            algorithm2ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // set start button
            setStartButton = new Button();
            setStartButton.Text = "Set Start";
            setStartButton.Size = new Size(220, 30);
            setStartButton.Location = new Point(algorithm2ComboBox.Left, algorithm2ComboBox.Bottom + 15);
            setStartButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // set end button
            setEndButton = new Button();
            setEndButton.Text = "Set End";
            setEndButton.Size = new Size(220, 30);
            setEndButton.Location = new Point(setStartButton.Left, setStartButton.Bottom + 6);
            setEndButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // random start end button
            setRandomStartEndButton = new Button();
            setRandomStartEndButton.Text = "Random Start/End";
            setRandomStartEndButton.Size = new Size(220, 30);
            setRandomStartEndButton.Location = new Point(setEndButton.Left, setEndButton.Bottom + 6);
            setRandomStartEndButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // editing walls
            editWallsButton = new Button();
            editWallsButton.Text = "Edit walls";
            editWallsButton.Size = new Size(220, 30);
            editWallsButton.Location = new Point(setRandomStartEndButton.Left, setRandomStartEndButton.Bottom + 6);
            editWallsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // solve button
            solveButton = new Button();
            solveButton.Text = "Solve maze";
            solveButton.Size = new Size(220, 30);
            solveButton.Location = new Point(editWallsButton.Left, editWallsButton.Bottom + 15);
            solveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            solveButton.Enabled = false;

            // pause button
            pauseButton = new Button();
            pauseButton.Text = "Pause";
            pauseButton.Size = new Size(220, 30);
            pauseButton.Location = new Point(solveButton.Left, solveButton.Bottom + 6);
            pauseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pauseButton.Enabled = false;

            // step button
            stepButton = new Button();
            stepButton.Text = "Step";
            stepButton.Size = new Size(220, 30);
            stepButton.Location = new Point(pauseButton.Left, pauseButton.Bottom + 6);
            stepButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            stepButton.Enabled = false;

            // reset animation button
            resetAnimationButton = new Button();
            resetAnimationButton.Text = "Reset Animation";
            resetAnimationButton.Size = new Size(220, 30);
            resetAnimationButton.Location = new Point(stepButton.Left, stepButton.Bottom + 6);
            resetAnimationButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resetAnimationButton.Enabled = false;

            // pathfinding steps info for algorithm 1
            algorithm1InfoLabel = new Label();
            algorithm1InfoLabel.Text = "";
            algorithm1InfoLabel.AutoSize = false;
            algorithm1InfoLabel.Size = new Size(220, 75);
            algorithm1InfoLabel.Location = new Point(resetAnimationButton.Left, resetAnimationButton.Bottom + 10);
            algorithm1InfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            algorithm1InfoLabel.Font = new Font("Arial", 8, FontStyle.Regular);

            // pathfinding steps info for algorithm 2
            algorithm2InfoLabel = new Label();
            algorithm2InfoLabel.Text = "";
            algorithm2InfoLabel.AutoSize = false;
            algorithm2InfoLabel.Size = new Size(220, 110);
            algorithm2InfoLabel.Location = new Point(resetAnimationButton.Left, algorithm1InfoLabel.Bottom + 5);
            algorithm2InfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            algorithm2InfoLabel.Font = new Font("Arial", 8, FontStyle.Regular);

            // results label
            resultsLabel = new Label();
            resultsLabel.Text = "";
            resultsLabel.Size = new Size(220, 50);
            resultsLabel.AutoSize = true;
            resultsLabel.Location = new Point(algorithm2InfoLabel.Left, algorithm2InfoLabel.Bottom + 8);
            resultsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resultsLabel.Font = new Font("Arial", 9, FontStyle.Regular);

            // tick speed input label
            tickSpeedInputLabel = new Label();
            tickSpeedInputLabel.Text = "Select animation speed: 0 - 100";
            tickSpeedInputLabel.Size = new Size(220, 35);
            tickSpeedInputLabel.Location = new Point(resultsLabel.Left, resultsLabel.Bottom + 3);
            tickSpeedInputLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tickSpeedInputLabel.Font = new Font("Arial", 9.5f, FontStyle.Bold);

            // tick speed setter
            tickSpeedInput = new NumericUpDown();
            tickSpeedInput.Minimum = 1;
            tickSpeedInput.Maximum = 100;
            tickSpeedInput.Value = 5;
            tickSpeedInput.Size = new Size(220, 30);
            tickSpeedInput.Location = new Point(tickSpeedInputLabel.Left, tickSpeedInputLabel.Bottom);
            tickSpeedInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // controls collection ordered identically to initialization
            Controls.Add(generationAlgorithmLabel);
            Controls.Add(generationAlgorithmComboBox);
            Controls.Add(mazeSizeLabel);
            Controls.Add(rowsLabel);
            Controls.Add(rowsInput);
            Controls.Add(columnsLabel);
            Controls.Add(columnsInput);
            Controls.Add(generateButton);
            Controls.Add(algorithm1Label);
            Controls.Add(algorithm1ComboBox);
            Controls.Add(algorithm2Label);
            Controls.Add(algorithm2ComboBox);
            Controls.Add(setStartButton);
            Controls.Add(setEndButton);
            Controls.Add(setRandomStartEndButton);
            Controls.Add(editWallsButton);
            Controls.Add(solveButton);
            Controls.Add(pauseButton);
            Controls.Add(stepButton);
            Controls.Add(resetAnimationButton);
            Controls.Add(algorithm1InfoLabel);
            Controls.Add(algorithm2InfoLabel);
            Controls.Add(resultsLabel);
            Controls.Add(tickSpeedInputLabel);
            Controls.Add(tickSpeedInput);
        }
        private void InitialiseEvents()
        {
            generateButton.Click += GenerateButton_Click;

            setStartButton.Click += SetStartButton_Click;
            setEndButton.Click += SetEndButton_Click;
            setRandomStartEndButton.Click += RandomStartEndButton_Click;
            editWallsButton.Click += EditWallsButton_Click;

            solveButton.Click += SolveButton_Click;

            pauseButton.Click += PauseButton_Click;
            stepButton.Click += StepButton_Click;
            resetAnimationButton.Click += ResetAnimationButton_Click;

            Paint += MainForm_Paint;
            MouseClick += MainForm_MouseClick;

            animator1.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
            animator2.AnimationUpdated += PathfindingAnimator_AnimationUpdated;
        }


        // BUTTON CLICKS
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // reset previous animations
            animator1.Reset();
            animator2.Reset();

            pauseButton.Enabled = false;
            stepButton.Enabled = false;
            resetAnimationButton.Enabled = false;
            pauseButton.Text = "Pause";

            // clear results
            if (resultsLabel != null)
            {
                resultsLabel.Text = "";
            }

            // clear info label text
            algorithm1InfoLabel.Text = "";
            algorithm2InfoLabel.Text = "";

            // stop selecting start end
            settingStart = false;
            settingEnd = false;

            // sets maze size

            mazeRows = (int)rowsInput.Value;
            mazeColumns = (int)columnsInput.Value;

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

        private void EditWallsButton_Click(object sender, EventArgs e)
        {
            editingWalls = !editingWalls;
            settingStart = false; 
            settingEnd = false;

            if (editingWalls)
            {
                editWallsButton.Text = "Finish editing";
            }
            else
            {
                editWallsButton.Text = "Edit walls";
            }
        }
        private void SolveButton_Click(object sender, EventArgs e)
        {
            // reset animators 
            animator1.Reset();
            animator2.Reset();

            animator1.CellSize = cellSize;
            animator2.CellSize = cellSize;

            animator1.Interval = 101 - (int)tickSpeedInput.Value;
            animator2.Interval = 101 - (int)tickSpeedInput.Value;

            // run conversion and pathfinding
            ConvertMazeToGraph();
            RunPathfindingAlgorithms();

            // start animation
            StartAnimation(solution1, animator1);
            StartAnimation(solution2, animator2);
            pauseButton.Enabled = true;
            stepButton.Enabled = true;
            resetAnimationButton.Enabled = true;

            solveButton.Enabled = false;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!animator1.IsPaused)
            {
                animator1.Pause();
                animator2.Pause();
                pauseButton.Text = "Resume";
            }
            else
            {
                animator1.Resume();
                animator2.Resume();
                pauseButton.Text = "Pause";
            }
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            animator1.Step();
            animator2.Step();
        }

        private void ResetAnimationButton_Click(object sender, EventArgs e)
        {
            animator1.Reset();
            animator2.Reset();

            pauseButton.Text = "Pause";
            solveButton.Enabled = true;

            resultsLabel.Text = "";
            Invalidate();
        }


        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            // clicks dont do anything if not selecting start end and not editing walls
            if (settingStart == false && settingEnd == false && editingWalls == false)
            {
                return;
            }
            if (editingWalls)
            {
                ToggleWall(e.X, e.Y);
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

        // helper methods
        private void GenerateMaze(int rows, int columns)
        {
            mazeGenerator = GetMazeGenerator(generationAlgorithmComboBox.SelectedItem.ToString());
            maze = new Maze(rows, columns);
            mazeGenerator.Generate(maze);
        }
        private void ConvertMazeToGraph()
        {
            graph = converter.Convert(maze);
        }
        private void RunPathfindingAlgorithms()
        {
            solver1 = GetSolver(algorithm1ComboBox.SelectedItem.ToString());
            solver2 = GetSolver(algorithm2ComboBox.SelectedItem.ToString());

            solution1 = solver1.Solve(graph);
            solution2 = solver2.Solve(graph);
        }
        private void StartAnimation(PathfindingProcess process, PathfindingAnimator animator)
        {
            animator.Animate(process);
        }
        private IMazeSolver GetSolver(string algorithm)
        {
            switch (algorithm)
            {
                case "BFS":
                    return new BFS();
                case "DFS":
                    return new DFS();
                case "Dijkstra":
                    return new Dijkstra();
                case "A*":
                    return new AStar();
                default:
                    return new BFS();
            }
        }
        private IMazeGenerator GetMazeGenerator(string algorithm)
        {
            switch (algorithm)
            {
                case "Imperfect Maze":
                    return new ImperfectMazeGenerator();

                case "Kruskal's":
                    return new KruskalsGenerator();

                case "Prim's":
                    return new PrimsGenerator();

                case "Recursive Backtracker":
                    return new RecursiveBackTracker();

                case "Recursive Division":
                    return new RecursiveDivision();

                default:
                    return new ImperfectMazeGenerator();
            }
        }

        private void ToggleWall(int mouseX, int mouseY)
        {
            int offsetX = 20;
            int offsetY = 20;

            // convert mouse position to relative maze position
            int x = mouseX - offsetX;
            int y = mouseY - offsetY;

            // make sure click in inside maze
            if (x < 0 || y < 0 || x >= maze.Columns * cellSize || y >= maze.Rows * cellSize)
            {
                return;
            }

            int column = x / cellSize;
            int row = y / cellSize;

            Cell cell = maze.GetCell(row, column);

            int xRemainder = x % cellSize;
            int yRemainder = y % cellSize;

            // find which wall is closest to the click
            int distanceToVertical = Math.Min(xRemainder, cellSize - xRemainder);
            int distanceToHorizontal = Math.Min(yRemainder, cellSize - yRemainder);

            Direction direction;

            if(distanceToVertical < distanceToHorizontal)
            {
                if (xRemainder < cellSize / 2)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Bottom;
                }

            }
            else
            {
                if (yRemainder < cellSize / 2)
                {
                    direction = Direction.Top;
                }
                else
                {
                    direction = Direction.Bottom;
                }
            }

            Cell neighbour = maze.GetNeighbour(cell, direction);

            if (neighbour == null) // dont allow to edit outer walls
            {
                return;
            }

            if (maze.HasWallBetween(cell, neighbour))
            {
                maze.RemoveWallBetween(cell, neighbour);
            }
            else
            {
                maze.AddWallBetween(cell, neighbour);
            }

            Invalidate();


        }

        private void PathfindingAnimator_AnimationUpdated(object sender, EventArgs e)
        {

            UpdateAlgorithmInfo();
            Invalidate();

            if (animator1 != null && animator2 != null)
            {
                if (animator1.ExplorationFinished && animator2.ExplorationFinished)
                {
                    // make sure path exists
                    int algorithm1Length = solution1 != null && solution1.Path != null ? solution1.Path.Count : 0;
                    int algorithm2Length = solution2 != null && solution2.Path != null ? solution2.Path.Count : 0;

                    // stats display
                    resultsLabel.Text =
                            "END FOUND!\n\n" +
                            $"{algorithm1ComboBox.SelectedItem}: {algorithm1Length}\n" +
                            $"{algorithm2ComboBox.SelectedItem}: {algorithm2Length}";
                }
            }
        }

        private void UpdateAlgorithmInfo()
        {
            PathfindingStep step1 = animator1.CurrentStep;
            PathfindingStep step2 = animator2.CurrentStep;

            // Algorithm 1
            if (step1 != null)
            {
                algorithm1InfoLabel.Text = FormatAlgorithmInfo(
                    algorithm1ComboBox.SelectedItem.ToString(),
                    step1);
            }
            else
            {
                algorithm1InfoLabel.Text = "";
            }

            // Algorithm 2
            if (step2 != null)
            {
                algorithm2InfoLabel.Text = FormatAlgorithmInfo(
                    algorithm2ComboBox.SelectedItem.ToString(),
                    step2);
            }
            else
            {
                algorithm2InfoLabel.Text = "";
            }
        }

        private string FormatAlgorithmInfo(string algorithm, PathfindingStep step)
        {
            string currentNode =
                $"Current node: ({step.CurrentNode.Cell.Row}, {step.CurrentNode.Cell.Column})";

            string nodesExplored =
                $"Nodes explored: {step.NodesExplored}";

            switch (algorithm)
            {
                case "BFS":
                    return
                        currentNode + "\n" +
                        nodesExplored + "\n" +
                        $"Queue size: {step.QueueSize}";

                case "DFS":
                    return
                        currentNode + "\n" +
                        nodesExplored + "\n" +
                        $"Stack size: {step.StackSize}";

                case "Dijkstra":
                    return
                        currentNode + "\n" +
                        nodesExplored + "\n" +
                        $"Priority queue: {step.PriorityQueueSize}\n" +
                        $"Distance: {step.Distance}";

                case "A*":
                    return
                        currentNode + "\n" +
                        nodesExplored + "\n" +
                        $"Priority queue: {step.PriorityQueueSize}\n" +
                        $"g(n): {step.GScore}\n" +
                        $"h(n): {step.HScore}\n" +
                        $"f(n): {step.FScore}";

                default:
                    return currentNode + "\n" + nodesExplored;
            }
        }

        private void FitMazeToScreen()
        {
            int rightSideBuffer = 260;
            int topBottomBuffer = 40;

            int availableWidth = ClientRectangle.Width - rightSideBuffer;
            int availableHeight = ClientRectangle.Height - topBottomBuffer;

            int cellWidth = availableWidth / mazeColumns;
            int cellHeight = availableHeight / mazeRows;

            cellSize = Math.Min(cellWidth, cellHeight);

            if (cellSize < 5)
            {
                cellSize = 5;
            }
        }
    }
}



