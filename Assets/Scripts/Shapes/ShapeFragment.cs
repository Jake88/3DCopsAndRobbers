using System;
using UnityEngine;

[Serializable]
public class ShapeFragment
{
    [SerializeField] Vector2Int _position;
    [SerializeField] TileType _type;

    public int X => _position.x;
    public int Y => _position.y;
    public Vector3 Position => new Vector3(X, 0, Y);
    public TileType Type => _type;

    public ShapeFragment(Vector2Int pos, TileType type)
    {
        _position = pos;
        _type = type;
    }
}

public enum TileType
{
    Empty,
    Unprotected,
    Impassable,
    Building
}

