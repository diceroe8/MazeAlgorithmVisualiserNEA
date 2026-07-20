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
        public MainForm()
        {
            InitializeComponent();

            maze = new Maze(64, 64);

            generator = new PrimsGenerator();

            generator.Generate(maze);
            renderer = new MazeRenderer();

            this.Paint += MainForm_Paint;
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



