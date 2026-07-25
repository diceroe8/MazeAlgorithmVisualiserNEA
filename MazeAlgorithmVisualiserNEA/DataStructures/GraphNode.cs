using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    public class GraphNode
    {

        public Cell Cell;
        public List<GraphEdge> Edges = new List<GraphEdge>();


        public GraphNode(Cell cell)
        {
            Cell = cell;
            Edges = new List<GraphEdge>();
        }
    }
}
