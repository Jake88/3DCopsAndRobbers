using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LowestCurrentHpTB",
    menuName = AssetMenuConstants.TARGETING_BEHAVIOUR + "Lowest current HP"
)]
public class LowestCurrentHpTargetBehaviour : TargetBehaviour
{
    // Lowest HP targeting system.
    // Switch to whichever potential target has the lowest current HP.

    int SortByLowestHP(Robber a, Robber b)
    {
        if (a.CurrentHealth < b.CurrentHealth)
            return -1;
        else if (a.CurrentHealth > b.CurrentHealth)
            return 1;

        return 0;
    }

    protected override bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
    {
        potentialTargets.Sort(SortByLowestHP);

        for (int i = 0; i < maxTargets; i++)
        {
            _newTargets.Add(potentialTargets[i]);
        }

        return _newTargets.Except(currentTargets).Count<Robber>() > 0;
    }
}
