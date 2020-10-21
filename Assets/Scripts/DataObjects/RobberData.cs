using QFSW.MOP2;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Robber_",
    menuName = AssetMenuConstants.ROBBER + "Robber"
)]
public class RobberData : ScriptableObject
{
    //[SerializeField] AI _intialAI;
    //[SerializeField] Ability[] _initialAbilities;
    [SerializeField] ObjectPool _pool;
    [SerializeField] int _initialStealAmount;
    [SerializeField] float _initialMoveSpeed;
    [SerializeField] float _initialHP;
    [SerializeField] float _initialDifficultyWeight;

    public ObjectPool Pool => _pool;
    public int InitialStealAmount => _initialStealAmount;
    public float InitialMoveSpeed => _initialMoveSpeed;
    public float InitialHP => _initialHP;
    public float InitialDifficultyWeight => _initialDifficultyWeight;
}
