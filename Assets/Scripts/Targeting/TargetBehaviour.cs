using System.Collections.Generic;
using UnityEngine;

public abstract class TargetBehaviour : ScriptableObject
{
    protected List<Robber> _newTargets = new List<Robber>();
    protected int _targetCount;
    protected Transform _transform;

    // TODO: Add a Persistant serializefield boolean.
    // If true, the target will not switch from the current target while it is still a potential target
    // If it is no longer a potential target, target the next based on TargetUniquely.
    // This basically lets us remove the "BasicTB" completetly, as it becomes something that
    // all other TBs can opt into.
    // This should be something the user can toggle on or off on the cop
    // This also allows the ability to have cops with abilities that allow or prevent the toggling of this ability.

    public void Initilise(Transform thisTransform)
    {
        _transform = thisTransform;
    }

    public bool DetermineNewTargets(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
    {
        _newTargets.Clear();
        _targetCount = 0;

        // Handle no targets.
        if (potentialTargets.Count == 0)
        {
            if (currentTargets.Count == 0)
                return false;
            else
            {
                currentTargets.Clear();
                return true;
            }
        }

        bool newTargetsFound = false;

        // Handle less options than max targets.
        if (potentialTargets.Count <= maxTargets)
        {
            foreach (var target in potentialTargets)
            {
                _newTargets.Add(target);
                if (!currentTargets.Contains(target)) newTargetsFound = true;
            }
        }
        // Delegate to actual targeting system.
        else
            newTargetsFound = TargetUniquely(currentTargets, potentialTargets, maxTargets);

        if (!newTargetsFound) return false;

        currentTargets.Clear();
        foreach (var newTarget in _newTargets)
            currentTargets.Add(newTarget);

        return true;
    }

    protected abstract bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1);
}
