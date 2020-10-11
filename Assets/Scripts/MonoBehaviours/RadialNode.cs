using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
public class RadialNode : MonoBehaviour
{
    [SerializeField] float _radius = 0.5f;
    public Vector3 RandomPoint { 
        get
        {
            Vector3 point = Random.insideUnitSphere * _radius;
            point.y = 0;
            return point + transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 255, 0, 0.8f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
