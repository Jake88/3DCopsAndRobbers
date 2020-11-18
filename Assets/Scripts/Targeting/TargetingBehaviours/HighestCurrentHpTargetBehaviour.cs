using System.Collections.Generic;
using System.Linq;

namespace My.TargetSystem
{
    public class HighestCurrentHpTargetBehaviour : TargetBehaviour
    {
        // Highest HP targeting system.
        // Switch to whichever potential target has the highest current HP.

        int SortByLowestHP(Robber a, Robber b)
        {
            if (a.CurrentHealth > b.CurrentHealth)
                return -1;
            else if (a.CurrentHealth < b.CurrentHealth)
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

            return _newTargets.Except(currentTargets).Count() > 0;
        }
    }
}