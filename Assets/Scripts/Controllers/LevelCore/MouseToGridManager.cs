using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseToGridManager : MonoBehaviour
{
    [SerializeField] private GameEvent_Vector3 changedGridEvent;
    [SerializeField] private GridDimensions dimensions;
    [SerializeField] private Camera mainCamera;
    public Vector3 currentPoint { get; private set; }

    void Start()
    {
        MouseRaycast.PointEvents.RegisterForPointRaycastHit(LayerMask.GetMask("Floor"), UpdateCurrentPoint);
    }

    public void UpdateCurrentPoint(RaycastHit hit)
    {
        // Maintain a useful point of where our mouse is on the grid
        var newPoint = GetNearestPoint(hit.point);
        if (dimensions.IsOutOfBounds(newPoint))
        {
            newPoint = Constants.INVALID_GRID_POSITION;
        }

        if (hasPositionChanged(newPoint))
        {
            currentPoint = newPoint;
            changedGridEvent.Raise(currentPoint);
        }
    }

    private bool hasPositionChanged(Vector3 newPoint) => !currentPoint.Equals(newPoint);

    private Vector3 GetNearestPoint(Vector3 position)
    {
        // Make sure the the passed in position is relative to our grid object's position
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / dimensions.runtimeTileSize);
        int y = 0; //Mathf.RoundToInt(position.y / tileSize);
        int z = Mathf.RoundToInt(position.z / dimensions.runtimeTileSize);

        Vector3 result = new Vector3(
            (float)x * dimensions.runtimeTileSize,
            (float)y * dimensions.runtimeTileSize,
            (float)z * dimensions.runtimeTileSize
        );

        result += transform.position;

        return result;
    }

    private void OnDestroy()
    {
        MouseRaycast.PointEvents.DeregisterForPointRaycastHit(LayerMask.GetMask("Floor"), UpdateCurrentPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 0.8f);
        for(float i = 0; i < dimensions.runtimeDimensions.x; i++)
        {
            float x = i * dimensions.runtimeTileSize;
            for (float j = 0; j < dimensions.runtimeDimensions.z; j++)
            {
                float z = j * dimensions.runtimeTileSize;
                var point = GetNearestPoint(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.05f);
                Gizmos.DrawWireCube(point, new Vector3(1, 0, 1));
            }
        }

        Gizmos.color = new Color(0, 0, 1f, 0.8f);
        Gizmos.DrawWireCube(currentPoint, new Vector3(1, 0, 1));
    }
}
