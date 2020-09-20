using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Vector3 INVALID_POINT = new Vector3(1000, 1000, 1000);

    [SerializeField] private float tileSize;
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private Camera mainCamera;
    public Vector3 currentPoint { get; private set; }

    public delegate void GridPointChanged();
    private List<GridPointChanged> subscribers = new List<GridPointChanged>();

    bool[,] occupiedSpaces; // TODO: This bool will turn into a struct / data object that holds more information about the space such as current affects on the space, what is occupying it etc 

    void Start()
    {
        occupiedSpaces = new bool[dimensions.x, dimensions.y];
    }

    // TODO: Verify that this is a reasonable way to do this.
    // Do I need to return an 'unsubscribe' function that the other component to call to cancel?
    public void SubscribeToPointChanges(GridPointChanged callback)
    {
        subscribers.Add(callback);
    }

    void Update()
    {
        // Maintain a useful point of where our mouse is on the grid
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
        {
            var newPoint = GetNearestPoint(hit.point);
            if (IsOutOfBounds((int)newPoint.x, (int)newPoint.z))
            {
                newPoint = INVALID_POINT;
            }

            if (hasPositionChanged(newPoint))
            {
                currentPoint = newPoint;
                subscribers.ForEach((callback) => callback());
            }
        }
    }

    private bool hasPositionChanged(Vector3 newPoint) => !currentPoint.Equals(newPoint);

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

    public bool IsSpaceOccupied (int x, int z)
    {
        if (IsOutOfBounds(x, z)) return true;

        return occupiedSpaces[x, z];
    }
    public bool IsSpaceOccupied(Vector3 vector)
    {
        return IsSpaceOccupied((int)vector.x, (int)vector.z);
    }

    public void ToggleSpaceOccupied(Vector3 vector)
    {
        occupiedSpaces[(int)vector.x, (int)vector.z] = !occupiedSpaces[(int)vector.x, (int)vector.z];
    }
    private bool IsOutOfBounds(int x, int z) => x >= dimensions.x || x < 0 || z < 0 || z >= dimensions.y;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 0.8f);
        for(float i = 0; i < dimensions.x; i++)
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

        Gizmos.color = new Color(0, 0, 1f, 0.8f);
        Gizmos.DrawWireCube(currentPoint, new Vector3(1, 0, 1));
    }
}
