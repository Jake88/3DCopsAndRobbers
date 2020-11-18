using My.ModifiableStats;
using UnityEngine;

namespace My.Abilities
{
    [CreateAssetMenu(
    fileName = "_MoveSpeed(PA)",
    menuName = AssetMenuConstants.PASSIVE_ABILITY + "Move speed"
    )]
    public class PassiveMoveSpeedAbility : Ability
    {
        [SerializeField] StatModifier _statModifer;

        public override void OnLoad(GameObject go)
        {
            var movement = go.GetComponent<IMove>();
            movement.ApplyMoveSpeedModifer(_statModifer);
        }

        public override void OnUnload(GameObject go)
        {
            var movement = go.GetComponent<IMove>();
            movement.RemoveMoveSpeedModifer(_statModifer);
        }
    }
}