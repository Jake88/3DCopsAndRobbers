using My.Abilities;
using My.Buildables;
using My.Utilities;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Cop_",
    menuName = AssetMenuConstants.BUILDABLE + "Cop"
    )]
public class CopData : BuildingData
{
    [Range(0, 3)]
    [SerializeField] float _initialMovementSpeed;
    [Range(0, 10)]
    [SerializeField] float _initialAttackSpeed;
    [Range(0, 1000)]
    [SerializeField] int _initialDamage;
    [SerializeField] int _baseSalary;
    [SerializeField] AbilityFlags _abilityFlags;
    [Range(0,10)]
    [SerializeField] int _randomAbilityCount;

    // [SerializeField] CopBehaviour[] _possibleCopBehaviours;
    // [SerializeField] TargetingBehaviour[] _possibleTargetingBehaviours;

    public float InitialMovementSpeed => _initialMovementSpeed;
    public float InitialAttackSpeed => _initialAttackSpeed;
    public int InitialDamage => _initialDamage;
    public int BaseSalary => _baseSalary;
    public AbilityFlags AbilityFlags => _abilityFlags;
    public int RandomAbilityCount => _randomAbilityCount;
}
