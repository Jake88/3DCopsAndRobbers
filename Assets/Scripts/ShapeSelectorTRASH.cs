using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class ShapeSelector
{
    private static ShopShape[] shapes = new[] {
        new ShapeTee()
    };

    public static ShopShape current = shapes[GenerateRandomIndex()];

    public static void RandomiseShape()
    {
        current = shapes[GenerateRandomIndex()];
    }

    private static int GenerateRandomIndex()
    {
        return Mathf.RoundToInt(Random.Range(1.0f, (float)shapes.Length)) - 1;
    }
}

public struct BoundingBox
{
    public float minX;
    public float minZ;
    public float maxX;
    public float maxZ;

    public static implicit operator BoundingBox(float initialValue)
    {
        return new BoundingBox()
        {
            minX = initialValue,
            minZ = initialValue,
            maxX = initialValue,
            maxZ = initialValue
        };
    }
    public static implicit operator BoundingBox(int initialValue)
    {
        return new BoundingBox()
        {
            minX = initialValue,
            minZ = initialValue,
            maxX = initialValue,
            maxZ = initialValue
        };
    }
}

public class ShopShape
{
    protected Vector3[][] shapes;
    private int rotationIndex = 0;

    public Vector3[] shape { get; private set; }
    public int size { get {return shape.Length;}}
    public BoundingBox bounds;

    public ShopShape(Vector3[][] shapes)
    {
        this.shapes = shapes;
        shape = this.shapes[rotationIndex];
        SetBounds();
    }
    public void Rotate()
    {
        rotationIndex++;
        if (rotationIndex >= shape.Length)
        {
            rotationIndex = 0;
        }
        shape = shapes[rotationIndex];
        SetBounds();
    }

    private void SetBounds()
    {
        BoundingBox bb = 0;
        for(int i = 0; i < size; i++)
        {
            if (shape[i].x < bb.minX) bb.minX = shape[i].x;
            if (shape[i].x > bb.maxX) bb.maxX = shape[i].x;
            if (shape[i].z < bb.minZ) bb.minZ = shape[i].z;
            if (shape[i].z > bb.maxZ) bb.maxZ = shape[i].z;
        }
        bounds = bb;
    }
}

public class ShapeTee : ShopShape
{
    public ShapeTee() : base(new[] {
        new[] {
            new Vector3(0, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1)
        },
        new[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0)
        },
        new[] {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, -1)
        },
        new[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 0)
        }
    }) { }
}