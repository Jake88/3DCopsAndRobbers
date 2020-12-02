using My.Abilities;
using QFSW.MOP2;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Robber_",
    menuName = AssetMenuConstants.ROBBER + "Robber"
)]
public class RobberData : ScriptableObject
{
    //[SerializeField] AI _intialAI;
    [SerializeField] Ability[] _initialAbilities;
    [SerializeField] ObjectPool _pool;
    [SerializeField] int _initialStealAmount;
    [SerializeField] float _initialMoveSpeed;
    [SerializeField] float _initialHP;
    [SerializeField] float _initialSpawnWeight;
    [SerializeField] float _initialDifficultyWeight;
    [SerializeField] float _ratingUnlockedAt;
    [SerializeField] AbilityFlags _abilityFlags;

    public Ability[] InitialAbilities => _initialAbilities;
    public ObjectPool Pool => _pool;
    public int InitialStealAmount => _initialStealAmount;
    public float InitialMoveSpeed => _initialMoveSpeed;
    public float InitialHP => _initialHP;
    public float InitialSpawnWeight => _initialSpawnWeight;
    public float InitialDifficultyWeight => _initialDifficultyWeight;
    public float RatingUnlockedAt => _ratingUnlockedAt;
    public AbilityFlags AbilityFlags => _abilityFlags;
}
