using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.Rendering;

namespace MazeAlgorithmVisualiserNEA
{
    public partial class MainForm : Form
    {
        private Maze maze;
        private MazeRenderer renderer;
        public MainForm()
        {
            InitializeComponent();

            maze = new Maze(10, 10);
            renderer = new MazeRenderer();

            this.Paint += MainForm_Paint;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw(e.Graphics, maze);
        }
    }
}



