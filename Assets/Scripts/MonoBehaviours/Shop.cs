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

    Range _income;
    Range _incomeRate;
    int _cost;
    SHOP_STATUS _status;
    float _rating;

    GameObject[] _renderModels;
    //Ability[] _abilities;
    //ShopMetaData _metaData;

    public Shape Shape { get => _data.Shape; }

    void Awake()
    {
        var modelLength = _data.RenderModels.Length;
        _renderModels = new GameObject[modelLength];
        for (int i = 0; i < modelLength; i++)
        {
            print(i);
            _renderModels[i] = Instantiate(_data.RenderModels[i], transform, true);

        }
    }
    public void Build(Shape shape)
    {
        int i = 0;
        shape.ForEachTile(tile =>
        {
            _renderModels[i].transform.position = tile.position;            
            i++;
        });
        gameObject.SetActive(true);
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
