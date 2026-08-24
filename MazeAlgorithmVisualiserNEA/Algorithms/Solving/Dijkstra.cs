using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public class Dijkstra : IMazeSolver
    {

        public PathfindingProcess Solve(Graph graph)
        {
            // distance from start node to every other node
            Dictionary<GraphNode, int> distances = new Dictionary<GraphNode, int>();

            // stores previous node in shortest path
            Dictionary<GraphNode, GraphNode> parents = new Dictionary<GraphNode, GraphNode>();

            // already visited nodes
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // stores order in which nodes are visited
            List<GraphNode> visitedOrder = new List<GraphNode>();

            // priority queue to select node with smallest known distance
            PriorityQueue<GraphNode> queue = new PriorityQueue<GraphNode>();

            // sets initial distance of each node to infinity (basically)
            foreach (GraphNode node in graph.Nodes)
            {
                distances[node] = 999999;
            }

            // distance from start is 0
            distances[graph.Start] = 0;
            queue.Enqueue(graph.Start, 0);

            // continues till no nodes are left
            while (!queue.IsEmpty())
            {
                // get node with smallest distance
                GraphNode current = queue.Dequeue();

                // if already processed ignore
                if (visited.Contains(current))
                {
                    continue;
                }

                // marks node as visited
                visited.Add(current);
                visitedOrder.Add(current);

                // shortest path found
                if (current == graph.End)
                {
                    break;
                }

                // examines every edge connected to current
                foreach(GraphEdge edge in current.Edges)
                {
                    GraphNode neighbour = edge.Destination;

                    // ignore already processed
                    if (visited.Contains(neighbour))
                    {
                        continue;
                    }

                    // calculates distance from start to neighbour by travelling through current
                    int newDistance = distances[current] + edge.Cost;

                    // if route is shorter, update it
                    if (newDistance < distances[neighbour])
                    {
                        distances[neighbour] = newDistance;

                        parents[neighbour] = current;

                        queue.Enqueue(neighbour, newDistance);
                    }
                }
            }
            // if end never reached, there is no path
            if (!visited.Contains(graph.End))
            {
                return new PathfindingProcess(new List<GraphNode>(), visitedOrder);
            }

            // reconstruct shortest path
            List<GraphNode> path = new List<GraphNode>();
            GraphNode currentNode = graph.End;

            while (currentNode != graph.Start)
            {
                path.Add(currentNode);

                currentNode = parents[currentNode];
            }

            // add starting node
            path.Add(graph.Start);

            // reverse path
            path.Reverse();

            return new PathfindingProcess(path, visitedOrder);

        }



    }
}
