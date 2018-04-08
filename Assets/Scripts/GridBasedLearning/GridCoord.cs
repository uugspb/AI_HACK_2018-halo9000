using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DefaultNamespace
{
    public struct GridCoord
    {
        public GridCoord(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public override string ToString()
        {
            return "[" + X + ", " + Y + "]";
        }

        public static readonly IEqualityComparer<GridCoord> Comparer = new CoordEqualityComparer();

        private sealed class CoordEqualityComparer : IEqualityComparer<GridCoord>
        {
            public bool Equals(GridCoord x, GridCoord y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(GridCoord obj)
            {
                unchecked
                {
                    return (obj.X * 397) ^ obj.Y;
                }
            }
        }
    }
}