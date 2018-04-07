using System;

namespace DefaultNamespace
{
    [Flags]
    public enum LemmingMovementDirection
    {
        None,
        Right = 1 << 0,
        Left = 1 << 1,
        Jump = 1 << 2
    }
}