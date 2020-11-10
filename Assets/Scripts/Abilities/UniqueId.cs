public static class UniqueId
{
    static uint _currentID;

    public static uint Next => _currentID++;

    public static void Reset() => _currentID = 0;
}