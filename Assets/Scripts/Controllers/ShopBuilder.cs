using My.Buildables;
using My.Singletons;
using UnityEngine;

namespace My.Buildables
{
    public class ShopBuilder : MonoBehaviour
    {
        [SerializeField] GridState _gridState;

        ConstructionShop _constructionShop;
        BuildingData _currentShop;
        bool _isEnabled; // Probably not a thing

        // DUMMY STUFF TO TEST
        public BuildingData[] testBuildingData;
        public BuildingData cop;

        void Awake() => _constructionShop = GetComponentInChildren<ConstructionShop>();

        void Start()
        {
            _constructionShop.gameObject.SetActive(false);
            ToggleActive(false);
        }

        void Update()
        {
            if (_isEnabled && Input.GetButtonDown("Fire1"))
            {
                    Build();
            }
            else if (Input.GetKeyDown(KeyCode.Comma))
            {
                Activate(cop);
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

            _constructionShop.SetShape(RefManager.BlueprintManager.CurrentShape);

            ToggleActive(true);
        }

        public void Deactivate()
        {
            _currentShop = null;
            ToggleActive(false);
        }

        void ToggleActive(bool enable)
        {
            _isEnabled = enable;
            _constructionShop.gameObject.SetActive(enable);
        }

        public void Build()
        {
            if (_constructionShop.IsValid && RefManager.PlayerMoney.CanAfford(_currentShop.InitialCost))
            {
                RefManager.PlayerMoney.SpendMoney(_currentShop.InitialCost);
                _currentShop.Pool.GetObjectComponent<IBuildable>().Build(_constructionShop);

                if (!_currentShop.Shape)
                {
                    RefManager.BlueprintManager.RandomiseShape();
                }

                _currentShop = null;
                ToggleActive(false);
            }
        }
    }
}