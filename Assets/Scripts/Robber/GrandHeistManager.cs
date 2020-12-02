using My.Singletons;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobberSpawnController))]
[RequireComponent(typeof(RobberFactory))]
public class GrandHeistManager : MonoBehaviour
{
    // Array of GrandHeists, in the order to go through them as the level progresses. For a 10 day level we would have 10 heists defined in the list.
    // --- how do we cator for "grace periods" in the game(currently in GameTime), or is there none?
    [SerializeField] GrandHeist[] _heists;
    int _currentHeistIndex;
    RobberSpawnController _spawnController;
    RobberFactory _factory;

    void Awake()
    {
        _spawnController = GetComponent<RobberSpawnController>();
        _factory = GetComponent<RobberFactory>();
    }

    public void SpawnNext()
    {
        if (_currentHeistIndex == _heists.Length - 1)
        {
            print("Final wave. Pause all income, prevent build mode etc???");
        }

        GrandHeist heist = _heists[_currentHeistIndex];
        var spawnCurrency = heist.AdditionalSpawnCurrency;

        List<Robber> robbersToSpawn = new List<Robber>();
        foreach (var rd in heist.RobbersInHeist)
        {
            var robber = _factory.BuildRobber(rd);
            ApplyAbilities(heist, robber);
            robbersToSpawn.Add(robber);
        }

        while (spawnCurrency > 0)
        {
            Robber robber = _factory.BuildRandomRobber(spawnCurrency);
            if (robber == null) break;
            spawnCurrency -= robber.SpawnCost;
            ApplyAbilities(heist, robber);
            robbersToSpawn.Add(robber);
        }

        // Trigger heist animation

        // On heist animation end

        _spawnController.SpawnGroup(robbersToSpawn);
        
        _currentHeistIndex++;
    }

    void ApplyAbilities(GrandHeist heist, Robber robber)
    {
        foreach (var ability in heist.Abilities)
        {
            robber.AddAbility(ability);
        }

        if (heist.RandomAbilitiesToAddToEachRobber.Max > 0)
        {
            var abilities = RefManager.AbilityFactory.GetRobberAbilities(
                robber.AbilityPrerequisites,
                heist.RandomAbilitiesToAddToEachRobber.RandomInt);

            foreach (var ability in abilities)
                robber.AddAbility(ability);

        }
    }

    public void SpawnRandomHeist()
    {
        // Function to create a random heist? // mainly for a sandbox type scenario
        // --- Pick from pre defined heists? Utilise RobberSpawner and just ask for a set of heists?
    }
}
