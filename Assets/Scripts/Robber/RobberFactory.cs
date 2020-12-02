using My.Abilities;
using My.WeightedOptions;
using System.Collections.Generic;
using UnityEngine;

public class RobberFactory : MonoBehaviour
{
    // How do we track locked / unlocked robbers?

    [Tooltip("Abilities to apply to every robber on spawn! WILD!")]
    [SerializeField] Ability[] _spawnAbilities;
    [SerializeField] List<RobberData> _availableRobbers;
    WeightedGroup<RobberData> _weightedRobbers;
    float _lowestSpawnCost = 1000;

    void Awake()
    {
        PopulateWeightedRobbers();
    }

    void PopulateWeightedRobbers()
    {
        _weightedRobbers = new WeightedGroup<RobberData>();
        foreach (var robber in _availableRobbers)
        {
            _weightedRobbers.AddItem(robber, robber.InitialSpawnWeight);
            if (robber.InitialDifficultyWeight < _lowestSpawnCost) _lowestSpawnCost = robber.InitialDifficultyWeight;
        }
    }

    public Robber BuildRandomRobber(float spawnPoints)
    {
        if (spawnPoints < _lowestSpawnCost) return null;
        var robberData = _weightedRobbers.GetRandomWithinDifficultyValue(spawnPoints);
        return BuildRobber(robberData);
    }

    public Robber BuildRobber(RobberData robberData)
    {
        if (robberData == null) return null;
        var robber = robberData.Pool.GetObjectComponent<Robber>();
        robber.gameObject.SetActive(false); // required because the Pool retrieve loads it in an active state, causing errors (no path assigned)
        ApplyAbilities(robber);
        return robber;
    }

    void ApplyAbilities(Robber robber)
    {
        foreach (var ability in _spawnAbilities)
            robber.AddAbility(ability);
    }

    /*public Robber[] BuildRobbers(float difficulty) // moving to the factory
    {
        List<RobberData> groupToSpawn = new List<RobberData>(); // TODO: Eventually we want to alter this from just being robber data to something more like spawn data. Spawn data should have an array of things to spawn, and a weight (difficulty and spawn weight). Spawn data allows predefined 'groups' of robbers to spawn.
        var spawnDifficulty = difficulty;

        // TODO: Eventually we will have built up "unlocked" robbers based on mall rating change events
        // For now just using all robbers array.

        // Todo: we should be able to cache this infomation and then just add to it when a a new tier of robbers / groups unlocks.
        while (spawnDifficulty > _lowestSpawnCost)
        {
            // TODO: Could actually spread out the performance cost of this loop by doing it in a co-routine. There's no reason why I need to determine the entire robber group first and then
            // start spawning. I could determine what robber to spawn, spawn it, check if spawnDifficulty > _lowestUnlockedDifficultyWeight, and if so yield return a random spawn interval.
            // This would mean rather than having to do this loop 4-5 times in a single frame, it is instead spread across multiple.
            // This combined with possible caching could make this solution actually viable..

            var robberToAdd = _weightedRobbers.GetRandomWithinDifficultyValue(spawnDifficulty);
            spawnDifficulty -= robberToAdd.InitialDifficultyWeight;
            groupToSpawn.Add(robberToAdd);
        }


         *//* ExtraOption:
         * - Could add specific groups of robbers that have a difficulty weight of their own. These would be counted the same as indiviudal robbers as above, only they would spawn a specific set of robbers.
         * - This could allow of specific groups to be defined (like a group of muggers, or stealthy units etc) to provide more themeatic waves rather than _completely random_ waves.
         * - Also allows skewing the difficulty weight of these, for example a super synergistic set of "lower difficulty" robbers could be defined with extra difficulty weight.
         * - Also means the opposite - could make a themeatically "easier" group of harder robbers and reduce the difficulty.
         * *//*

        // Return the array of robbers to spawn.
        return groupToSpawn.ToArray();
    }*/

    bool RemoveRobber(RobberData rd) => _availableRobbers.Remove(rd);
    void AddRobber(RobberData rd)
    {
        if (!_availableRobbers.Contains(rd))
        {
            _availableRobbers.Add(rd);

            if (_lowestSpawnCost > rd.InitialDifficultyWeight)
                _lowestSpawnCost = rd.InitialDifficultyWeight;
        }
    }
}
