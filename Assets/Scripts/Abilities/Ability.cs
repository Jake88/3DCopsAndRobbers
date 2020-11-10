using My.ModifiableStats;
using UnityEngine;

public abstract class Ability<AttachType, ApplyType> : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string _description;
    [SerializeField] AbilityRequirements _requirements;

    public abstract void Attach(AttachType to);
    public abstract void Detatch(AttachType to);
    public abstract void Apply(ApplyType to, AttachType from);
}

public class SimpleStatModiferAbility : Ability<Robber, ModifiableStat>
{
    [SerializeField] StatModifier _statModifer;

    public override void Apply(ModifiableStat reciever, Robber sender)
    {
        reciever.AddModifier(_statModifer);
    }

    public override void Attach(Robber to)
    {
        throw new System.NotImplementedException();
    }

    public override void Detatch(Robber to)
    {
        throw new System.NotImplementedException();
    }
}
