using Graph;
using GraphExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NUnitGraphTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private void InitGraph1(Graph<int> graph)
        {
            graph.AddEdge(1, 2, 7);
            graph.AddEdge(1, 4, 9);
            graph.AddEdge(1, 5, 8);
            graph.AddEdge(3, 2, 5);
            graph.AddEdge(3, 4, 15);
            graph.AddEdge(3, 5, 1);
            graph.AddEdge(3, 7, 1);
            graph.AddEdge(5, 1, 11);
            graph.AddEdge(5, 3, 7);
            graph.AddEdge(5, 7, 9);
            graph.AddEdge(6, 4, 10);
            graph.AddEdge(6, 5, 14);
            graph.AddEdge(3, 8, 10);
            graph.AddEdge(7, 8, 1);
        }

        private void InitGraph2(Graph<int> graph)
        {
            graph.AddEdge(0, 1, 0);
            graph.AddEdge(0, 2, 0);
            graph.AddEdge(1, 3, 0);
            graph.AddEdge(1, 4, 0);
            graph.AddEdge(2, 3, 0);
            graph.AddEdge(3, 4, 0);
        }

        [Test]
        public void DeikstraTest1()
        {
            Graph<int> graph = new Graph<int>();
            InitGraph1(graph);

            List<int> path = graph.Dejkstra(1, 8);
            List<int> expectedPath = new List<int>() { 8,7,3,5,1 };
            Assert.AreEqual(path, expectedPath);

            graph.RemoveNode(3);
            path = graph.Dejkstra(1, 8);
            expectedPath = new List<int>() { 8, 7, 5, 1 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void DeikstraTest2()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(0, 1, 0);
            graph.AddEdge(1, 0, 0);
            graph.AddEdge(0, 2, 0);
            graph.AddEdge(2, 0, 0);
            graph.AddEdge(1, 4, 0);
            graph.AddEdge(4, 1, 0);
            graph.AddEdge(2, 3, 0);
            graph.AddEdge(3, 2, 0);
            graph.AddEdge(3, 4, 0);
            graph.AddEdge(4, 3, 0);

            List<int> path = graph.Dejkstra(0, 4);
            List<int> expectedPath = new List<int>() { 4,1,0 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void DepthSearchTest1()
        {
            Graph<int> graph = new Graph<int>();
            InitGraph1(graph);

            var path = graph.DepthSearch(1);
            List<int> expectedPath = new List<int>() { 1,5,7,8,3,4,2 };
            Assert.AreEqual(path, expectedPath);

            path = graph.DepthSearch(6);
            expectedPath = new List<int>() { 6,5,7,8,3,4,2,1 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void DepthSearchTest2()
        {
            Graph<int> graph = new Graph<int>();
            InitGraph2(graph);

            var path = graph.DepthSearch(0);
            List<int> expectedPath = new List<int>() { 0,2,3,4,1 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void BreadthSearchTest1()
        {
            Graph<int> graph = new Graph<int>();
            InitGraph1(graph);

            var path = graph.BreadthSearch(1);
            List<int> expectedPath = new List<int>() { 1,2,4,5,3,7,8 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void BreadthSearchTest2()
        {
            Graph<int> graph = new Graph<int>();
            InitGraph2(graph);

            var path = graph.BreadthSearch(0);
            List<int> expectedPath = new List<int>() { 0, 1, 2, 3, 4 };
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void ConnectedComponentsTest1()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(1, 2, 0);
            graph.AddEdge(3, 4, 0);
            graph.AddEdge(4, 5, 0);
            graph.AddEdge(5, 3, 0);

            var result = graph.FindConnectedComponents();
            List<List<int>> expectedResult = new List<List<int>>() { 
                                                                        new List<int>{1,2},
                                                                        new List<int>{3,4,5}
                                                                    };
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void ConnectedComponentsTest2()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(1, 2, 0);
            graph.AddEdge(3, 4, 0);
            graph.AddEdge(4, 5, 0);
            graph.AddEdge(5, 3, 0);
            graph.AddEdge(2, 4, 0);
            graph.AddEdge(6, 7, 0);
            graph.AddEdge(7, 8, 0);
            graph.AddEdge(9, 10, 0);

            var result = graph.FindConnectedComponents();
            List<List<int>> expectedResult = new List<List<int>>() {
                                                                        new List<int>{1,2,4,5,3},
                                                                        new List<int>{6,7,8},
                                                                        new List<int>{9,10}
                                                                    };
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void TarjanAlgorithmTest1()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(0, 1, 0);
            graph.AddEdge(0, 2, 0);
            graph.AddEdge(1, 2, 0);
            graph.AddEdge(1, 3, 0);
            graph.AddEdge(2, 3, 0);
            graph.AddEdge(2, 4, 0);
            graph.AddEdge(3, 4, 0);

            var path = GraphTarjanAlgorithm.TarjanAlgorithm(graph);
            List<int> expectedPath = new List<int>() {0,1,2,3,4};
            Assert.AreEqual(path, expectedPath);
        }

        [Test]
        public void GraphHasCycleTest1()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(0, 1, 0);
            graph.AddEdge(0, 2, 0);
            graph.AddEdge(1, 2, 0);
            graph.AddEdge(1, 3, 0);
            graph.AddEdge(2, 3, 0);
            graph.AddEdge(2, 4, 0);
            graph.AddEdge(3, 4, 0);

            Assert.AreEqual(graph.HasCycle(), false);
        }

        [Test]
        public void GraphHasCycleTest2()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddEdge(0, 1, 0);
            graph.AddEdge(0, 2, 0);
            graph.AddEdge(2, 1, 0);
            graph.AddEdge(1, 3, 0);
            graph.AddEdge(3, 2, 0);
            graph.AddEdge(2, 4, 0);
            graph.AddEdge(3, 4, 0);

            Assert.AreEqual(graph.HasCycle(), true);
        }
    }
}