using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Generation
{
    public interface IMazeGenerator
    {
        void Generate(Maze maze);
    }
}
