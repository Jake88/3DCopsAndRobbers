using My.ModifiableStats;
using My.TargetSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CopAttack : MonoBehaviour
{
    [SerializeField] protected Transform _renderModel;

    protected const string ShootTriggerName = "Shoot";

    protected ModifiableStat _damage;
    protected ModifiableStat _attackSpeed;

    protected Animator _animator;
    protected Targeter _targeter;

    protected Quaternion _originalRotation;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _targeter = GetComponentInChildren<Targeter>();
    }

    public virtual void Initialise(int damage, float attackSpeed, float movementSpeed = 0)
    {
        _damage = new ModifiableStat(damage);
        _attackSpeed = new ModifiableStat(attackSpeed);
        _originalRotation = _renderModel.localRotation;
    }

    void Start()
    {
        GetComponentInChildren<Targeter>().onNewTargets += ChangeTarget;

        StartCoroutine(Attack());
    }

    protected abstract IEnumerator Attack();
    protected abstract void ChangeTarget(List<Robber> newTargets);
}
