using Pathfinding;
using QFSW.MOP2;
using UnityEngine;

public enum SHOP_STATUS
{
    NORMAL,
    CLOSED,
    DAMAGED
}

public class Shop : MonoBehaviour
{
    [SerializeField] ShopData _data;
    [SerializeField] ObjectPool _pool;
    [SerializeField] CashDropManager _cashDropManager;

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

    public void Build(Shape shape)
    {
        PositionShopTiles(shape);
        SetNewCashDropTimer();
        gameObject.SetActive(true);
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
            _income.RandomInt);
        _cashDropPositionIndex = ++_cashDropPositionIndex % _renderModels.Length;
    }

    void SetNewCashDropTimer()
    {
        _timeUntilNextDrop = _incomeRate.Random;
    }

    void PositionShopTiles(Shape shape)
    {
        int i = 0;
        shape.ForEachTile(tile =>
        {
            if (i == 0) transform.position = tile.position;
            _renderModels[i].transform.localPosition = tile.localPosition;
            i++;
        });
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
