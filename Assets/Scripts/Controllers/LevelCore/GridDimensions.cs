using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelX_GridDimensions",
    menuName = AssetMenuConstants.LEVEL_SETUP + "Grid Dimensions",
    order = 1
)]
[Serializable]
public class GridDimensions : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private Vector3 dimensions;
    [NonSerialized]
    public Vector3 runtimeDimensions;

    [SerializeField]
    private float tileSize;
    [NonSerialized]
    public float runtimeTileSize;

    public bool IsOutOfBounds(Vector3 v) => (
           v.x >= runtimeDimensions.x
        || v.z >= runtimeDimensions.z
        || v.x < 0
        || v.z < 0
    );

    public void OnAfterDeserialize()
    {
        runtimeDimensions = dimensions;
        runtimeTileSize = tileSize;
    }

    public void OnBeforeSerialize() { }
}

