using My.Movement;
using My.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberSpawner : MonoBehaviour
{
    [SerializeField] Range _groupSpawnInterval;

    Queue<SpawnData> _queue = new Queue<SpawnData>();
    Coroutine _spawnCoroutine;

    void Start() => _spawnCoroutine = StartCoroutine(SpawnLoop());
    public void Pause() => StopCoroutine(_spawnCoroutine);
    public void Resume()
    {
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = StartCoroutine(SpawnLoop());
    }
    public void AddToQueue(SpawnData rd) => _queue.Enqueue(rd);
    public void ClearQueue() => _queue.Clear();
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            while (_queue.Count == 0) yield return null;

            var spawnData = _queue.Dequeue();

            spawnData.Robber.Spawn(spawnData.Path);

            yield return new WaitForSeconds(_groupSpawnInterval.Random);
        }
    }
}
