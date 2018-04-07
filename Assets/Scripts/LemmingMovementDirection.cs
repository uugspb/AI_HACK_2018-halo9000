namespace DefaultNamespace
{
    public enum LemmingMovementDirection
    {
        None,
        Left = 1 << 0,
        Right = 1 << 1,
        Jump = 1 << 2
    }
}