using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour, IRobber
{
    [Header("Required game managers")]
    [SerializeField] CashDropManager _cashDropManager;
    PlayerMoney _playerMoney;

    [Header("Unique robber fields")]
    [SerializeField] RobberData _data;

    int _amountToSteal;
    int _amountStolen;

    float _moveSpeed;
    float _maxHp;
    float _hp;
    float _difficultyWeight;

    // Other components
    RobberMovement _robberMovement;

    void Awake()
    {
        _robberMovement = GetComponent<RobberMovement>();
        _playerMoney = RefManager.PlayerMoney;
    }

    public void Spawn(Path path)
    {
        _amountToSteal = _data.InitialStealAmount;
        _moveSpeed = _data.InitialMoveSpeed;
        _maxHp = _data.InitialHP;
        _hp = _maxHp;
        _difficultyWeight = _data.InitialDifficultyWeight;
        _robberMovement.Spawn(_moveSpeed, path);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp < 0) Die();
    }

    void Die()
    {
        if (_amountStolen > 0)
            _cashDropManager.DropCash(transform.position, _amountStolen, CashSource.Robber);

        CleanUp();
    }

    public void Steal()
    {
        _playerMoney.LoseMoney(_amountToSteal);
        _amountStolen = _amountToSteal;
    }


    void Escape()
    {
        CleanUp();
    }

    void CleanUp()
    {
        _amountStolen = 0;
        _data.Pool.Release(gameObject);
    }
}
