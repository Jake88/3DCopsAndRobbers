using UnityEngine;

[CreateAssetMenu(
    fileName = "RepeatOffender(PA)",
    menuName = AssetMenuConstants.PASSIVE_ABILITY + "Repeat offender"
    )]
public class PassiveRepeatOffenderAbility : Ability
{
    AbilityCallbackData _data;

    void OnEnable()
    {
        _data = new AbilityCallbackData();
        _data.PreventEscape = true;
    }

    public override AbilityCallbackData OnEscape(GameObject go, GameObject exit)
    {
        var movement = go.GetComponent<RobberMovement>();
        var stealer = go.GetComponent<Stealer>();
        movement.Restart();
        stealer.ClearAmountStolen();
        return _data;
    }
}