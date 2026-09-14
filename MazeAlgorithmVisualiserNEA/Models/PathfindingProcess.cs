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
        public List<PathfindingStep> Steps;

        

        public PathfindingProcess(List<GraphNode> path, List<GraphNode> visitedNodes, List<PathfindingStep> steps)
        {
            Path = path;
            VisitedNodes = visitedNodes;
            Steps = steps;
        } 
    }
}
