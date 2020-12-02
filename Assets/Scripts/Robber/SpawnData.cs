using My.Movement;

public class SpawnData
{
    public Robber Robber { get; }
    public Path Path { get; }

    public SpawnData(Robber robber, Path path)
    {
        Robber = robber;
        Path = path;
    }
}