using My.ModifiableStats;
using My.Utilities;
using UnityEngine;

public class RobberSpawnDifficulty : MonoBehaviour
{
    // This class is a mystery. Maybe it should be scriptable object with a bunch of configurable options.
    /*
     * First option would be as simple as "amount to increase per grand heist"
     * But options might be increase based off some other state (like total time expired or mall rating etc...)
     * This COULD be solved by making special "modifiers" that can be applied to the modifiable stat?
     *    these modifiers would have to extend current modifiers and fetch these variables to then apply the value... but then they need to update constantly.. don't know. this is tricky...
     */

    [SerializeField] float _initialDifficulty;
    [SerializeField] Range _spawnDifficultyRange;

    ModifiableStat _difficulty;

    void Awake()
    {
        _difficulty = new ModifiableStat(_initialDifficulty);
    }

    public float DetermineSpawnDifficulty()
    {
        return _difficulty.Value * _spawnDifficultyRange.Random;
    }
}
