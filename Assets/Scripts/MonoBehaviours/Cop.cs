using My.Abilities;
using My.Buildables;
using My.Singletons;
using Pathfinding;
using UnityEngine;

public class Cop : MonoBehaviour, IBuildable
{
    [SerializeField] CopData _initialData;
    [SerializeField] AbilityPrerequisite[] _abilityPrerequisites;
    Ability[] _abilities;

    CopAttack _copAttack;

    void Awake()
    {
        _copAttack = GetComponent<CopAttack>();
    }

    void Start()
    {
        _copAttack.Initialise(
            _initialData.InitialDamage,
            _initialData.InitialAttackSpeed,
            _initialData.InitialMovementSpeed);


        // Move to Build
        var numberOfMods = 1;
        _abilities = RefManager.AbilityFactory.GetCopAbilities(_abilityPrerequisites, numberOfMods);
        foreach (var ability in _abilities)
            ability.OnLoad(gameObject);
    }

    public void Build(ConstructionShop constructionShop)
    {
        transform.position = constructionShop.transform.position;
        transform.Rotate(Vector3.up, constructionShop.CurrentRotation);

        gameObject.SetActive(true);
        // Use the bounds of the shape to only update that area of our grid, for performance;
        SyncNavMesh(constructionShop);
    }

    static void SyncNavMesh(ConstructionShop constructionShop)
    {
        Physics.SyncTransforms();
        var guo = new GraphUpdateObject(constructionShop.Bounds);
        AstarPath.active.UpdateGraphs(guo);
    }
}
