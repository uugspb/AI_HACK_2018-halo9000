using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class LemmingRunHistory
    {
        public List<GridCoord> Coords = new List<GridCoord>();
        public List<LemmingMovementDirection> Directions = new List<LemmingMovementDirection>();
        public Killer Killer = Killer.None;
        public GridCoord FinalCoord;

        public void AddRecord(GridCoord coord, LemmingMovementDirection direction)
        {
            Coords.Add(coord);
            Directions.Add(direction);
        }

        public void Kill(Killer killer)
        {
            FinalCoord = Coords.Last();
            Killer = killer;
        }
        
        public void Win()
        {
            FinalCoord = Coords.Last();
            Killer = Killer.None;
        }
    }
}