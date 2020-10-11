using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelX_GridDimensions",
    menuName = AssetMenuConstants.LEVEL_SETUP + "Grid Dimensions",
    order = 1
)]
[Serializable]
public class GridDimensions : ScriptableObject
{
    [Range(1, 100)]
    [SerializeField] private int width = 10;
    [Range(1, 100)]
    [SerializeField] private int depth = 10;
    [Range(1, 3)]
    [SerializeField] private int height = 1;
    [Range(.5f, 2)]
    [SerializeField] private float tileSize = 1;

    public int Width { get => width; }
    public int Depth { get => depth; }
    public int Height { get => height; }
    public float TileSize { get => tileSize; }

    public bool IsOutOfBounds(Vector3 v) => (
           v.x >= Width
        || v.z >= Depth
        || v.x < 0
        || v.z < 0
    );
}

