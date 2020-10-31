using System;
using UnityEngine;

[Serializable]
public class ShapeFragment
{
    [SerializeField] Vector2Int _position;
    [SerializeField] TileType _type = TileType.Building;

    public int X => _position.x;
    public int Y => _position.y;
    public Vector3 Position => new Vector3(X, 0, Y);
    public TileType Type => _type;
}

public enum TileType
{
    Empty,
    Unprotected,
    Unbuildable,
    Building
}

