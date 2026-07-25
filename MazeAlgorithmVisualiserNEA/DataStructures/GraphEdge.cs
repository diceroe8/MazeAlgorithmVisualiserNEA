using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    public class GraphEdge
    {

        public GraphNode Destination;

        public int Cost;

        public GraphEdge(GraphNode destination, int cost = 1)
        {
            Destination = destination;
            Cost = cost;
        }
    }
}
