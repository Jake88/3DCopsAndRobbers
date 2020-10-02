﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditorInternal;

[System.Serializable]
public struct Range 
{
    public float Min;
    public float Max;

    public Range(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }

    public float Random { get { return UnityEngine.Random.Range(Min, Max); } }
    public int RandomInt { get { return Mathf.RoundToInt(UnityEngine.Random.Range(Min, Max)); } }
}
