using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
public class RadialNode : MonoBehaviour
{
    public float radius;
    public Vector3 GetRandomPoint()
    {
        var point = Random.insideUnitSphere * radius;
        point.y = 0;
        return point + transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 255, 0, 0.8f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
