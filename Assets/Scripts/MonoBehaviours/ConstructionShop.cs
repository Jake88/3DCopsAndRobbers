using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionShop : MonoBehaviour
{
    [SerializeField] ObjectPool _constructionTilePool;
    [SerializeField] GridState _gridState;

    Shape _currentShape;
    int _currentRotation;
    List<ConstructionFragment> _constructionTiles;
    BoxCollider _collider;
    bool _isValid;

    public Bounds Bounds => _collider.bounds;
    public bool IsValid => _isValid;

    public void OnMouseMove(Vector3 gridPosition)
    {
        transform.position = gridPosition;
        Validate();
    }

    public void SetShape(Shape shape)
    {
        _constructionTilePool.ReleaseAll();
        _constructionTiles.Clear();

        _currentShape = shape;

        foreach (var shapeFragment in _currentShape.Fragments)
        {
            
            var tile = _constructionTilePool.GetObjectComponent<ConstructionFragment>();
            
            tile.Initilise(shapeFragment.Position, shapeFragment.Type);
            _constructionTiles.Add(tile);
        }

        DetermineNewBounds();
        Validate();
    }

    void OnEnable()
    {
        if (!_currentShape)
        {
            _isValid = false;
            return;
        }

        // Probably need to get current mouse position on grid.
        Validate();
    }

    void Awake()
    {
        _constructionTiles = new List<ConstructionFragment>();
        _collider = GetComponent<BoxCollider>();    
    }

    void Start()
    {
        // Make sure all Construction Fragments sit under this.
        _constructionTilePool.ObjectParent.transform.parent = transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2")) DetermineRotatation();
    }

    void DetermineRotatation()
    {
        print("determining roation");
        if (_currentShape.Rotation == Rotation.None) return;

        print("((int)_currentShape.Rotation == _currentRotation" + ((int)_currentShape.Rotation == _currentRotation));

        if ((int)_currentShape.Rotation == _currentRotation)
            ResetRotationAndValidate();
        else
            RotateAndValidate();

        DetermineNewBounds();
    }

    void ResetRotationAndValidate()
    {
        _currentRotation = 0;
        _isValid = true;
        _constructionTiles.ForEach(tile => {
            tile.ResetPosition();
            if (!tile.ValidatePosition()) _isValid = false;
        });
    }

    void RotateAndValidate()
    {
        _isValid = true;
        _constructionTiles.ForEach(tile => {
            tile.Rotate();
            if (!tile.ValidatePosition()) _isValid = false;
        });
    }

    void Validate()
    {
        _isValid = true;
        _constructionTiles.ForEach(tile => {
            if (!tile.ValidatePosition()) _isValid = false; 
        });
    }

    void DetermineNewBounds()
    {
        Vector3 min = Vector3.up;
        Vector3 max = Vector3.up;

        _constructionTiles.ForEach(tile => {
            if (tile.transform.position.x < min.x) min.x = Mathf.RoundToInt(tile.transform.position.x);
            if (tile.transform.position.z < min.z) min.z = Mathf.RoundToInt(tile.transform.position.z);
            if (tile.transform.position.x < max.x) max.x = Mathf.RoundToInt(tile.transform.position.x);
            if (tile.transform.position.z < max.z) max.z = Mathf.RoundToInt(tile.transform.position.z);

            var size = max - min;
            var center = min + (size / 2);
           
            _collider.center = center;
            _collider.size = size;
        });
    }
}
