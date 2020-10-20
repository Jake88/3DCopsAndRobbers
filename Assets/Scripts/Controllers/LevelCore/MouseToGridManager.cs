using UnityEngine;

public class MouseToGridManager : MonoBehaviour
{
    [SerializeField] GameEvent_Vector3 _changedGridEvent;
    [SerializeField] GridDimensions _dimensions;
    [SerializeField] Camera _mainCamera;
    Vector3 _currentPoint;

    // DEBUG ONLY
    public bool WithGizmos;

    void Awake()
    {
        _mainCamera = _mainCamera ?? Camera.main;
    }
    void Start()
    {
        MouseRaycast.PointEvents.RegisterForRaycastHit(LayerMask.GetMask("Floor"), UpdateCurrentPoint);
    }

    public void UpdateCurrentPoint(RaycastHit hit)
    {
        // Maintain a useful point of where our mouse is on the grid
        var newPoint = GetNearestPoint(hit.point);
        if (_dimensions.IsOutOfBounds(newPoint))
        {
            newPoint = Constants.INVALID_GRID_POSITION;
        }

        if (hasPositionChanged(newPoint))
        {
            _currentPoint = newPoint;
            _changedGridEvent.Raise(_currentPoint);
        }
    }

    bool hasPositionChanged(Vector3 newPoint) => !_currentPoint.Equals(newPoint);

    Vector3 GetNearestPoint(Vector3 position)
    {
        // Make sure the the passed in position is relative to our grid object's position
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / _dimensions.TileSize);
        int y = 0; //Mathf.RoundToInt(position.y / tileSize);
        int z = Mathf.RoundToInt(position.z / _dimensions.TileSize);

        Vector3 result = new Vector3(
            (float)x * _dimensions.TileSize,
            (float)y * _dimensions.TileSize,
            (float)z * _dimensions.TileSize
        );

        result += transform.position;

        return result;
    }

    void OnDestroy()
    {
        MouseRaycast.PointEvents.DeregisterForRaycastHit(LayerMask.GetMask("Floor"), UpdateCurrentPoint);
    }

    void OnDrawGizmos()
    {
        if (!WithGizmos) return;
        Gizmos.color = new Color(1f, 1f, 0, 0.8f);
        for(float i = 0; i < _dimensions.Width; i++)
        {
            float x = i * _dimensions.TileSize;
            for (float j = 0; j < _dimensions.Depth; j++)
            {
                float z = j * _dimensions.TileSize;
                var point = GetNearestPoint(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.05f);
                Gizmos.DrawWireCube(point, new Vector3(1, 0, 1));
            }
        }

        Gizmos.color = new Color(0, 0, 1f, 0.8f);
        Gizmos.DrawWireCube(_currentPoint, new Vector3(1, 0, 1));
    }
}
