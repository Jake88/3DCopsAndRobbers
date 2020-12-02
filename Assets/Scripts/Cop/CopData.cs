using My.Buildables;
using My.Utilities;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Cop_",
    menuName = AssetMenuConstants.BUILDABLE + "Cop"
    )]
public class CopData : BuildingData
{
    [SerializeField] float _initialMovementSpeed;
    [SerializeField] float _initialAttackSpeed;
    [SerializeField] int _initialDamage;
    [SerializeField] Range _baseSalary;

    // [SerializeField] CopBehaviour[] _possibleCopBehaviours;
    // [SerializeField] TargetingBehaviour[] _possibleTargetingBehaviours;

    public float InitialMovementSpeed => _initialMovementSpeed;
    public float InitialAttackSpeed => _initialAttackSpeed;
    public int InitialDamage => _initialDamage;
    public Range BaseSalary => _baseSalary;
}
