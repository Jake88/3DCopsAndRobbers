using QFSW.MOP2;
using UnityEngine;

public class ConstructionFragment : MonoBehaviour
{
    static int _buildingModelIndex = 0;     // Purely used to loop through the models and make sure we have a variety of the models used.
    [SerializeField] GameObject[] _buildingModels;
    GameObject _buildingRenderer;

    [SerializeField] GameObject _impassableModel;
    GameObject _impassableRenderer;

    Material _validMaterial;
    [SerializeField] Material _invalidMaterial;
    
    [SerializeField] GridState _gridState;

    bool _isValid;
    Vector3 _oiriginalPosition;

    MeshRenderer[] _renderers;


    void Awake()
    {
        _impassableRenderer = Instantiate(_impassableModel, transform);
        _buildingRenderer = Instantiate(_buildingModels[_buildingModelIndex], transform);
        _buildingModelIndex = (_buildingModelIndex + 1) % _buildingModels.Length;
    }

    void Start()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _validMaterial = _renderers[0].material;
    }

    public void Initilise(Vector3 newLocalPosition, TileType type)
    {
        _oiriginalPosition = newLocalPosition;
        transform.localPosition = newLocalPosition;
        ToggleCorrectModel(type);


        // Should we rotate the model this by a random amount,
        // And should we randomise the order of the models (in ConstructionSHop)
        /*void RandomiseArrayOrder()
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
        }*/
    }

    public void Rotate()
    {
        var nextX = transform.localPosition.z;
        var nextZ = transform.localPosition.x * -1;
        transform.localPosition = new Vector3(nextX, 0, nextZ);
        transform.Rotate(Vector3.up, 90);
    }

    public void ResetPosition()
    {
        transform.localPosition = _oiriginalPosition;
    }

    public bool ValidatePosition()
    {
        _isValid = true;
        if (_gridState.IsSpaceInvalid(transform.position))
        {
            // Out of bounds, or similar. Hide the render model?
            // _renderModel.SetActive(false)
            _isValid = false;
        }
        else if (_gridState.IsSpaceOccupied(transform.position))
        {
            _isValid = false;
        }

        if (_renderers != null && _renderers.Length > 0)
        {
            foreach (MeshRenderer mr in _renderers)
            {
                mr.material = _isValid ? _validMaterial : _invalidMaterial;
            }
        }

        return _isValid;
    }

    void ToggleCorrectModel(TileType type)
    {
        _buildingRenderer.SetActive(false);
        _impassableRenderer.SetActive(false);

        if (type == TileType.Impassable)
            _impassableRenderer.SetActive(true);
        if (type == TileType.Building)
            _buildingRenderer.SetActive(true);
    }
}
