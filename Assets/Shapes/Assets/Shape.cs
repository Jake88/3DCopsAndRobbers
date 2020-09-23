using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public struct ShapeTile
{
    public Transform transform;
    public Renderer renderer;
    public Vector3 originalPosition;
}

public class Shape : MonoBehaviour
{
    private int currentRotation = 0;
    private ShapeTile[] tiles;

    [SerializeField] public int rotations = 0;
    [SerializeField] private BoxCollider outterBoundsCollider;
    [SerializeField] private GameObject tileWrapper;

    public delegate void ForEach(Transform tile);

    void Start()
    {
        tiles = new ShapeTile[tileWrapper.transform.childCount];
        
        for (int i = 0; i < tileWrapper.transform.childCount; i++)
        {
            tiles[i].transform = tileWrapper.transform.GetChild(i);
            tiles[i].originalPosition = tiles[i].transform.localPosition;
            tiles[i].renderer = tiles[i].transform.GetComponent<Renderer>();
        }
    }

    public Bounds GetBounds() => outterBoundsCollider.bounds;

    public void ForEachTile(ForEach callback)
    {
        foreach (var tile in tiles)
        {
            // return the transform and the world position of the tile ignoring rotation
            callback(tile.transform);
        }
    }

    // Rotate using coords rather than transform rotate
    public void Rotate()
    {
        if (currentRotation == rotations)
        {
            outterBoundsCollider.transform.Rotate(Vector3.up, (-90f * currentRotation));
            foreach (var tile in tiles)
            {
                tile.transform.localPosition = tile.originalPosition;
            }
            currentRotation = 0;
        }
        else
        {
            currentRotation++;
            outterBoundsCollider.transform.Rotate(Vector3.up, 90f);
            foreach (var tile in tiles)
            {
                tile.transform.localPosition = new Vector3(
                    tile.transform.localPosition.z,
                    .5f,
                    tile.transform.localPosition.x * -1
                );
            }
        }
    }

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
