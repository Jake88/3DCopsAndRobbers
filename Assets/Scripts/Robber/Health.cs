using My.ModifiableStats;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    ModifiableStat _maxHP;
    float _currentHP;

    public Action<GameObject> OnDeath;
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

    public bool TakeDamage(int amount, GameObject attacker = null)
    {
        _currentHP += amount;
        if (_currentHP < 0)
        {
            OnDeath?.Invoke(attacker);
            return true;
        }

        return false;
    }

    public bool Heal(int amount, GameObject healer = null)
    {
        if (_currentHP == _maxHP.Value) return false;

        _currentHP += amount;
        if (_currentHP > _maxHP.Value)
            _currentHP = _maxHP.Value;
        
        return true;
    }
}
