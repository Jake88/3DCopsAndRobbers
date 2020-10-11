using UnityEngine;

public struct ShapeTile
{
    public Transform transform;
    public Renderer renderer;
    public Vector3 originalPosition;
}

public class Shape : MonoBehaviour
{
    [SerializeField] [Range(0, 3)] int _rotations = 0;
    [SerializeField] GameObject _tileWrapper;

    int _currentRotation = 0;
    ShapeTile[] _tiles;
    BoxCollider _outterBoundsCollider;
    Transform _outterBoundsColliderTransform;

    public delegate void ForEach(Transform tile);

    public Bounds Bounds => _outterBoundsCollider.bounds;
    bool MaxRotationReached => _currentRotation == _rotations;

    void Awake()
    {
        _outterBoundsCollider = GetComponentInChildren<BoxCollider>(true);
        _outterBoundsColliderTransform = _outterBoundsCollider.transform;

        _tiles = new ShapeTile[_tileWrapper.transform.childCount];
        for (int i = 0; i < _tileWrapper.transform.childCount; i++)
        {
            _tiles[i].transform = _tileWrapper.transform.GetChild(i);
            _tiles[i].originalPosition = _tiles[i].transform.localPosition;
            _tiles[i].renderer = _tiles[i].transform.GetComponentInChildren<Renderer>();
        }
    }

    public void ForEachTile(ForEach callback)
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            callback(_tiles[i].transform);
        }
    }

    // Rotate using coords rather than transform rotate
    public void Rotate()
    {
        if (MaxRotationReached) ResetRotation();
        else ApplyNextRotation();
    }


    void ResetRotation()
    {
        _outterBoundsColliderTransform.Rotate(Vector3.up, (-90f * _currentRotation));
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].transform.localPosition = _tiles[i].originalPosition;
        }
        _currentRotation = 0;
    }

    void ApplyNextRotation()
    {
        _currentRotation++;
        _outterBoundsColliderTransform.Rotate(Vector3.up, 90f);
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].transform.localPosition = new Vector3(
                 _tiles[i].transform.localPosition.z,
                0,
                 _tiles[i].transform.localPosition.x * -1
            );
        }
    }

    // Temporary.
    public void SetColor(Color c)
    {
        foreach (var tile in _tiles)
        {
            tile.renderer.material.color = c;
        }
    }

}
