using UnityEngine;

abstract public class PurchasableData : ScriptableObject
{
    [SerializeField] int _initialCost;
    [SerializeField] bool _unlocked;
    [SerializeField] GameObject[] _renderModels;
    [SerializeField] float _initialRating;
    [SerializeField] string _shopName;
    [SerializeField] string _flavourText;
    [SerializeField] Sprite _uiSprite;

    string _initialCostString;

    protected virtual void OnEnable()
    {
        _initialCostString = _initialCost.ToString();
    }

    public int InitialCost { get => _initialCost; }
    public string InitialCostString { get => _initialCostString; }
    public bool Unlocked { get => _unlocked; }
    public GameObject[] RenderModels { get => _renderModels; }
    public float InitialRating { get => _initialRating; }
    public string ShopName { get => _shopName; }
    public string FlavourText { get => _flavourText; }
    public Sprite UiSprite { get => _uiSprite; }
}
