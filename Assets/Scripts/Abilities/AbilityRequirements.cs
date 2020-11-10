using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityRequirements
{
    [SerializeField] AbilityPrerequisite[] _prerequisite;
    Dictionary<AbilityPrerequisite, bool> cachedPrerequisites;

    void Initilise()
    {
        cachedPrerequisites = new Dictionary<AbilityPrerequisite, bool>();
        foreach (var prereq in _prerequisite)
            cachedPrerequisites.Add(prereq, true);
    }

    public bool HasRequirements(AbilityPrerequisite[] prerequisites)
    {
        bool requirementsMet = true;

        if (cachedPrerequisites == null) Initilise();
        foreach (var prerequisite in prerequisites)
        {
            if (!cachedPrerequisites[prerequisite]) requirementsMet = false;
            break;
        }

        return requirementsMet;
    }
}
