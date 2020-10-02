using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class RobberMovement : MonoBehaviour
{
    public RadialNode[] route;

    private AIPath ai;
    private int destinationIndex = 0;
    private bool returning = false;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AIPath>();
        SetDestination(GetNextDestination());
    }

    void Update()
    {
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            if (HasFinishedRoute()) return;
            SetDestination(GetNextDestination());
        }
    }

    private void SetDestination(Vector3 point)
    {
        ai.destination = point;
        ai.SearchPath();
    }

    private Vector3 GetNextDestination()
    {
        if (returning)
        {
            destinationIndex--;
        }
        else
        {
            destinationIndex++;
        }

        if (destinationIndex >= route.Length)
        {
            destinationIndex -= 2;
            returning = true;
        }

        return route[destinationIndex].GetRandomPoint();
    }
    private bool HasFinishedRoute()
    {
        if (destinationIndex == 0)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}