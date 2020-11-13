using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "BasicTB",
    menuName = AssetMenuConstants.TARGETING_BEHAVIOUR + "Basic",
    order = 0
)]
public class BasicTargetBehaviour : TargetBehaviour
{
    // Basic targeting system.
    // If current target is still targetable, keep attacking it.
    // If not, retarget based on distance
    protected override bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
    {
        potentialTargets.Sort(SortByDistance);

        foreach (var currentTarget in currentTargets)
        {
            if (potentialTargets.Contains(currentTarget))
            {
                _targetCount++;
                // If our current targets are all still viable, just return;

                if (_targetCount >= maxTargets) return false;
                // Make sure we add the current target incase we need to set new targets at the end.
                _newTargets.Add(currentTarget);
            }
        }

        foreach (var potentialTarget in potentialTargets)
        {
            if (!currentTargets.Contains(potentialTarget))
            {
                _targetCount++;
                _newTargets.Add(potentialTarget);
                if (_targetCount >= maxTargets) break;
            }
        }

        return true;
    }

    int SortByDistance(Robber a, Robber b)
    {
        var aDistance = Vector3.Distance(_transform.position, a.transform.position);
        var bDistance = Vector3.Distance(_transform.position, b.transform.position);

        if (aDistance < bDistance)
            return -1;
        else if (aDistance > bDistance)
            return 1;

        return 0;
    }
}
