using MazeAlgorithmVisualiserNEA.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeAlgorithmVisualiserNEA.Models
{
    public class PathfindingStep
    {
        public GraphNode CurrentNode { get; set; }
        public int NodesExplored { get; set; }
        // question mark as may be null for some algorithms
        public int? QueueSize { get; set; }
        public int? StackSize { get; set; }
        public int? PriorityQueueSize { get; set; }
        public int? Distance { get; set; }
        public int? GScore { get; set; }
        public int? HScore { get; set; }
        public int? FScore { get; set; }


    }
}
