using My.Abilities;
using My.Utilities;
using UnityEngine;

[CreateAssetMenu(
    fileName = "_GrandHeist",
    menuName = AssetMenuConstants.HEIST + "General heist"
)]
public class GrandHeist : ScriptableObject
{
    [SerializeField] string _heistName;
    [SerializeField] float _difficultyLevel; // Not sure if this will be used or if it's useful?

    [Tooltip("Pre-defined robbers in this heist.")]
    [SerializeField] RobberData[] _robbersInHeist;
    [Tooltip("Amount of points to spend on random additional robbers to spawn in this heist")]
    [SerializeField] float _additionalSpawnCurrency; // To create a completely random heist we can just use this value and least the robber list empty.
    [Tooltip("Abilities to be applied to each robber spawned in this heist.")]

    // Do the below abilities affect boss robbers? Should boss robbers have their own script that prevents random abilities being applied? :S Probably not.
    [SerializeField] Ability[] _abilities;
    [Tooltip("Number of random abilities to be applied to each robber spawned in this heist. Applied after specific abilities above are applied.")]
    [SerializeField] Range _randomAbilitiesToAddToEachRobber;

    public string HeistName => _heistName;
    public float DifficultyLevel => _difficultyLevel;
    public RobberData[] RobbersInHeist => _robbersInHeist;
    public float AdditionalSpawnCurrency => _additionalSpawnCurrency;
    public Ability[] Abilities => _abilities;
    public Range RandomAbilitiesToAddToEachRobber => _randomAbilitiesToAddToEachRobber;
}
