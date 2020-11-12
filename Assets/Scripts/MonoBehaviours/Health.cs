using My.ModifiableStats;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    ModifiableStat _maxHP;
    float _currentHP;

    public Action OnDeath;
    public int CurrentHealth => Mathf.RoundToInt(_currentHP);

    public void Initilise(float initialHP)
    {
        _maxHP = new ModifiableStat(initialHP);
    }

    public void Reset()
    {
        _maxHP.Reset();
        _currentHP = _maxHP.Value;
    }

    public bool ChangeHealth(int amount)
    {
        _currentHP += amount;
        if (_currentHP < 0)
        {
            OnDeath?.Invoke();
            return true;
        }
        else if (_currentHP > _maxHP.Value)
            _currentHP = _maxHP.Value;

        return false;
    }
}
