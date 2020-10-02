using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static Vector3 INVALID_POINT = new Vector3(1000, 1000, 1000);
    public Vector3 currentPoint { get; private set; }

    [SerializeField] private GameEvent changedGridEvent;
    [SerializeField] private bool showGizmos;
    [SerializeField] private float tileSize;
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private Camera mainCamera;

    void Update()
    {
        // Maintain a useful point of where our mouse is on the grid
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
        {
            var newPoint = GetNearestPoint(hit.point);
            if (IsOutOfBounds(newPoint))
            {
                newPoint = INVALID_POINT;
            }

            if (hasPositionChanged(newPoint))
            {
                currentPoint = newPoint;
                //changedGridEvent.Raise();
            }
        }
    }

    private Vector3 GetNearestPoint(Vector3 position)
    {
        // Make sure the the passed in position is relative to our grid object's position
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / tileSize);
        int y = 0; //Mathf.RoundToInt(position.y / tileSize);
        int z = Mathf.RoundToInt(position.z / tileSize);

        Vector3 result = new Vector3(
            (float)x * tileSize,
            (float)y * tileSize,
            (float)z * tileSize
        );

        result += transform.position;

        return result;
    }

    private bool hasPositionChanged(Vector3 newPoint) => !currentPoint.Equals(newPoint);
    private bool IsOutOfBounds(Vector3 v) => v.x >= dimensions.x || v.x < 0 || v.z < 0 || v.z >= dimensions.y;

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = new Color(1f, 1f, 0, 0.1f);
            for (float i = 0; i < dimensions.x; i++)
            {
                float x = i * tileSize;
                for (float j = 0; j < dimensions.y; j++)
                {
                    float z = j * tileSize;
                    var point = GetNearestPoint(new Vector3(x, 0f, z));
                    Gizmos.DrawSphere(point, 0.05f);
                    Gizmos.DrawWireCube(point, new Vector3(1, 0, 1));
                }
            }

            Gizmos.color = new Color(1f, 1f, 0);
            Gizmos.DrawWireCube(currentPoint, new Vector3(1, 0, 1));
        } 
    }
}
