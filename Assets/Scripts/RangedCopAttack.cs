using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCopAttack : CopAttack
{
    void FixedUpdate()
    {
        if (_targeter.CurrentTargets.Count > 0)
            _renderModel.LookAt(_targeter.CurrentTargets[0].transform.position);
        else
            _renderModel.localRotation = _originalRotation;
    }

    protected override void ChangeTarget(List<Robber> newTargets)
    {
        foreach (var target in newTargets)
        {
            print($"new targets found {target.name}");
        }
    }

    protected override IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            while (_targeter.CurrentTargets.Count == 0) yield return new WaitForSeconds(.1f);

            foreach (var _target in _targeter.CurrentTargets)
            {
                if (_target.ChangeHealth(-_damage.IntValue))
                {
                    // Robber killed. Add some meta stats and stuff to this cop.
                }
            }

            _animator.SetTrigger(ShootTriggerName);

            yield return new WaitForSeconds(_attackSpeed.Value);
        }
    }
}
