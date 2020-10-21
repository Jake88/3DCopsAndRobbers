using UnityEngine;
using Pathfinding;

public class RobberMovement : MonoBehaviour
{
    Robber _robber;
    Path _path;
    AIPath _ai;
    int _destinationIndex = 1;

    public bool Repeat;

    void Awake()
    {
        _ai = GetComponent<AIPath>();
        _robber = GetComponent<Robber>();
    }

    public void Spawn(float speed, Path path)
    {
        _path = path;
        transform.position = _path.StartNode.RadialPosition;
        _ai.maxSpeed = speed;
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
        if (_path.HasArrivedAtBank(_destinationIndex)) _robber.Steal();
    }

    void SetDestination()
    {
        // Maybe turn this into a coroutine so we can pause at the start if we're at a bank or distraction
        _destinationIndex++;
        RadialNode nextDestination = _path.GetNode(_destinationIndex);

        if (nextDestination == null)
        {
            if (Repeat)
            {
                _destinationIndex = 1;
                nextDestination = _path.GetNode(_destinationIndex);
            }
            else
            {
                Debug.Log("Robber route finished!");
                CleanUp();
                return;
            }
        }

        Vector3 randomPoint = nextDestination.RadialPosition;
        // transform.LookAt(randomPoint);
        _ai.destination = randomPoint;
        _ai.SearchPath();
    }

    void CleanUp()
    {
        gameObject.SetActive(false);
    }
}