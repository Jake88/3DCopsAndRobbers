using My.Movement;
using My.Utilities;
using My.WeightedOptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberSpawner : MonoBehaviour
{
    [SerializeField] Path[] _levelPaths;
    [SerializeField] RobberData[] _robbers;
    WeightedGroup<RobberData> _weightedRobbers;
    float _lowestUnlockedDifficultyWeight = 1000f;

    [SerializeField] Range _spawnDifficultyRange; // TODO: Should this be a thing, or should there be a button to "Open mall"
    
    [SerializeField] float _levelStartSpawnDelay; // TODO: Should this be a thing, or should there be a button to "Open mall"
    float _timeRemainingUntilSpawnsBegin;

    [SerializeField] Range _initialSpawnIntervalRange;
    [SerializeField] Range _groupSpawnInterval;

    Range _spawnIntervalRange;

    float _timeUntilSpawn;

    float DetermineSpawnDifficulty()
    {
        // Based on difficulty selected, mall rating and whatever other factors exist,
        // return a float that represents the difficulty this spawn.

        return 4f * _spawnDifficultyRange.Random;
    }

    RobberData[] DetermineWhatToSpawn()
    {
        List<RobberData> groupToSpawn = new List<RobberData>(); // TODO: Eventually we want to alter this from just being robber data to something more like spawn data. Spawn data should have an array of things to spawn, and a weight (difficulty and spawn weight). Spawn data allows predefined 'groups' of robbers to spawn.

        float spawnDifficulty = DetermineSpawnDifficulty();
        //Debug.Log($"Initial spawnDifficulty: {spawnDifficulty}");

        // Randomly create a group of robbers to spawn that add up to the spawn difficulty.

        // TODO: Eventually we will have built up "unlocked" robbers based on mall rating change events
        // For now just using all robbers array.

        // Todo: we should be able to cache this infomation and then just add to it when a a new tier of robbers / groups unlocks.
        while (spawnDifficulty > _lowestUnlockedDifficultyWeight)
        {
            // TODO: Could actually spread out the performance cost of this loop by doing it in a co-routine. There's no reason why I need to determine the entire robber group first and then
            // start spawning. I could determine what robber to spawn, spawn it, check if spawnDifficulty > _lowestUnlockedDifficultyWeight, and if so yield return a random spawn interval.
            // This would mean rather than having to do this loop 4-5 times in a single frame, it is instead spread across multiple.
            // This combined with possible caching could make this solution actually viable..

            //var robberToAdd = _weightedRobbers.GetRandom();
            var robberToAdd = _weightedRobbers.GetRandomWithinDifficultyValue(spawnDifficulty);
            spawnDifficulty -= robberToAdd.InitialDifficultyWeight;
            groupToSpawn.Add(robberToAdd);
            //Debug.Log($"Added robber ({robberToAdd.name}) of difficulty ({robberToAdd.InitialDifficultyWeight}). spawnDifficulty remaining: {spawnDifficulty}");
            // Debug.Log($"spawnDifficulty ({spawnDifficulty}) / _lowestUnlockedDifficultyWeight ({_lowestUnlockedDifficultyWeight})");

        }

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
        return groupToSpawn.ToArray();
    }

    void Awake()
    {
        _weightedRobbers = new WeightedGroup<RobberData>();
        _timeRemainingUntilSpawnsBegin = _levelStartSpawnDelay;
        _spawnIntervalRange = _initialSpawnIntervalRange;

        foreach (var robber  in _robbers)
        {
            _weightedRobbers.AddItem(robber, robber.InitialSpawnWeight);
            if (robber.InitialDifficultyWeight < _lowestUnlockedDifficultyWeight) _lowestUnlockedDifficultyWeight = robber.InitialDifficultyWeight;
        }
    }

    void SetNewSpawnTimer() => _timeUntilSpawn = _spawnIntervalRange.Random;

    void FixedUpdate()
    {
        //TODO: This whole function should really be an infinite looping coroutine that kicks off on start.
        if (_timeRemainingUntilSpawnsBegin > 0)
        {
            _timeRemainingUntilSpawnsBegin -= Time.fixedDeltaTime;
            return;
        }

        _timeUntilSpawn -= Time.fixedDeltaTime;
        if (_timeUntilSpawn < 0) StartSpawnSequence();
    }

    void StartSpawnSequence()
    {
        SetNewSpawnTimer();
        var group = DetermineWhatToSpawn();
        StartCoroutine(SpawnGroup(group));
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

    IEnumerator SpawnGroup(RobberData[] robbersToSpawn)
    {
        // Spawn it/them
        foreach (var robber in robbersToSpawn)
        {
            var r = robber.Pool.GetObjectComponent<Robber>();
            r.Spawn(_levelPaths[Random.Range(0, _levelPaths.Length - 1)]);
            yield return new WaitForSeconds(_groupSpawnInterval.Random);
        }

        SetNewSpawnTimer();
    }
}
