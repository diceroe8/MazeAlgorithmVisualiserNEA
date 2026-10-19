using MazeAlgorithmVisualiserNEA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MazeAlgorithmVisualiserNEA.DataStructures;


namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public class BFS : IMazeSolver
    {
        public PathfindingProcess Solve (Graph graph)
        {
            CustomQueue<GraphNode> queue = new CustomQueue<GraphNode>();
            HashSet<GraphNode> visited = new HashSet<GraphNode>();
            Dictionary<GraphNode, GraphNode> parents = new Dictionary<GraphNode, GraphNode>();
            List<GraphNode> visitedOrder = new List<GraphNode>();
            List<PathfindingStep> steps = new List<PathfindingStep>();

            queue.Enqueue(graph.Start);
            visited.Add(graph.Start);

            // continues until no nodes to visit
            while(!queue.IsEmpty())
            {
                // get next node
                GraphNode current = queue.Dequeue();
                visitedOrder.Add(current);

                // record information
                steps.Add(new PathfindingStep{CurrentNode = current, NodesExplored = visitedOrder.Count, QueueSize = queue.Count()});

                // stop at end
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
                        queue.Enqueue(neighbour);

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
