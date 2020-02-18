using Graph;
using System;
using System.Collections.Generic;

namespace GraphExtensions
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayGame8();
        }

        public static void PlayGame8()
        {
            var start = new Game(new[,] {
                {4, 1, 3},
                {7, 2, 6},
                {5, 0, 8}
             });

            var target = new Game(new[,]{
               {1, 2, 3},
               {4, 5, 6},
               {7, 8, 0}
            });

            Graph<Game> graph = new Graph<Game>();

            foreach (var e in graph.FindPath(start, target))
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}
