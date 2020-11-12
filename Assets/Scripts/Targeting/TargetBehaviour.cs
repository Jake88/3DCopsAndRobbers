using System.Collections.Generic;
using UnityEngine;

public abstract class TargetBehaviour : ScriptableObject
{
    protected List<Robber> _newTargets = new List<Robber>();
    protected int _targetCount;

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

        bool newTargetsFound = TargetUniquely(currentTargets, potentialTargets, maxTargets);

        if (!newTargetsFound) return false;

        currentTargets.Clear();
        foreach (var newTarget in _newTargets)
            currentTargets.Add(newTarget);

        return true;
    }

    protected abstract bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1);
}
