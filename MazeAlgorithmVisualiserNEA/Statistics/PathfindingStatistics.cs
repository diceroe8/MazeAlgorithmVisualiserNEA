using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Statistics
{
    public class PathfindingStatistics
    {
        public string AlgorithmName;

        public int PathLength;

        public int NodesExplored;

        public double ExecutionTime;

        public bool PathFound;

        public PathfindingStatistics(
            string algorithmName,
            int pathLength,
            int nodesExplored,
            double executionTime,
            bool pathFound)
        {
            AlgorithmName = algorithmName;
            PathLength = pathLength;
            NodesExplored = nodesExplored;
            ExecutionTime = executionTime;
            PathFound = pathFound;
        }
    }
}