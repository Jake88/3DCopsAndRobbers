using System;
using UnityEngine;

[Serializable]
public class RadialNode
{
    [SerializeField] float _radius = 0.5f;
    [SerializeField] Vector3 _position = Vector3.zero;

    public float Radius => _radius;
    public Vector3 Position => _position;
    public Vector3 RadialPosition
    { 
        get
        {
            Vector3 point = UnityEngine.Random.insideUnitSphere * _radius;
            point.y = 0;
            return point + _position;
        }
    }
}
