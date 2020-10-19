public struct Direction
{
    public static int FORWARD = 1;
    public static int BACKWARD = -1;

    public int Current { 
        get => Current == 0 ? FORWARD : Current;
        private set { Current = value; }
    }

    public void Set(int direction)
    {
      Current = direction >= 0 ? FORWARD : BACKWARD;
    }

    public void Reverse()
    {
        Current *= -1;
    }
}
