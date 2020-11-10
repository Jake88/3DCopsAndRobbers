using My.ModifiableStats;
using System.Collections;
using UnityEngine;

public class Robber : MonoBehaviour
{
    const float SecondsSpentCollectingMoney = 3f;

    // Ability flags. Probably not required???
    public bool RepeatOffender;

    [Header("Required game managers")]
    [SerializeField] CashDropManager _cashDropManager;
    PlayerMoney _playerMoney;

    [Header("Unique robber fields")]
    [SerializeField] RobberData _data;

    ModifiableStat _difficultyWeight;

    ModifiableStat _amountToSteal;
    int _amountStolen;

    // Sibling components
    Floater _floater;
    RobberMovement _movement;
    Health _health;

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

        _playerMoney = RefManager.PlayerMoney;
        _difficultyWeight = new ModifiableStat(_data.InitialDifficultyWeight);
        _amountToSteal = new ModifiableStat(_data.InitialStealAmount);
    }

    public void Spawn(Path path)
    {
        _movement.Reset(path);
        _difficultyWeight.Reset();
        _amountToSteal.Reset();
        _health.Reset();
    }

    // Purely a proxy fn to make less GetComponent calls from cop attacks.
    public bool ChangeHealth(int amount) => _health.ChangeHealth(amount);

    void Steal()
    {
        _playerMoney.LoseMoney(_amountToSteal.IntValue);
        _amountStolen = _amountToSteal.IntValue;
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
        if (_amountStolen > 0)
            _cashDropManager.DropCash(transform.position, _amountStolen, CashSource.Robber);

        CleanUp();
    }


    void Escape()
    {
        if (RepeatOffender)
            _movement.Restart();
        else
            CleanUp();
    }

    void CleanUp()
    {
        _amountStolen = 0;
        _data.Pool.Release(gameObject);
    }
}