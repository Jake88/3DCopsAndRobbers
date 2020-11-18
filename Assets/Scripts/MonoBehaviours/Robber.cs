using My.Abilities;
using My.ModifiableStats;
using My.Movement;
using My.ScriptableAnimation;
using My.Singletons;
using System.Collections;
using UnityEngine;

public class Robber : MonoBehaviour
{
    const float SecondsSpentCollectingMoney = 3f;

    // Ability flags. Probably not required???
    public bool RepeatOffender; // Move to a unique ability soon

    [Header("Unique robber fields")]
    [SerializeField] RobberData _data;
    [SerializeField] AbilityPrerequisite[] _abilityPrerequisites;

    ModifiableStat _difficultyWeight;

    Ability[] _abilities;


    // Sibling components
    Floater _floater;
    RobberMovement _movement;
    Health _health;
    Stealer _stealer;


    #region Health proxy functions
    // Purely a proxy fn to make less GetComponent calls from cop attacks.
    public bool ChangeHealth(int amount) => _health.ChangeHealth(amount);
    public int CurrentHealth => _health.CurrentHealth;
    #endregion

    void Awake()
    {
        _floater = GetComponentInChildren<Floater>();

        _movement = GetComponent<RobberMovement>();
        _movement.Initilise(_data.InitialMoveSpeed);
        _movement.OnBankReached += Steal;
        _movement.OnExitReached += Escape;

        _health = GetComponent<Health>();
        _health.Initilise(_data.InitialHP);
        _health.OnDeath += Die;

        _stealer = GetComponent<Stealer>();
        _stealer.Initilise(_data.InitialStealAmount);

        _difficultyWeight = new ModifiableStat(_data.InitialDifficultyWeight);
    }

    public void Spawn(Path path)
    {
        _movement.Reset(path);
        _difficultyWeight.Reset();
        _stealer.Reset();
        _health.Reset();

        var numberOfMods = UnityEngine.Random.Range(0, 3);
        _abilities = RefManager.AbilityFactory.GetRobberAbilities(_abilityPrerequisites, numberOfMods);

        foreach (var ability in _abilities)
            ability.OnLoad(gameObject);
    }

    void Steal()
    {
        _stealer.Steal();
        _floater.Pause();
        _movement.Pause();

        StartCoroutine(Resume());
    }

    IEnumerator Resume()
    {
        yield return new WaitForSeconds(SecondsSpentCollectingMoney);

        _floater.Resume();
        _movement.Resume();
    }

    void Die()
    {
        _stealer.DropCash();
        CleanUp();
    }


    void Escape()
    {
        bool preventEscape = false;
        
        foreach (var ability in _abilities)
        {
            var abilityData = ability.OnEscape(gameObject, null);
            if (abilityData.PreventEscape) preventEscape = true;
        }

        if (!preventEscape)
            CleanUp();
    }

    void CleanUp()
    {
        foreach (var ability in _abilities)
            ability.OnUnload(gameObject);

        _data.Pool.Release(gameObject);
    }
}
