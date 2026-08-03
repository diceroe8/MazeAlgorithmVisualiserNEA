using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public interface IMazeSolver
    {
        PathfindingProcess Solve(Graph graph);
    }
}
