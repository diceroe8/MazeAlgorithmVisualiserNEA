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
        private KruskalsGenerator generator;
        private Button generateButton;
        public MainForm()
        {
            InitializeComponent();

            // creates maze, generator, and renderer objects
            maze = new Maze(32, 32);
            generator = new KruskalsGenerator();
            renderer = new MazeRenderer();

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
            maze.GetCell(rnd.Next(0, 32), rnd.Next(0, 32)).SetStart(true);
            maze.GetCell(rnd.Next(0, 32), rnd.Next(0, 32)).SetEnd(true);

            // fill all cells


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



