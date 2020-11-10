using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopAttack : MonoBehaviour
{
    const string ShootTriggerName = "Shoot";
    
    [SerializeField] CopData _initialData;
    [SerializeField] Transform _renderModel;

    Quaternion _originalRotation;

    Targeter _targeter;

    int _damage;
    float _attackSpeed;
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _targeter = GetComponentInChildren<Targeter>();
        _damage = _initialData.InitialDamage;
        _attackSpeed = _initialData.InitialAttackSpeed;
        _originalRotation = _renderModel.localRotation;
    }

    void Start()
    {
        GetComponentInChildren<Targeter>().onNewTargets += ChangeTarget;

        StartCoroutine(Attack());
    }


    void ChangeTarget(List<Robber> newTargets)
    {
        foreach (var target in newTargets)
        {
            print($"new targets found {target.name}");
        }
    }

    void FixedUpdate()
    {
        if (_targeter.CurrentTargets.Count > 0)
            _renderModel.LookAt(_targeter.CurrentTargets[0].transform.position);
        else
            _renderModel.localRotation = _originalRotation;
    }

    IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            while (_targeter.CurrentTargets.Count == 0) yield return new WaitForSeconds(.1f);

            foreach (var _target in _targeter.CurrentTargets)
            {
                if (_target.ChangeHealth(-_damage))
                {
                    // Robber killed. Add some meta stats and stuff to this cop.
                }
            }

            _animator.SetTrigger(ShootTriggerName);

            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    public void MultiplyDamage(int multiplyModifer)
    {
        _damage *= multiplyModifer;
    }

    public void AddDamage(int modifer)
    {
        _damage += modifer;
        if (_damage < 0) _damage = 0;
    }
    public void MultiplyDamage(int multiplyModifer, float timeUntilExpires)
    {
        var deltaDamage = (_damage * multiplyModifer) - _damage;
        StartCoroutine(AddDamageWithExpiryCoroutine(deltaDamage, timeUntilExpires));
    }

    public void AddDamageWithExpiry(int addModifer, float timeUntilExpires)
    {
        StartCoroutine(AddDamageWithExpiryCoroutine(addModifer, timeUntilExpires));
    }

    IEnumerator AddDamageWithExpiryCoroutine(int addModifer, float timeUntilExpires)
    {
        if (_damage + addModifer < 0) 
            addModifer = -_damage;

        _damage += addModifer;
        yield return new WaitForSeconds(timeUntilExpires);
        _damage -= addModifer;
    }        

}
