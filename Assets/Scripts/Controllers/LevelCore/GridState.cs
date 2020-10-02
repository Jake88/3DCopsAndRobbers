using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelX_GridState",
    menuName = AssetMenuConstants.LEVEL_SETUP + "Grid state"
)]
public class GridState : ScriptableObject
{
    [SerializeField] private GridDimensions dimensions;
    [SerializeField] private bool[,] occupiedSpaces; // TODO: This bool will turn into a struct / data object that holds more information about the space such as current affects on the space, what is occupying it etc 

    private void OnEnable()
    { occupiedSpaces = new bool[(int)dimensions.runtimeDimensions.x, (int)dimensions.runtimeDimensions.z]; }

    public bool IsSpaceInvalid(Vector3 vector)
    {
        return dimensions.IsOutOfBounds(vector) || IsSpaceOccupied(vector);
    }

    public bool IsSpaceOccupied(Vector3 vector) => occupiedSpaces[(int)vector.x, (int)vector.z];

    public void ToggleSpaceOccupied(Vector3 vector)
    { occupiedSpaces[(int)vector.x, (int)vector.z] = !occupiedSpaces[(int)vector.x, (int)vector.z]; }
}
