using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    internal class GridMapScore
    {
        public static GridMapScore GLOBAL_SCORE = new GridMapScore();

        public const LemmingMovementDirection DEFAULT_DIRECTION = LemmingMovementDirection.Right;

        private Dictionary<GridCoord, Dictionary<LemmingMovementDirection, int>> myMap =
            new Dictionary<GridCoord, Dictionary<LemmingMovementDirection, int>>(GridCoord.Comparer);

        private int AVERAGE_MAX = 0;
        private int COUNT = 1;
        private readonly Random _random = new Random();

        public LemmingMovementDirection GetBestDirection(GridCoord coord)
        {
            var directions = GetOrCreateScore(coord);
            var maxPair = new KeyValuePair<LemmingMovementDirection, int>(CreateRandomDirection(), _random.Next(300));
            foreach (var pair in directions)
            {
                if (pair.Value > maxPair.Value) maxPair = pair;
            }

            if (_random.Next(10) > 1) return maxPair.Key;
            while (true)
            {
                var direction = CreateRandomDirection();
                if (direction != maxPair.Key) return direction;
            }
        }

        public Dictionary<LemmingMovementDirection, int> GetOrCreateScore(GridCoord coord)
        {
            Dictionary<LemmingMovementDirection, int> directions;
            if (myMap.TryGetValue(coord, out directions)) return directions;

            directions = new Dictionary<LemmingMovementDirection, int>(4)
            {
                {LemmingMovementDirection.None, 0},
                {LemmingMovementDirection.Left, 0},
                {LemmingMovementDirection.Right, 0},
                {LemmingMovementDirection.Jump, 0},
            };
            myMap[coord] = directions;
//            var newDirection = CreateRandomDirection();
//            directions[newDirection] = 1;    

            return directions;
        }

        private LemmingMovementDirection CreateRandomDirection()
        {
            var next = _random.Next(6);
//            if (next == 0) return LemmingMovementDirection.Left;
            var newDirection = (next > 2) ? LemmingMovementDirection.Right : LemmingMovementDirection.Jump;
            return newDirection;
        }

        public void CorrectScore(LemmingRunHistory history)
        {
            switch (history.Killer)
            {
                case Killer.Player:
                    for (var i = history.Coords.Count/3*2; i < history.Coords.Count; i++)
                    {
                        var score = GetOrCreateScore(history.Coords[i]);
                        score[history.Directions[i]] -= 10000;
                    }

                    break;
                case Killer.Suicide:
                    var good = history.FinalCoord.X > AVERAGE_MAX + 10;
                    if (good)
                    {
                        var limit = history.Coords.Count * 9 / 10;
                        for (var i = 0; i < limit; i++)
                        {
                            var score = GetOrCreateScore(history.Coords[i]);
                            score[history.Directions[i]] += 300;
                        }
                        AVERAGE_MAX = (AVERAGE_MAX * COUNT + history.FinalCoord.X) / ++COUNT;
                    }
                    else
                    {
                        for (var i = history.Coords.Count/3 * 2; i < history.Coords.Count; i++)
                        {
                            var score = GetOrCreateScore(history.Coords[i]);
                            score[history.Directions[i]] -= 500;
                        }
                    }

                    break;
                case Killer.None:
                    for (var i = 0; i < history.Coords.Count; i++)
                    {
                        var score = GetOrCreateScore(history.Coords[i]);
                        score[history.Directions[i]] += 10000;
                    }

                    AVERAGE_MAX = (AVERAGE_MAX * COUNT + history.FinalCoord.X) / ++COUNT;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}