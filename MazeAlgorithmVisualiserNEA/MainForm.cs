using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.Rendering;
using MazeAlgorithmVisualiserNEA.Algorithms;
using MazeAlgorithmVisualiserNEA.Algorithms.Generation;

namespace MazeAlgorithmVisualiserNEA
{
    public partial class MainForm : Form
    {
        private Maze maze;
        private MazeRenderer renderer;
        private PrimsGenerator generator;
        private Button generateButton;
        public MainForm()
        {
            InitializeComponent();

            // creates maze, generator, and renderer objects
            maze = new Maze(16, 16);
            generator = new PrimsGenerator();
            renderer = new MazeRenderer();

            //generate initial maze
            generator.Generate(maze);

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
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // regenerate maze
            maze = new Maze(16, 16);
            generator.Generate(maze);

            //redraw maze
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
            renderer.Draw(e.Graphics, maze);
        }
    }
}



