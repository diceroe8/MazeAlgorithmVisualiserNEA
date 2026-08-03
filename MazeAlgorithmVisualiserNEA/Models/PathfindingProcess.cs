using MazeAlgorithmVisualiserNEA.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Models
{
    public class PathfindingProcess
    {
        public List<GraphNode> Path;

        public List<GraphNode> VisitedNodes;

        public PathfindingProcess(List<GraphNode> path, List<GraphNode> visitedNodes)
        {
            Path = path;
            VisitedNodes = visitedNodes;
        } 
    }
}
