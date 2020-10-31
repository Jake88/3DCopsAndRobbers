using Pathfinding;
using QFSW.MOP2;
using UnityEngine;

public enum SHOP_STATUS
{
    NORMAL,
    CLOSED,
    DAMAGED
}

public class Shop : MonoBehaviour, IBuildable
{
    [SerializeField] ShopData _data;
    [SerializeField] ObjectPool _pool;
    [SerializeField] CashDropManager _cashDropManager;
    [SerializeField] GridState _gridState;

    Range _income;
    Range _incomeRate;
    float _timeUntilNextDrop;

    int _cost;
    SHOP_STATUS _status;
    float _rating;

    GameObject[] _renderModels;
    int _cashDropPositionIndex = 0;
    //Ability[] _abilities;
    //ShopMetaData _metaData;

    public Shape Shape { get => _data.Shape; }

    void Awake()
    {
        var modelLength = _data.RenderModels.Length;
        _renderModels = new GameObject[modelLength];
        for (int i = 0; i < modelLength; i++)
        {
            _renderModels[i] = Instantiate(_data.RenderModels[i], transform, true);
        }

        _income = _data.InitialIncome;
        _incomeRate = _data.InitialIncomeDropSpeed;
    }

    public void Build(ConstructionShop constructionShop)
    {
        transform.position = constructionShop.transform.position;

        PositionShopTiles(constructionShop);
        gameObject.SetActive(true);
        // Use the bounds of the shape to only update that area of our grid, for performance;
        SyncNavMesh(constructionShop);

        SetNewCashDropTimer();
    }

    static void SyncNavMesh(ConstructionShop constructionShop)
    {
        Physics.SyncTransforms();
        var guo = new GraphUpdateObject(constructionShop.Bounds);
        AstarPath.active.UpdateGraphs(guo);
    }

    void Update()
    {
        if (ShouldDropCash()) DropCash();
    }

    bool ShouldDropCash()
    {
        _timeUntilNextDrop -= Time.deltaTime;
        return _timeUntilNextDrop <= 0;
    }

    void DropCash()
    {
        SetNewCashDropTimer();
        _cashDropManager.DropCash(
            _renderModels[_cashDropPositionIndex].transform.position,
            _income.RandomInt,
            CashSource.Shop);
        _cashDropPositionIndex = ++_cashDropPositionIndex % _renderModels.Length;
    }

    void SetNewCashDropTimer()
    {
        _timeUntilNextDrop = _incomeRate.Random;
    }

    void PositionShopTiles(ConstructionShop constructionShop)
    {
        int fragmentsPlaced = 0;
        foreach (var fragment in constructionShop.Fragments)
        {
            _gridState.ToggleSpaceOccupied(fragment.transform.position);

            if (fragment.Type == TileType.Building)
            {
                _renderModels[fragmentsPlaced].transform.localPosition = fragment.transform.localPosition;
                fragmentsPlaced++;
            }
        }
    }

    public void Sell()
    {
        gameObject.SetActive(false);
    }

    private void CleanUp()
    {
        _pool.Release(gameObject);
    }
}
