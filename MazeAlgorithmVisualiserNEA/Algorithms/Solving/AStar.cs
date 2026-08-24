using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MazeAlgorithmVisualiserNEA.Algorithms.Solving
{
    public class AStar :IMazeSolver
    {
        public PathfindingProcess Solve(Graph graph)
        {

            // cost from start to each node
            Dictionary<GraphNode, int> gScore = new Dictionary<GraphNode, int>();

            // stores previous node in shortest path
            Dictionary<GraphNode, GraphNode> parents = new Dictionary<GraphNode, GraphNode>();

            // nodes that have already been processed
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // order in which nodes are visited
            List<GraphNode> visitedOrder = new List<GraphNode>();

            // priority queue
            PriorityQueue<GraphNode> queue = new PriorityQueue<GraphNode>();

            // set each nodes gScore to infinity
            foreach (GraphNode node in graph.Nodes)
            {
                gScore[node] = 999999;
            }

            // start node has cost of 0
            gScore[graph.Start] = 0;


            // heuristic

            int startHeuristic = Heuristic(graph.Start, graph.End);

            int startPriority = gScore[graph.Start] + startHeuristic;

            queue.Enqueue(graph.Start, startPriority);

            // continue till no nodes left
            while (!queue.IsEmpty())
            {

                // node with lowest heuristic
                GraphNode current = queue.Dequeue();

                // ignore already processed nodes
                if (visited.Contains(current))
                {
                    continue;
                }

                // mark as visited
                visited.Add(current);
                visitedOrder.Add(current);

                // end has been reached
                if (current == graph.End)
                {
                    break;
                }

                //examine neighbours
                foreach (GraphEdge edge in current.Edges)
                {
                    GraphNode neighbour = edge.Destination;

                    if (visited.Contains(neighbour))
                    {
                        continue;
                    }

                    // find new g score
                    int newGScore = gScore[current] + edge.Cost;

                    // if new route is better
                    if (newGScore < gScore[neighbour])
                    {
                        gScore[neighbour] = newGScore;

                        parents[neighbour] = current;

                        // calculate heuristic
                        int heuristic = Heuristic(neighbour, graph.End);

                        int fScore = newGScore + heuristic;

                        queue.Enqueue(neighbour, fScore);


                    }
                }
            }

            if (!visited.Contains(graph.End))
            {
                return new PathfindingProcess(new List<GraphNode>(), visitedOrder);
            }

            // reconstruct path
            List<GraphNode> path = new List<GraphNode>();

            GraphNode currentNode = graph.End;

            while (currentNode != graph.Start)
            {
                path.Add(currentNode);

                currentNode = parents[currentNode];
            }

            path.Add(graph.Start);
            path.Reverse();

            return new PathfindingProcess(path, visitedOrder);
        }

        // Manhattan distance heuristic
        private int Heuristic(GraphNode current, GraphNode goal)
        {
            int rowDistance = Math.Abs(current.Cell.Row - goal.Cell.Row);
            int columnDistance = Math.Abs(current.Cell.Column - goal.Cell.Column);
            return rowDistance + columnDistance;
        }
    }
}
