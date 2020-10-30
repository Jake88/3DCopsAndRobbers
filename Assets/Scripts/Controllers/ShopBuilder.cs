using UnityEngine;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] GridState _gridState;

    [SerializeField] Shape[] _baseShapes;
    int _currentBaseShapeIndex;

    ConstructionShop _constructionShop;
    BuildingData _currentShop;
    bool _isEnabled; // Probably not a thing
    Vector3 _currentMousePosition;

    // DUMMY STUFF TO TEST
    public BuildingData testBuildingData;

    Shape CurrentShape => _currentShop && _currentShop.Shape ? _currentShop.Shape : _baseShapes[_currentBaseShapeIndex];

    void Awake()
    {
        _constructionShop = GetComponentInChildren<ConstructionShop>();
    }

    void Start()
    {
        RandomiseShape();
        _constructionShop.gameObject.SetActive(false);
        ToggleActive(false);
    }

    void Update()
    {
        if (_isEnabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Build();
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ToggleActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            Activate(testBuildingData);
        }
    }

    // Callback function to run when grid changes.
    // Handling this here because when _constructionShop is turned off it no longer listens to these updates itself.
    // Could avoid this by having "currentPoint" be a static variable of GridDimensions.
    public void OnGridPointChange(Vector3 currentPoint)
    {
        _constructionShop.transform.position = currentPoint;
    }

    public void Activate(BuildingData shopData)
    {
        _currentShop = shopData;

        _constructionShop.SetShape(CurrentShape);

        ToggleActive(true);
    }

    void ToggleActive(bool enable)
    {
        _isEnabled = enable;
        _constructionShop.gameObject.SetActive(enable);
    }

    public void BuyNewBlueprint()
    {
        // if we only have 1 baseShape / blueprint to switch between, return;

        // spend the money on buying the new blueprint
        // RandomiseShape();
    }

    public void RandomiseShape()
    {
        if (_currentShop && _currentShop.Shape || _baseShapes.Length == 1) return;

        var oldShapeIndex = _currentBaseShapeIndex;

        while (oldShapeIndex == _currentBaseShapeIndex)
            _currentBaseShapeIndex = UnityEngine.Random.Range(0, _baseShapes.Length);

        _constructionShop.SetShape(CurrentShape);
    }

    public void Build()
    {
        if (_constructionShop.IsValid)
        {
            _currentShop.Pool.GetObjectComponent<Shop>().Build(_constructionShop);

            if (!_currentShop.Shape)
            {
                RandomiseShape();
            }

            _currentShop = null;
            ToggleActive(false);
        }
    }
}