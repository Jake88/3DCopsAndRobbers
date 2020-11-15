using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityRequirements
{
    [SerializeField] AbilityPrerequisite[] _prerequisite;
    HashSet<AbilityPrerequisite> cachedPrerequisites;

    void Initilise()
    {
        cachedPrerequisites = new HashSet<AbilityPrerequisite>();
        foreach (var prereq in _prerequisite)
            cachedPrerequisites.Add(prereq);
    }

    public bool HasRequirements(AbilityPrerequisite[] requirements)
    {
        bool requirementsMet = true;

        if (cachedPrerequisites == null) Initilise();
        foreach (var requirement in requirements)
        {
            if (!cachedPrerequisites.Contains(requirement)) requirementsMet = false;
            break;
        }

        return requirementsMet;
    }
}
