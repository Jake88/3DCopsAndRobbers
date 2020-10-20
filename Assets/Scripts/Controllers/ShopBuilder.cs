using UnityEngine;
using Pathfinding;
using System.Collections;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] GridState _gridState;

    [SerializeField] Path[] _paths;

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
    }

    private void Start()
    {
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
    /* Check whether the current location of the shop is valid, using grid.IsSpaceOccupied
     * Turns off any invalid shape blocks, and sets valid blocks to red to indicate the placement is unavailable
     */
    IEnumerator Validate()
    {
        yield return null;

        _validPosition = true;

        _currentShape.ForEachTile((tile) => {
            if (_gridState.IsSpaceInvalid(tile.position))
            {
                _validPosition = false;
            }
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
            _currentShape.SetColor(new Color(0, .5f, .5f, .1f));
        }
        else
        {
            _currentShape.SetColor(new Color(.8f, 0, 0, .01f));
        }
    }

    /*
     * Select a new shape at random. Maybe should belong in a different script.
     */
    public void RandomiseShape()
    {
        if (_lockShape) return;

        // Ensure next shape is different
        Shape newShape = _currentShape;
        while (_currentShape == newShape)
        {
            newShape = _shapes[Random.Range(0, _shapes.Length)];
        }

        SelectShape(newShape);
    }

    /*
     * Using the supplied index, set the new shape as active
     */
    public void SelectShape(Shape newShape)
    {
        // Turn off whatever the previous shape was, if we had one
        if (_currentShape != null)
        {
            _currentShape.gameObject.SetActive(false);
        }
        _currentShape = newShape;
    }

    /*
     * Place our new shop. Currently just Instantiating dummy blocks in each spot.
     * This will be changed significantly, and I'll utilise pooling so there's less performance worries
     */
    public void Build()
    {
        if (_validPosition)
        {
            _currentShape.ForEachTile(tile =>
            {
                _gridState.ToggleSpaceOccupied(tile.position);
            });
            _currentShop.Pool.GetObjectComponent<Shop>().Build(_currentShape);
            StartCoroutine(UpdatePathCoroutine());
        }
    }

    IEnumerator UpdatePathCoroutine()
    {
        yield return new WaitForSeconds(.05f);
        Physics.SyncTransforms();
        // Use the bounds of the shape to only update that area of our grid, for performance;
        var guo = new GraphUpdateObject(_currentShape.Bounds);
        AstarPath.active.UpdateGraphs(guo);

        Deactivate();
        RandomiseShape();
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
