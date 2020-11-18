﻿using UnityEngine;
namespace My.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected string _description;
        [SerializeField] protected float _probabilityWeight;
        [SerializeField] protected AbilityRequirements _requirements;

        public virtual bool IsUsable(AbilityPrerequisite[] requirements) => _requirements.HasRequirements(requirements);

        public virtual void OnLoad(GameObject go) { }
        public virtual void OnUnload(GameObject go) { }
        public virtual void OnRecieveHit(GameObject go, GameObject attacker) { }
        public virtual void OnAttack(GameObject go, GameObject target) { }
        public virtual void OnMove(GameObject go) { }
        public virtual void OnCollision(GameObject go, GameObject from) { }
        public virtual void ReachBank(GameObject go, GameObject bank) { }
        public virtual void OnExitBank(GameObject go, GameObject bank) { }
        public virtual void OnDie(GameObject go, GameObject killer) { }
        public virtual AbilityCallbackData OnEscape(GameObject go, GameObject exit) => new AbilityCallbackData();
    }
}