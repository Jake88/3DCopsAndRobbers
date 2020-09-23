using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelXXX.GridDimensions",
    menuName = AssetMenuConstants.LEVEL_SETUP + "Grid Dimensions",
    order = 1
)]
[Serializable]
public class GridDimensions : ScriptableObject
{
    // Todo: Possibly this should have y as well, incase we want a multi level level 
    [SerializeField]
    private int x;
    [NonSerialized]
    public int runtimeX;

    [SerializeField]
    private int z;
    [NonSerialized]
    public int runtimeZ;

    [SerializeField]
    private float tileSize;
    [NonSerialized]
    public float runtimeTileSize;

    public void OnAfterDeserialize()
    {
        runtimeX = x;
        runtimeZ = z;
        runtimeTileSize = tileSize;
    }

    public void OnBeforeSerialize() { }
}

