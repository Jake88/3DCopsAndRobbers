using UnityEngine;
using Pathfinding;

public class RobberMovement : MonoBehaviour
{
    [SerializeField] Path _path;

    AIPath _ai;
    int _destinationIndex = 1;

    void Awake()
    {
        _ai = GetComponent<AIPath>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        transform.position = _path.Start.RadialPosition;
        _destinationIndex = 0;
        SetDestination();
    }

    void Update()
    {
        if (!_ai.pathPending && (_ai.reachedEndOfPath || !_ai.hasPath))
        {
            SetDestination();
        }
    }

    void SetDestination()
    {
        _destinationIndex++;
        RadialNode nextDestination = _path.GetNode(_destinationIndex);

        if (nextDestination == null)
        {
            Debug.Log("Robber route finished!");
            CleanUp();
            return;
        }

        Vector3 randomPoint = nextDestination.RadialPosition;
        transform.LookAt(randomPoint);
        _ai.destination = randomPoint;
        _ai.SearchPath();
    }

    void CleanUp()
    {
        gameObject.SetActive(false);
    }
}