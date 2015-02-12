using System;
using System.Linq;

namespace MazeGen
{
    public class DisjointSet
    {
        private readonly int[] _data;

        public DisjointSet(int capacity)
        {
            _data = Enumerable.Repeat(-1, capacity).ToArray();
            Count = capacity;
        }

        public int this[int i]
        {
            get { return _data[i]; }
        }

        public int Count { get; private set; }

        public int Find(int element)
        {
            if (element < 0 || element >= _data.Length)
                throw new IndexOutOfRangeException();

            if (_data[element] < 0) return element;

            var set = Find(_data[element]);

            _data[element] = set;

            return set;
        }

        public void Union(int element1, int element2)
        {
            element1 = Find(element1);
            element2 = Find(element2);

            if (_data[element1] < _data[element2])
            {
                _data[element2] = element1;
                _data[element1]--;
            }
            else
            {
                _data[element1] = element2;
                _data[element2]--;
            }

            Count--;
        }
    }
}