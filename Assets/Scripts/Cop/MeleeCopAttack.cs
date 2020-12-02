using My.ModifiableStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCopAttack : CopAttack
{
    // Making an assumption that melee cops will only ever have 1 target.

    [SerializeField] ModifiableStat _meleeRangeRadius = new ModifiableStat(.5f);
    [SerializeField] ModifiableStat _movementSpeed;
    Rigidbody _rigidbody;

    Vector3 _originalPosition;

    public override void Initialise(int damage, float attackSpeed, float movementSpeed)
    {
        base.Initialise(damage, attackSpeed);
        _rigidbody = GetComponent<Rigidbody>();
        _movementSpeed = new ModifiableStat(movementSpeed);
        _originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (_targeter.CurrentTargets.Count > 0)
        {
            var targetPosition = _targeter.CurrentTargets[0].transform.position;
            _renderModel.LookAt(targetPosition);
            if (Vector3.Distance(transform.position, targetPosition) > _meleeRangeRadius.Value)
                _rigidbody.position -= (transform.position - targetPosition).normalized * _movementSpeed.Value * Time.fixedDeltaTime;
        }
        else if (transform.position != _originalPosition)
        {
            _renderModel.LookAt(_originalPosition);
            _rigidbody.position -= (transform.position - _originalPosition).normalized * _movementSpeed.Value * Time.fixedDeltaTime;

            if (Vector3.Distance(transform.position, _originalPosition) < .1f)
            {
                _renderModel.localRotation = _originalRotation;
                _rigidbody.position = _originalPosition;
            }
        }
    }

    protected override void ChangeTarget(List<Robber> newTargets)
    {
        foreach (var target in newTargets)
        {
            // print($"new targets found {target.name}");
        }
    }

    protected override IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            while (_targeter.CurrentTargets.Count == 0) yield return new WaitForSeconds(.1f);

            if (Vector3.Distance(transform.position, _targeter.CurrentTargets[0].transform.position) < _meleeRangeRadius.Value)
            {
                if (_targeter.CurrentTargets[0].ChangeHealth(-_damage.IntValue))
                {
                    // Robber killed. Add some meta stats and stuff to this cop.
                }
            }

            _animator.SetTrigger(ShootTriggerName);

            yield return new WaitForSeconds(_attackSpeed.Value);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _meleeRangeRadius.Value);
    }
}