using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ShapeTile
{
    public Transform transform;
    public Renderer renderer;
}

public class Shape : MonoBehaviour
{
    [SerializeField] public int rotations  = 0;

    private int currentRotation = 0;
    private BoxCollider outterBoundsCollider;

    private ShapeTile[] tiles;

    public delegate void ForEach(Transform tileTransform);

    void Start()
    {
        outterBoundsCollider = GetComponent<BoxCollider>();
        tiles = new ShapeTile[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            tiles[i].transform = transform.GetChild(i);
            tiles[i].renderer = tiles[i].transform.GetComponent<Renderer>();
        }
    }

    public Bounds GetBounds() => outterBoundsCollider.bounds;

    public void Rotate()
    {
        currentRotation++;
        if (currentRotation > rotations)
        {
            transform.Rotate(Vector3.up, (-90f * currentRotation) + 90f);
            currentRotation = 0;
        }
        else
        {
            transform.Rotate(Vector3.up, 90f);
        }
    }

    public void ForEachTile(ForEach callback)
    {
        foreach (var tile in tiles)
        {
            callback(tile.transform);
        }
    }

    // Rotate using coords rather than transform rotate
    /*public void Rotate()
    {
        if (currentRotation == rotations)
        {
            currentRotation = 0;
            foreach (var tile in tiles)
            {
                tile.transform.localPosition = tile.originalPosition;
            }
        } 
        else
        {
            currentRotation++;
            foreach (var tile in tiles)
            {
                tile.transform.localPosition = new Vector3(
                    tile.transform.localPosition.z,
                    .5f,
                    tile.transform.localPosition.x * -1
                );
            }
        }
    }*/

    public Vector3[] GetWorldPositions()
    {
        var v3 = new Vector3[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            v3[i] = tiles[i].transform.position;
        }

        return v3;
    }

    // Temporary.
    public void SetColor(Color c)
    {
        foreach (var tile in tiles)
        {
            tile.renderer.material.color = c;
        }
    }

}
