using UnityEngine;
using Pathfinding;
using System.Collections;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] GridState _gridState;
    [SerializeField] Path[] _paths;

    [SerializeField] Shape[] _baseShapes;

    ConstructionShop _constructionShop;
    BuildingData _currentShop;
    bool _isEnabled; // Probably not a thing
    Vector3 _currentMousePosition;

    // DUMMY STUFF TO TEST
    public BuildingData testBuildingData;

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
    public void OnGridPointChange(Vector3 currentPoint)
    {
        _constructionShop.transform.position = currentPoint;
    }

    public void Activate(BuildingData shopData)
    {
        _currentShop = shopData;
        if (shopData.Shape != null)
        {
            _constructionShop.SetShape(shopData.Shape);
        }
        
        ToggleActive(true);
    }

    void ToggleActive(bool enable)
    {
        _isEnabled = enable;
        _constructionShop.gameObject.SetActive(enable);
    }

    bool Validate()
    {
        bool isValid = _constructionShop.IsValid;
        if (isValid)
        {
            Physics.SyncTransforms();
            var guo = new GraphUpdateObject(_constructionShop.Bounds);
            foreach (var path in _paths)
            {
                if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, path.PathAsAStarNodes, true))
                    isValid = false;
            }
        }
        return isValid;
    }

    public void RandomiseShape()
    {
        if (_currentShop && _currentShop.Shape) return;

        _constructionShop.SetShape(_baseShapes[UnityEngine.Random.Range(0, _baseShapes.Length)]);;
    }

    /*
     * Place our new shop. Currently just Instantiating dummy blocks in each spot.
     * This will be changed significantly, and I'll utilise pooling so there's less performance worries
     */
    public void Build()
    {
        if (Validate())
        {
            /*_currentShape.ForEachTile(tile =>
            {
                _gridState.ToggleSpaceOccupied(tile.position);
            });
            _currentShop.Pool.GetObjectComponent<Shop>().Build(_currentShape);

            Deactivate();
            RandomiseShape();*/
        }
    }

    void Deactivate()
    {
        /*_lockShape = false;
        ToggleActive(false);*/
    }

    void OnDrawGizmos()
    {
        /*if (_currentShape != null && _isEnabled)
        {
            _currentShape.ForEachTile(tile =>
            {
                if (_gridState.IsSpaceInvalid(tile.position))
                {
                    Gizmos.color = new Color(1, 0, 0);
                }
                else
                {
                    Gizmos.color = new Color(0, 1, 0);
                }
                Gizmos.DrawWireCube(tile.position + new Vector3(0, 0.5f, 0), new Vector3(1, 1, 1));
            });
        }*/
    }
}



