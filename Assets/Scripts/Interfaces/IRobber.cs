using My.Movement;

interface IRobber
{
    bool TakeDamage(int damage);
    void Spawn(Path path);
}
