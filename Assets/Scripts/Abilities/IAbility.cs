using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AbilityType
{
    Passive,
    Active,
    Aura, //?
    Trigger //?
}

// https://forum.unity.com/threads/scriptableobject-skill-system-how-to-better-organize.539400/
// https://forum.unity.com/threads/how-to-implement-a-active-passive-skill-system-in-a-2d-sprite-game.234963/
// https://www.reddit.com/r/Unity3D/comments/b5dcz1/best_way_to_create_an_ability_system/
// https://www.reddit.com/r/gamedev/comments/676i72/best_way_to_implement_unique_passive_abilities_on/
// https://learn.unity.com/tutorial/create-an-ability-system-with-scriptable-objects#5cf5ecededbc2a36a1bd53b7

// A useful resource http://toqoz.fyi/unity-painless-inventory.html

public interface IPassiveAbility
{
    void Apply();
}

public interface IActiveAbility
{
    void Activate();
}

public interface IRobberActiveAbility
{
    void Activate();
}
