using UnityEngine;

namespace DefaultNamespace
{
    class PositionToGridTransformer
    {
        public static readonly PositionToGridTransformer Instance = new PositionToGridTransformer();
        private readonly int X_CELL_SIZE = 300;
        private readonly int Y_CELL_SIZE = 300;        

        private System.Random _random = new System.Random();
        
        public GridCoord GetGridCoord(Vector3 position)
        {
            var x = (int) (position.x + _random.NextDouble() * 2) /*/ X_CELL_SIZE * X_CELL_SIZE*/;
            var y = (int) (position.y + _random.NextDouble() * 2) /*/ Y_CELL_SIZE * Y_CELL_SIZE*/;
            return new GridCoord(x, y);
        }
    }
}