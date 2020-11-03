using UnityEngine;

public class RobberSpawner : MonoBehaviour
{
    [SerializeField] Path[] _levelPaths;
    [SerializeField] RobberData[] _robbers;

    [SerializeField] float _levelStartSpawnDelay; // TODO: Should this be a thing, or should there be a button to "Open mall"
    float _timeRemainingUntilSpawnsBegin;

    [SerializeField] Range _initialSpawnIntervalRange;
    [SerializeField] Range _groupSpawnInterval;

    Range _spawnIntervalRange;

    float _timeUntilSpawn;

    void DetermineSpawnDifficulty()
    {
        // Based on difficulty selected, mall rating and whatever other factors exist,
        // return a float that represents the difficulty this spawn.
    }

    Robber[] DetermineWhatToSpawn()
    {
        // float spawnDifficulty = DetermineSpawnDifficulty()

        // Randomly create a group of robbers to spawn that add up to the spawn difficulty.

        // How?
        /*
         * Lots of ways to do this.
         * Opt1: Create a dictionary of "unlocked" robbers. This gets added to at certain difficulty / mall rating thresholds, adding more difficult robbers int.
         * - Each of these "more difficult" robbers would have a higher weight in terms of likelyness to spawn. IE (RobberA weight = 1, RobberB weight = 2, RobberC weight = 3
         * - On spawning, generate a random number between 0 to (SumOfAllUnlockedWeights - difficultyWeightConsumed).
         * - Using the random number, select the robber that applies to that weight. IE RobberA(0-1), RobberB(1-3), RobberC(3-6), etc
         * - difficultyWeightConsumed += weight of selectedRobber above.
         * - Repeat this until (SumOfAllUnlockedWeights - difficultyWeightConsumed) < weight of lowest difficulty weight robber.
         * 
         * ExtraOption:
         * - Could add specific groups of robbers that have a difficulty weight of their own. These would be counted the same as indiviudal robbers as above, only they would spawn a specific set of robbers.
         * - This could allow of specific groups to be defined (like a group of muggers, or stealthy units etc) to provide more themeatic waves rather than _completely random_ waves.
         * - Also allows skewing the difficulty weight of these, for example a super synergistic set of "lower difficulty" robbers could be defined with extra difficulty weight.
         * - Also means the opposite - could make a themeatically "easier" group of harder robbers and reduce the difficulty.
         * */

        // Return the array of robbers to spawn.
        return new Robber[0];
    }

    void Awake()
    {
        _timeRemainingUntilSpawnsBegin = _levelStartSpawnDelay;
        _spawnIntervalRange = _initialSpawnIntervalRange;
    }

    void SetNewSpawnTimer() => _timeUntilSpawn = _spawnIntervalRange.Random;

    void FixedUpdate()
    {
        if (_timeRemainingUntilSpawnsBegin > 0)
        {
            _timeRemainingUntilSpawnsBegin -= Time.fixedDeltaTime;
            return;
        }

        _timeUntilSpawn -= Time.fixedDeltaTime;
        if (_timeUntilSpawn < 0) Spawn();
    }

    public void AdjustSpawnRange(Range range)
    {
        _spawnIntervalRange = new Range(
            range.Min + _spawnIntervalRange.Min,
            range.Max + _spawnIntervalRange.Max);
    }

    public void ResetSpawnRange()
    {
        _spawnIntervalRange = _initialSpawnIntervalRange;
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
