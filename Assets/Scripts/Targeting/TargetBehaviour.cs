using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This is an example of a more complex system
 * If there is a robber with less HP that the current target, focus it.
 * NOTE: This is be FLAT hp, not percentage.
Robber newTarget = _currentTarget;
foreach (var robber in _targetables)
{
   if (robber.hp < newTarget.hp)
   {
       newTarget = robber;
   }
}

 if (_currentRobber == newTarget) return;

_currentRobber = newTarget;
_onNewTargets.Invoke(_currentTarget);
 */


public class TargetBehaviour : MonoBehaviour
{
    List<Robber> _newTargets = new List<Robber>();

    public bool DetermineNewTargets(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
    {
        _newTargets.Clear();
        int targetCount = 0;


        print("determining new behaviour");
        // Basic targeting system.
        // If current target is still targetable, keep attacking it.

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

        foreach (var currentTarget in currentTargets)
        {
            if (potentialTargets.Contains(currentTarget))
            {
                targetCount++;
                // If our current targets are all still viable, just return;

                if (targetCount >= maxTargets) return false;
                // Make sure we add the current target incase we need to set new targets at the end.
                _newTargets.Add(currentTarget);
            }
        }

        foreach (var potentialTarget in potentialTargets)
        {
            if (!currentTargets.Contains(potentialTarget))
            {
                targetCount++;
                _newTargets.Add(potentialTarget);
                if (targetCount >= maxTargets) break;
            }
        }

        currentTargets.Clear();
        foreach (var newTarget in _newTargets)
            currentTargets.Add(newTarget);

        return true;
    }
}
