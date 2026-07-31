using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.DataStructures;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public interface IMazeSolver
    {
        List<GraphNode> Solve(Graph graph);
    }
}
