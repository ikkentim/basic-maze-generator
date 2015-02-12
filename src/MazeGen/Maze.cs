using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGen
{
    /// <summary>
    /// Represents a random maze.
    /// </summary>
    public class Maze
    {
        private Maze(IEnumerable<Tuple<int, int>> edges, int width, int height)
        {
            Edges = edges;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the edges in the maze.
        /// </summary>
        public IEnumerable<Tuple<int, int>> Edges { get; private set; }

        /// <summary>
        /// Gets the width of the maze.
        /// </summary>
        public int Width { get; private set; }
        
        /// <summary>
        /// Gets the height of the maze.
        /// </summary>
        public int Height { get; private set; }

        private static void Shuffle<T>(IList<T> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /// <summary>
        /// Generates a random <see cref="Maze" />.
        /// </summary>
        /// <param name="width">Width of the maze.</param>
        /// <param name="height">Height of the maze.</param>
        /// <returns></returns>
        public static Maze Generate(int width, int height)
        {
            /* disjoint set of all cells .*/
            var cells = new DisjointSet(width*height);

            /* unprocessed edges. */
            var edgesTmp = new List<Tuple<int, int>>();

            /* edges in the maze. */
            var maze = new Stack<Tuple<int, int>>();

            /* Populate edges. */
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height - 1; y++)
                    edgesTmp.Add(new Tuple<int, int>(x + y * width, x + (y + 1) * width));

            for (var x = 0; x < width - 1; x++)
                for (var y = 0; y < height; y++)
                    edgesTmp.Add(new Tuple<int, int>(x + y * width, x + 1 + y * width));

            Shuffle(edgesTmp);
            var edges = new Stack<Tuple<int, int>>(edgesTmp);
            
            /* Keep removing random edges, until only one set of cells is left. */
            while (cells.Count != 1)
            {
                /* Get next edge to process */
                var edge = edges.Pop();

                /* If cells next to edge are connected, add the edge to the maze. */
                if (cells.Find(edge.Item1) == cells.Find((edge.Item2)))
                {
                    maze.Push(edge);
                    continue;
                }

                /* Unite the sections next to the edge. */
                cells.Union(edge.Item1, edge.Item2);
            }

            /* Return the maze. */
            return new Maze(maze.Union(edges).ToArray(), width, height);
        }
    }
}