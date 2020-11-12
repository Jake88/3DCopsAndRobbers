﻿using My.ModifiableStats;
using UnityEngine;

public class CircleTargeter : Targeter
{
#if UNITY_EDITOR
    float _viewRadiusForGizmos;
#endif

    [SerializeField] ModifiableStat _viewRadius = new ModifiableStat(1);
    [SerializeField] LayerMask _obstacleMask;

    void OnValidate()
    {
        _viewRadiusForGizmos = _viewRadius.BaseValue;
    }

    protected override void FindTargets()
    {
        _potentialTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius.Value, _targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            var targetTransform = target.GetComponent<Robber>();

            if (_obstacleMask.Equals(LayerMask.GetMask(NO_MASK)))
                _potentialTargets.Add(targetTransform);
            else
            {
                Vector3 dirToTarget = (targetTransform.transform.position - transform.position).normalized;
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

    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawWireSphere(transform.position, _viewRadiusForGizmos);
    }
}
