using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphExtensions
{
    public class GraphTarjanAlgorithm
    {
        public enum State
        {
            White,
            Gray,
            Black
        }

        public static List<T> TarjanAlgorithm<T>(Graph<T> graph)
            //topological search
        {
            var topSort = new List<Node>();
            var states = graph.GetAllNodes().ToDictionary(node => node, node => State.White);
            while (true)
            {
                var nodeToSearch = states
                    .Where(z => z.Value == State.White)
                    .Select(z => z.Key)
                    .FirstOrDefault();
                if (nodeToSearch == null) break;

                if (!TarjanDepthSearch(nodeToSearch, states, topSort))
                    return null;
            }
            var result = topSort.Select(e=> graph.GetObjectByNode(e))
                                .Reverse()
                                .ToList();
            return result;
        }

        public static bool TarjanDepthSearch(Node node, Dictionary<Node, State> states, List<Node> topSort)
        {
            if (states[node] == State.Gray) return false;
            if (states[node] == State.Black) return true;
            states[node] = State.Gray;

            var outgoingNodes = node.outgoingEdges
                .Where(edge => edge.From == node)
                .Select(edge => edge.To);
            foreach (var nextNode in outgoingNodes)
                if (!TarjanDepthSearch(nextNode, states, topSort)) return false;

            states[node] = State.Black;
            topSort.Add(node);
            return true;
        }
    }
}
