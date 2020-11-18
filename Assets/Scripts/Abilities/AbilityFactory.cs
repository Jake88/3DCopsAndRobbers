using System.Collections.Generic;
using UnityEngine;

namespace My.Abilities
{
    public class AbilityFactory : MonoBehaviour
    {
        // Think about how we are going to work with these. Should there be different lists based on Tiers, etc?

        // Should there be different arrays that unlock at certain difficulty points in the game?

        [SerializeField] Ability[] availableSharedAbilities;
        [SerializeField] Ability[] availableRobberAbilities;
        [SerializeField] Ability[] availableCopAbilities;

        Ability[] GetAbilities(AbilityPrerequisite[] restrictions, int numberOfAbilities, Ability[][] lists)
        {
            var set = new List<Ability>();

            foreach (var abilityList in lists)
            {
                foreach (var ability in abilityList)
                    if (ability.IsUsable(restrictions)) set.Add(ability);
            }


            // Pass filted list to the weighting system

            // Randomly fill new array of nuymberOfAbilities weights?

            // Ability groups?


            if (numberOfAbilities > set.Count) numberOfAbilities = set.Count;

            #region
            var chosenAbilities = new Ability[numberOfAbilities];
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (i >= set.Count) break;
                chosenAbilities[i] = set[UnityEngine.Random.Range(0, set.Count)];
            }
            #endregion

            return chosenAbilities;
        }

        public Ability[] GetRobberAbilities(AbilityPrerequisite[] restrictions, int numberOfAbilities = 1)
        {
            return GetAbilities(
                restrictions, 
                numberOfAbilities, 
                new Ability[][] { availableSharedAbilities, availableRobberAbilities });
        }

        public Ability[] GetCopAbilities(AbilityPrerequisite[] restrictions, int numberOfAbilities = 1)
        {
            return GetAbilities(
                restrictions,
                numberOfAbilities,
                new Ability[][] { availableSharedAbilities, availableCopAbilities });
        }
    }
}