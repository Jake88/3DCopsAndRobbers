using System.Collections.Generic;
using UnityEngine;

public class AbilityFactory : MonoBehaviour
{
    // Think about how we are going to work with these. Should there be different lists based on Tiers, etc?

    // Should there be different arrays that unlock at certain difficulty points in the game?
    
    [SerializeField] Ability[] availableAbilities;

    public Ability[] GetAbilities(AbilityPrerequisite[] restrictions, int numberOfAbilities = 1)
    {
        // Filter list based on restrictions

        // Pass filted list to the weighting system

        // Randomly fill new array of nuymberOfAbilities weights?

        var set = new List<Ability>();
        foreach (var ability in availableAbilities)
        {
            if (ability.IsUsable(restrictions)) set.Add(ability);
        }

        #region
        var chosenAbilities = new Ability[numberOfAbilities];
        for (int i = 0; i < numberOfAbilities; i++)
        {
            chosenAbilities[i] = availableAbilities[UnityEngine.Random.Range(0, availableAbilities.Length)];
        }
        #endregion

        return chosenAbilities;
    }
}
