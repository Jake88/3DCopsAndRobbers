using My.Movement;
using My.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobberSpawner))]
[RequireComponent(typeof(RobberFactory))]
[RequireComponent(typeof(RobberSpawnDifficulty))]
public class RobberSpawnController : MonoBehaviour
{
    [SerializeField] Range _spawnIntervalRange;
    [SerializeField] Path[] _levelPaths;
    //[SerializeField] SpawnBehaviour _spawnBehaviour; exposes a function that determines how to choose what path to spawn into.

    RobberSpawner _spawner;
    RobberFactory _factory;
    RobberSpawnDifficulty _difficulty;

    float _timeUntilSpawn;

    void Awake()
    {
        _spawner = GetComponent<RobberSpawner>();
        _factory = GetComponent<RobberFactory>();
        _difficulty = GetComponent<RobberSpawnDifficulty>();
    }
    void Start() => StartCoroutine(SpawnTimer());

    IEnumerator SpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeUntilSpawn);
            StartSpawnSequence();
        }
    }
    void StartSpawnSequence()
    {
        _timeUntilSpawn = _spawnIntervalRange.Random;
        StartCoroutine(SpawnGroup());
    }

    IEnumerator SpawnGroup()
    {
        var remainingSpawnPoints = _difficulty.DetermineSpawnDifficulty();
        while (remainingSpawnPoints > 0)
        {
            Robber robber = _factory.BuildRandomRobber(remainingSpawnPoints);
            if (robber == null) break;
            remainingSpawnPoints -= robber.SpawnCost;
            _spawner.AddToQueue(new SpawnData(robber, DeterminePath()));
            yield return null;
        }
    }

    public void SpawnGroup(List<Robber> robbers)
    {
        foreach (var robber in robbers)
        {
            _spawner.AddToQueue(new SpawnData(robber, DeterminePath()));
        }
    }

    Path DeterminePath()
    {
        // based on behaviour, return the next path.
        return _levelPaths[UnityEngine.Random.Range(0, _levelPaths.Length)];
    }
}
