using My.ModifiableStats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    [SerializeField] protected LayerMask _targetMask;
    [SerializeField] protected TargetBehaviour _targetBehaviour;
    [SerializeField] protected ModifiableStat _maxTargets = new ModifiableStat(1);

    protected List<Robber> _currentTargets = new List<Robber>();
    protected List<Robber> _potentialTargets = new List<Robber>();
    protected int _previouslyCountedPotentialTargets;

    public Action<List<Robber>> onNewTargets;
    public List<Robber> CurrentTargets => _currentTargets;

    protected abstract void FindTargets();


    protected virtual void Start()
    {
        StartCoroutine(FindTargetsWithinView(.3f));
    }

    protected virtual IEnumerator FindTargetsWithinView(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    protected virtual void TargetChanged()
    {
        onNewTargets?.Invoke(_currentTargets);
    }

}

public class FieldOfView : Targeter
{
    #if UNITY_EDITOR
        float _viewRadiusForGizmos;
        float _viewAngleForGizmos;
    #endif

    [SerializeField] ModifiableStat _viewRadius = new ModifiableStat(1);
    [SerializeField] ModifiableStat _viewAngle = new ModifiableStat(45, 360);
    [SerializeField] LayerMask _obstacleMask;

    void OnValidate()
    {
        _viewRadiusForGizmos = _viewRadius.BaseValue;
        _viewAngleForGizmos = _viewAngle.BaseValue;
    }

    void Update()
    {
        foreach (var target in _currentTargets)
        {
            Debug.DrawLine(transform.position, target.transform.position);
        }
    }

    protected override void FindTargets()
    {
        _potentialTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius.Value, _targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            var targetTransform = target.GetComponent<Robber>();

            Vector3 dirToTarget = (targetTransform.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle.Value /2 )
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, _obstacleMask))
                    _potentialTargets.Add(targetTransform);
            }
        }

        if (_previouslyCountedPotentialTargets == _potentialTargets.Count) return;

        // Perform targeting behaviour
        if (_targetBehaviour.DetermineNewTargets(_currentTargets, _potentialTargets, _maxTargets.IntValue))
        {
            _previouslyCountedPotentialTargets = _potentialTargets.Count;
            TargetChanged();
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawWireSphere(transform.position, _viewRadiusForGizmos);

        Vector3 viewAngleA = DirFromAngle(-_viewAngleForGizmos / 2, false);
        Vector3 viewAngleB = DirFromAngle(_viewAngleForGizmos / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * _viewRadiusForGizmos / 4);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * _viewRadiusForGizmos);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * _viewRadiusForGizmos);
    }
}
