using UnityEngine;

public class RobberSpawner : MonoBehaviour
{
    [SerializeField] Path[] _levelPaths;
    [SerializeField] RobberData[] _robbers;

    [SerializeField] Range _initialSpawnRange;
    [SerializeField] Range _groupSpawnInterval;

    Range _spawnRange;

    float _timeUntilSpawn;

    void Awake()
    {
        _spawnRange = _initialSpawnRange;
        SetNewSpawnTimer();
    }

    void SetNewSpawnTimer() => _timeUntilSpawn = _spawnRange.Random;

    void FixedUpdate()
    {
        _timeUntilSpawn -= Time.fixedDeltaTime;
        if (_timeUntilSpawn < 0) Spawn();
    }

    public void AdjustSpawnRange(Range range)
    {
        _spawnRange = new Range(
            range.Min + _spawnRange.Min,
            range.Max + _spawnRange.Max);
    }

    public void ResetSpawnRange()
    {
        _spawnRange = _initialSpawnRange;
    }

    public void AlterNextSpawnTime(float amount)
    {
        _timeUntilSpawn += amount;
    }

    public void Spawn()
    {
        // Determine what to spawn based on weight
        RobberData[] robbersToSpawn = _robbers;

        // Spawn it/them
        foreach (var robber in robbersToSpawn)
        {
            var r = robber.Pool.GetObjectComponent<Robber>();
            r.Spawn(_levelPaths[Random.Range(0, _levelPaths.Length-1)]);
        }

        // Set a new timer
        SetNewSpawnTimer();
    }
}
