using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.DataStructures;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public class DFS : IMazeSolver
    {
        public PathfindingProcess Solve(Graph graph)
        {
            // underlying data structures
            CustomStack<GraphNode> stack = new CustomStack<GraphNode>();
            HashSet<GraphNode> visited = new HashSet<GraphNode>();
            Dictionary<GraphNode, GraphNode> parents = new Dictionary<GraphNode, GraphNode>();
            List<GraphNode> visitedOrder = new List<GraphNode>();
            List<PathfindingStep> steps = new List<PathfindingStep>(); // to display information

            // start at the start node

            stack.Push(graph.Start);
            visited.Add(graph.Start);

            // continues until there are no more nodes to visit
            while(!stack.IsEmpty())
            {
                // get next node
                GraphNode current = stack.Pop();
                visitedOrder.Add(current);
                steps.Add(new PathfindingStep{CurrentNode = current, NodesExplored = visitedOrder.Count, StackSize = stack.Count()});

                // stop if we reach end
                if (current == graph.End)
                {
                    break;
                }

                foreach (GraphEdge edge in current.Edges)
                {
                    GraphNode neighbour = edge.Destination;

                    if (!visited.Contains(neighbour))
                    {
                        visited.Add(neighbour);

                        parents.Add(neighbour, current);

                        stack.Push(neighbour);
                    }
                }
            }

            if (!visited.Contains(graph.End))
            {
                return new PathfindingProcess(new List<GraphNode>(), visitedOrder, steps);
            }

            List<GraphNode> path = new List<GraphNode>();

            GraphNode node = graph.End;

            while (node != graph.Start)
            {
                path.Add(node);
                node = parents[node];
            }
            path.Add(graph.Start);
            path.Reverse();
            return new PathfindingProcess(path, visitedOrder, steps);
        }
    }
}
