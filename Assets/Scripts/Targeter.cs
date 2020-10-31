
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Targeter<T> : MonoBehaviour
{
    protected HashSet<T> _targetables;
    protected T _currentTarget;
    
    [SerializeField] protected UnityEvent<T> _onNewTargets;

    public T Target => _currentTarget;

    void Awake()
    {
        _targetables = new HashSet<T>();
    }

    // Because OnTriggerExit doesn't get called when we disable our monobehaviours while they are still colliding.
    // This will be super inefficient. Might need to look into some form of event trigger.
    private void FixedUpdate()
    {
        T missingT = default(T);
        foreach (T t in _targetables)
        {
            if (!(t as MonoBehaviour).isActiveAndEnabled)
                missingT = t;
        }

        if (missingT != null)
        {
            _targetables.Remove(missingT);

            if (_targetables.Count == 0)
            {
                _currentTarget = default(T);
                _onNewTargets.Invoke(_currentTarget);
            }
            else
            {
                DetermineNewTarget();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var t = other.GetComponent<T>();
        if (t == null) return;

        _targetables.Add(t);
        DetermineNewTarget();
    }
    
  /*  void OnTriggerExit(Collider other)
    {
        var t = other.GetComponent<T>();
        if (t == null) return;

        print("handling the removal of go " + other.gameObject.GetInstanceID());


        _targetables.Remove(t);

        if (_targetables.Count == 0)
        {
            _currentTarget = default(T);
            _onNewTargets.Invoke(_currentTarget);
        } 
        else
        {
            DetermineNewTarget();
        }

    }*/

    protected abstract void DetermineNewTarget();
}
