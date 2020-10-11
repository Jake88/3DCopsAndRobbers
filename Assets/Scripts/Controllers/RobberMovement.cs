using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class RobberMovement : MonoBehaviour
{
    [SerializeField] RadialNode[] _route;

    AIPath _ai;
    int _destinationIndex = 0;
    bool _returning = false;

    void Awake()
    {
        _ai = GetComponent<AIPath>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetDestination(GetNextDestination());
    }

    void Update()
    {
        if (!_ai.pathPending && (_ai.reachedEndOfPath || !_ai.hasPath))
        {
            if (HasFinishedRoute()) return;
            SetDestination(GetNextDestination());
        }
    }

    void SetDestination(Vector3 point)
    {
        _ai.destination = point;
        _ai.SearchPath();
    }

    Vector3 GetNextDestination()
    {
        if (_returning)
        {
            _destinationIndex--;
        }
        else
        {
            _destinationIndex++;
        }

        if (_destinationIndex >= _route.Length)
        {
            _destinationIndex -= 2;
            _returning = true;
        }

        return _route[_destinationIndex].RandomPoint;
    }
    bool HasFinishedRoute()
    {
        if (_destinationIndex == 0)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}