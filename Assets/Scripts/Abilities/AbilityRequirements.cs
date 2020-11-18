using System;
using System.Collections.Generic;
using UnityEngine;
namespace My.Abilities
{
    [Serializable]
    public class AbilityRequirements
    {
        [SerializeField] AbilityPrerequisite[] _prerequisite;

        public bool HasRequirements(AbilityPrerequisite[] entityFeatures)
        {
            foreach (var prerequisite in _prerequisite)
            {
                bool requirementsMet = false;
                for (int i = 0; i < entityFeatures.Length; i++)
                {
                    if (prerequisite == entityFeatures[i])
                    {
                        requirementsMet = true;
                        break;
                    }
                }
                if (!requirementsMet) return false;
            }

            return true;
        }
    }
}