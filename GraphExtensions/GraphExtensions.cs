using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphExtensions
{
    public static class GraphExtensions
    {
        public static IEnumerable<T> FindPath<T>(this Graph<T> graph, T start, T end)
            where T : IAdjacent<T>
        {
            Dictionary<T, T> path = new Dictionary<T, T>();
            path[start] = default;
            var queue = new Queue<T>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                var element = queue.Dequeue();

                var nextElements = element
                    .AllAdjacentElements()
                    .Where(g => !path.ContainsKey(g));
                foreach (var nextElement in nextElements)
                {
                    path[nextElement] = element;
                    queue.Enqueue(nextElement);
                }
                if (path.ContainsKey(end)) break;
            }
            var target = end;
            while (target != null)
            {
                yield return target;
                target = path[target];               
            }
        }
        public static void CreateGraph<T>(this Graph<T> graph, T start)
            where T : IAdjacent<T>
        {
            var queue = new Queue<T>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                var element = queue.Dequeue();

                var nextElements = element
                    .AllAdjacentElements()
                    .Where(g => !(graph.HasNode(g)));
                foreach (var nextElement in nextElements)
                {
                    graph.AddEdge(element, nextElement,0);
                    queue.Enqueue(nextElement);
                }
            }
        }
        public static bool HasCycle<T>(this Graph<T> graph)
        {
            return GraphTarjanAlgorithm.TarjanAlgorithm(graph) == null;
        }
        public static List<List<T>> FindConnectedComponents<T>(this Graph<T> graph)
        //finds all components of undirected graph, constructed from directed graph
        {
            var result = new List<List<T>>();
            var markedNodes = new HashSet<Node>();
            while (true)
            {
                var nextNode = graph.GetAllNodes()
                                    .Where(node => !markedNodes.Contains(node))
                                    .FirstOrDefault();
                if (nextNode == null) break;

                var breadthSearch = graph.BreadthSearch(graph.GetObjectByNode(nextNode))
                                         .Select(e=>graph.GetNodeFromDictionary(e))
                                         .ToList();
                result.Add(breadthSearch.Select(e=>graph.GetObjectByNode(e)).ToList());
                foreach (var node in breadthSearch)
                    markedNodes.Add(node);
            }
            return result;
        }
        public static IEnumerable<T> BreadthSearch<T>(this Graph<T> graph, T start)
        {
            var visited = new HashSet<Node>();
            var queue = new Queue<Node>();
            Node startNode = graph.GetNodeFromDictionary(start);
            queue.Enqueue(startNode);
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                if (visited.Contains(node)) continue;
                visited.Add(node);
                yield return graph.GetObjectByNode(node);

                foreach (var incidentEdge in node.outgoingEdges)
                {
                    queue.Enqueue(incidentEdge.To);
                }
            }
        }

        public static IEnumerable<T> DepthSearch<T>(this Graph<T> graph, T start)
        {
            var visited = new HashSet<Node>();
            var stack = new Stack<Node>();
            Node startNode = graph.GetNodeFromDictionary(start);
            stack.Push(startNode);
            while (stack.Count!=0)
            {
                var node = stack.Pop();
                if (visited.Contains(node)) continue;
                visited.Add(node);
                yield return graph.GetObjectByNode(node);

                foreach (var incidentEdge in node.outgoingEdges)
                {
                    stack.Push(incidentEdge.To);
                }
            }
        }
        public static List<T> Dejkstra<T>(this Graph<T> graph, T start, T end)
        {
            Node startNode = graph.GetNodeFromDictionary(start);
            Node endNode = graph.GetNodeFromDictionary(end);
            var distance = new Dictionary<Node, float>();
            var visitedNodes = new Dictionary<Node, bool>();

            List<Node> path = new List<Node>();
            List<T> pathT = new List<T>();

            foreach (Node n in graph.GetAllNodes())
            {
                distance.Add(n, float.MaxValue);
                visitedNodes.Add(n, false);
            }

            List<Node> needCalculateNodes = new List<Node>();
            distance[startNode] = 0;
            needCalculateNodes.Add(startNode);

            while (needCalculateNodes.Count != 0)
            {
                needCalculateNodes.Sort((node1, node2) => distance[node2].CompareTo(distance[node1]));//по убыванию
                var currentNode = needCalculateNodes[needCalculateNodes.Count - 1];
                needCalculateNodes.RemoveAt(needCalculateNodes.Count - 1);

                if (currentNode == endNode)
                    break;

                foreach (Edge e in currentNode.outgoingEdges)
                {
                    Node tmp = e.To;
                    if (distance[currentNode] + e.Weight < distance[tmp])
                    {
                        distance[tmp] = distance[currentNode] + e.Weight;
                    }
                    if (visitedNodes[tmp] == false)
                    {
                        needCalculateNodes.Add(tmp);
                    }
                }
                visitedNodes[currentNode] = true;
            }

            path = BackTracePath(startNode, endNode, distance);
            foreach (Node n in path)
            {
                pathT.Add(graph.GetObjectByNode(n));
            }
            return pathT;
        }

        private static List<Node> BackTracePath(Node startNode, Node endNode, Dictionary<Node, float> distance)
        {
            var path = new List<Node>();
            var currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                foreach (Edge e in currentNode.incomingEdges)
                {
                    if (distance[currentNode] - e.Weight == distance[e.From])
                    {
                        currentNode = e.From;
                        break;
                    }
                }
            }
            path.Add(currentNode);
            return path;
        }
        public static void PrintGraph<T>(this Graph<T> graph)
        {
            Console.Write("Nodes: \n");
            foreach (Node n in graph.GetAllNodes())
            {
                Console.Write(graph.GetObjectByNode(n).ToString() + " ");
                Console.WriteLine();
            }
            Console.WriteLine();

            foreach (Node n in graph.GetAllNodes())
            {
                Console.WriteLine("Node \n" + graph.GetObjectByNode(n).ToString());
                Console.WriteLine("incoming: ");
                foreach (Edge e in n.incomingEdges)
                {
                    Console.WriteLine(graph.GetObjectByNode(e.From).ToString() + " " +
                                  graph.GetObjectByNode(e.To).ToString()
                                  );
                }
                Console.WriteLine("outgoing: ");
                foreach (Edge e in n.outgoingEdges)
                {
                    Console.WriteLine(graph.GetObjectByNode(e.From).ToString() + " " +
                                  graph.GetObjectByNode(e.To).ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
