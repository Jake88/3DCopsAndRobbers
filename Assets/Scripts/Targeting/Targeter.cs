using My.ModifiableStats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    protected readonly static string NO_MASK = "Nothing";

    [Tooltip("If set, the overlapping sphere will be based off original position instead of current")]
    [SerializeField] protected bool _static = true;
    protected Vector3 _originalPosition;

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
        _targetBehaviour.Initilise(transform);
        _originalPosition = transform.position;
        StartCoroutine(FindTargetsWithinView(.25f));
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

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (var robber in _currentTargets)
        {
            Gizmos.DrawLine(transform.position, robber.transform.position);
        }
    }
}
