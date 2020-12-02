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
    float _firstDayProRata;

    void Awake()
    {
        _copAttack = GetComponent<CopAttack>();
        RefManager.GameTime.PaydayEvent.AddListener(Payday);
    }

    public void Payday()
    {
        var amountToPay = Salary * _firstDayProRata;
        RefManager.PlayerMoney.SpendMoney(Mathf.RoundToInt(amountToPay));
        _firstDayProRata = 1;
    }

    public float Salary => _initialData.InitialCost / 4;
    public void Sell()
    {
        // EITHER
        // Use GameTime current time to figure out how much it will cost to "pay out" the cop his last day, based off the percentage of the day passed.
        // OR
        // Just make it cost Salary no matter what time the player sells it

        RefManager.GameTime.PaydayEvent.RemoveListener(Payday);
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

        _firstDayProRata = RefManager.GameTime.TimeUntilPayday / GameTime.TimeInAGameDay;

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
