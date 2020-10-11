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
    [SerializeField] private GridDimensions _dimensions;
    [SerializeField] private bool[,] _occupiedSpaces; // TODO: This bool will turn into a struct / data object that holds more information about the space such as current affects on the space, what is occupying it etc 

    private void OnEnable()
    { 
        _occupiedSpaces = new bool[_dimensions.Width, _dimensions.Depth];
    }

    public bool IsSpaceInvalid(Vector3 vector) => _dimensions.IsOutOfBounds(vector) || IsSpaceOccupied(vector);
    public bool IsSpaceOccupied(Vector3 vector) => _occupiedSpaces[(int)vector.x, (int)vector.z];
    public void ToggleSpaceOccupied(Vector3 vector)
    { 
        _occupiedSpaces[(int)vector.x, (int)vector.z] = !_occupiedSpaces[(int)vector.x, (int)vector.z];
    }
}
