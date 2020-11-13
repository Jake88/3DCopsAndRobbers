using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ClosestTB",
    menuName = AssetMenuConstants.TARGETING_BEHAVIOUR + "Closest HP"
)]
public class ClosestTargetBehaviour : TargetBehaviour
{
    // Closest targeting system.
    // Attack whatever target is the closest.

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

    protected override bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
    {
        potentialTargets.Sort(SortByDistance);

        for (int i = 0; i < maxTargets; i++)
        {
            _newTargets.Add(potentialTargets[i]);
        }

        return _newTargets.Except(currentTargets).Count<Robber>() > 0;
    }
}
