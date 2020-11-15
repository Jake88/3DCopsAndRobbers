using My.ModifiableStats;
using UnityEngine;

[CreateAssetMenu(
    fileName = "_StealAmount(PA)",
    menuName = AssetMenuConstants.PASSIVE_ABILITY + "Steal amount"
    )]
public class PassiveStealAmountAbility : Ability
{
    [SerializeField] StatModifier _statModifer;

    public override void OnLoad(GameObject go)
    {
        var stealer = go.GetComponent<ISteal>();
        stealer.AmountToSteal.AddModifier(_statModifer);
    }

    public override void OnUnload(GameObject go)
    {
        var stealer = go.GetComponent<ISteal>();
        stealer.AmountToSteal.RemoveModifier(_statModifer);
    }
}