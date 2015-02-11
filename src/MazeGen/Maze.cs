using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGen
{
    public class Maze
    {
        private Maze(IEnumerable<Tuple<int, int>> edges, int width, int height)
        {
            Edges = edges;
            Width = width;
            Height = height;
        }

        public IEnumerable<Tuple<int, int>> Edges { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public static Maze Generate(int width, int height)
        {
            var random = new Random();

            var cells = new DisjointSet(width*height);

            var edges = new List<Tuple<int, int>>();
            var maze = new List<Tuple<int, int>>();

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height - 1; y++)
                    edges.Add(new Tuple<int, int>(x + y*width, x + (y + 1)*width));

            for (var x = 0; x < width - 1; x++)
                for (var y = 0; y < height; y++)
                    edges.Add(new Tuple<int, int>(x + y*width, x + 1 + y*width));

            while (cells.Count != 1)
            {
                var edge = edges[random.Next(edges.Count)];

                edges.Remove(edge);

                if (cells.Find(edge.Item1) == cells.Find((edge.Item2)))
                {
                    maze.Add(edge);
                    continue;
                }
                cells.Union(edge.Item1, edge.Item2);
            }

            return new Maze(maze.Union(edges).ToArray(), width, height);
        }
    }
}