/*
public class ShopBuilder : MonoBehaviour
{
    [SerializeField] GridState _gridState;
    [SerializeField] Path[] _paths;
    [SerializeField] Transform[] _ghostTiles;

    Shape[] _shapes;

    Shape _currentShape;
    bool _lockShape;
    BuildingData _currentShop;
    bool _validPosition;
    bool _isEnabled; // Probably not a thing
    Vector3 _currentMousePosition;

    // DUMMY STUFF TO TEST
    public BuildingData testBuildingData;

    void Awake()
    {
        _shapes = transform.GetComponentsInChildren<Shape>(true);
        for (int i = 0; i < _ghostTiles.Length; i++)
        {
            _ghostTiles[i] = Instantiate(_ghostTiles[i]);
        }
    }

    void RandomiseArrayOrder()
    {
        Transform[] newArr = new Transform[_ghostTiles.Length];

        var random = Mathf.RoundToInt(Random.Range(0, _ghostTiles.Length - 1));
        for (int i = 0; i < _ghostTiles.Length; i++)
        {
            newArr[i] = _ghostTiles[(i + random) % _ghostTiles.Length];
            var randomRotation = Mathf.RoundToInt(Random.Range(1, 4)) * 90;
            newArr[i].Rotate(Vector3.up, 90 * randomRotation);
        }
        _ghostTiles = newArr;
    }

    private void Start()
    {
        foreach (var shape in _shapes)
        {
            shape.gameObject.SetActive(false);
        }
        RandomiseShape();
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
            if (Input.GetButtonDown("Fire2"))
            {
                _currentShape.Rotate();
                StartCoroutine(Validate());
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

    public void Activate(BuildingData shopData)
    {
        _currentShop = shopData;
        if (shopData.Shape != null)
        {
            _currentShape = shopData.Shape;
            _lockShape = true;
        }

        ToggleActive(true);
    }

    void ToggleActive(bool enable)
    {
        _currentShape.gameObject.SetActive(enable);
        _isEnabled = enable;

        var i = 0;
        _currentShape.ForEachTile((tile) =>
        {
            _ghostTiles[i].gameObject.SetActive(enable);
            i++;
        });

        if (enable)
        {
            _currentShape.transform.position = _currentMousePosition;
            StartCoroutine(Validate());
        }
    }

    // Callback function to run when grid changes. It updates the position of the current shape, and validates it
    public void OnGridPointChange(Vector3 currentPoint)
    {
        _currentMousePosition = currentPoint;

        if (_isEnabled)
        {
            _currentShape.transform.position = _currentMousePosition;
            StartCoroutine(Validate());
        }
    }
    *//* Check whether the current location of the shop is valid, using grid.IsSpaceOccupied
     * Turns off any invalid shape blocks, and sets valid blocks to red to indicate the placement is unavailable
     *//*
    IEnumerator Validate()
    {
        yield return null;

        _validPosition = true;

        int i = 0;
        _currentShape.ForEachTile((tile) => {
            _ghostTiles[i].position = tile.position;

            if (_gridState.IsSpaceInvalid(tile.position))
            {
                _validPosition = false;
            }
            i++;
        });

        if (_validPosition)
        {
            Physics.SyncTransforms();
            var guo = new GraphUpdateObject(_currentShape.Bounds);
            foreach (var path in _paths)
            {
                if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, path.PathAsAStarNodes, true))
                    _validPosition = false;
            }
        }

        // Likely just placeholder stuff
        if (_validPosition)
        {
        }
        else
        {
        }
    }

    *//*
     * Select a new shape at random. Maybe should belong in a different script.
     *//*
    public void RandomiseShape()
    {
        if (_lockShape) return;

        // Ensure next shape is different
        Shape newShape = _currentShape;
        while (_currentShape == newShape)
        {
            newShape = _shapes[UnityEngine.Random.Range(0, _shapes.Length)];
        }

        SelectShape(newShape);
        RandomiseArrayOrder();
    }

    *//*
     * Using the supplied index, set the new shape as active
     *//*
    public void SelectShape(Shape newShape)
    {
        // Turn off whatever the previous shape was, if we had one
        if (_currentShape != null)
        {
            _currentShape.gameObject.SetActive(false);
        }
        _currentShape = newShape;
    }

    *//*
     * Place our new shop. Currently just Instantiating dummy blocks in each spot.
     * This will be changed significantly, and I'll utilise pooling so there's less performance worries
     *//*
    public void Build()
    {
        if (_validPosition)
        {
            _currentShape.ForEachTile(tile =>
            {
                _gridState.ToggleSpaceOccupied(tile.position);
            });
            _currentShop.Pool.GetObjectComponent<Shop>().Build(_currentShape);

            Deactivate();
            RandomiseShape();
        }
    }

    void Deactivate()
    {
        _lockShape = false;
        ToggleActive(false);
    }

    void OnDrawGizmos()
    {
        if (_currentShape != null && _isEnabled)
        {
            _currentShape.ForEachTile(tile =>
            {
                if (_gridState.IsSpaceInvalid(tile.position))
                {
                    Gizmos.color = new Color(1, 0, 0);
                }
                else
                {
                    Gizmos.color = new Color(0, 1, 0);
                }
                Gizmos.DrawWireCube(tile.position + new Vector3(0, 0.5f, 0), new Vector3(1, 1, 1));
            });
        }
    }
}
*/