using My.ModifiableStats;
using Pathfinding;
using System;
using UnityEngine;

public class RobberMovement : MonoBehaviour, IMove
{
    ModifiableStat _moveSpeed;
    int _destinationIndex = 1;
    Path _path;
    AIPath _ai;

    public Action OnBankReached;
    public Action OnExitReached;

    public void Initilise(float initialMovementSpeed)
    {
        _ai = GetComponent<AIPath>();
        _moveSpeed = new ModifiableStat(initialMovementSpeed);
    }

    public void Reset(Path path)
    {
        _moveSpeed.Reset();
        _ai.maxSpeed = _moveSpeed.Value;

        _path = path;
        transform.position = _path.StartNode.RadialPosition;
        _destinationIndex = 0;
        SetDestination();
    }

    void Update()
    {
        if (!_ai.pathPending && (_ai.reachedEndOfPath || !_ai.hasPath))
        {
            CheckForBank();
            SetDestination();
        }
    }

    void CheckForBank()
    {
        if (_path.HasArrivedAtBank(_destinationIndex)) OnBankReached?.Invoke();
    }

    void SetDestination()
    {
        // Maybe turn this into a coroutine so we can pause at the start if we're at a bank or distraction
        _destinationIndex++;
        RadialNode nextDestination = _path.GetNode(_destinationIndex);

        if (nextDestination == null)
        {
            OnExitReached?.Invoke();
            return;
        }

        Vector3 randomPoint = nextDestination.RadialPosition;
        _ai.destination = randomPoint;
        _ai.SearchPath();
    }

    public void Restart()
    {
        _destinationIndex = 0;
        SetDestination();
    }

    public void Pause()
    {
        _ai.maxSpeed = 0;
    }

    public void Resume()
    {
        _ai.maxSpeed = _moveSpeed.Value;
    }

    public void ApplyMoveSpeedModifer(StatModifier modifer)
    {
        _moveSpeed.AddModifier(modifer);
        _ai.maxSpeed = _moveSpeed.Value;
    }

    public void RemoveMoveSpeedModifer(StatModifier modifer)
    {
        _moveSpeed.AddModifier(modifer);
        _ai.maxSpeed = _moveSpeed.Value;
    }
}