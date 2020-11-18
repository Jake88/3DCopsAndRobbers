using My.TargetSystem;
using UnityEngine;

namespace My.Abilities
{
    [CreateAssetMenu(
    fileName = "PersistantTargeting(PA)",
    menuName = AssetMenuConstants.PASSIVE_ABILITY + "Persistant targeting"
    )]
    public class PassivePersistantTargetingAbility : Ability
    {
        [SerializeField] bool _modifer = true;

        public override void OnLoad(GameObject go)
        {
            var targetingBehaviour = go.GetComponent<TargetBehaviour>();
            targetingBehaviour.PersistantTargeting = _modifer;
        }

        public override void OnUnload(GameObject go)
        {
            var targetingBehaviour = go.GetComponent<TargetBehaviour>();
            targetingBehaviour.PersistantTargeting = false;
        }
    }
}