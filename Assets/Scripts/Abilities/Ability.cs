using UnityEngine;

/*public abstract class Ability<AttachType, ApplyType> : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string _description;
    [SerializeField] float _probabilityWeight;
    [SerializeField] AbilityRequirements _requirements;

    public abstract void Attach(AttachType to);
    public abstract void Detatch(AttachType to);
    public abstract void Apply(ApplyType to, AttachType from);
}

public abstract class SimpleStatModiferAbility<AttachType, ApplyType> : Ability<AttachType, ApplyType>
{
    [SerializeField] StatModifier _statModifer;
}*/


public struct AbilityCallbackData
{
    public bool PreventEscape;
}

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



// I can have an interface that defines properties (such as getters) so I could do something like
/*
 * interface Mover {
 *   public ModifableStat MoveSpeed {get;}
 * }
 * 
 * class Robber : MonoBehaviour, Mover {
 *   ModifableStat _moveSpeed;
 *   public ModifableStat MoveSpeed => _moveSpeed;
 * }
 * 
 * class PassiveMoveSpeedAbility : SimpleStatModiferAbility<GameObject, GameObject> {
 *  public override void Apply(Mover reciever, GameObject sender) {
 *    reciever.MoveSpeed.AddModifer(_statModifer)
 *  }
 * }
 * 
 */
