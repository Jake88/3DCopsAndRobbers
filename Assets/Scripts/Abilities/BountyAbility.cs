using My.MoneySystem;
using UnityEngine;

namespace My.Abilities
{
    [CreateAssetMenu(
    fileName = "_Bounty(PA)",
    menuName = AssetMenuConstants.TRIGGER_ABILITY + "Bounty"
    )]
    public class BountyAbility : Ability
    {
        [SerializeField] CashDropManager _cashDropManager;
        [SerializeField] int _bounty;

        public override void OnDie(GameObject go, GameObject killer)
        {
            _cashDropManager.DropCash(go.transform.position, _bounty, CashSource.Bounty);
        }
    }
}