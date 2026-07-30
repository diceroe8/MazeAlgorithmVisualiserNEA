using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    public class Graph
    {

        public List<GraphNode> Nodes;

        public GraphNode Start;

        public GraphNode End; // used in the pathfinding algorithms

        public Graph()
        {
            Nodes = new List<GraphNode>();
        }

        public void AddNode(GraphNode node)
        {
            Nodes.Add(node);
        }

        public Dictionary<Cell, GraphNode> NodeLookup; // o(1)   rather than o(n) for future
    }
}

// implement as adjacency list not matrix as graph will definitely be sparse
