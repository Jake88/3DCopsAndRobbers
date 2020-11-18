using My.Buildables;
using UnityEngine;

public class ConstructionFragment : MonoBehaviour
{
    static int _buildingModelIndex = 0;     // Purely used to loop through the models and make sure we have a variety of the models used.
    [SerializeField] GameObject[] _buildingModels;
    [SerializeField] GameObject _unbuildableModel;
    [SerializeField] GameObject _invalidModel;

    [SerializeField] GridState _gridState;

    GameObject _buildingModel;
    bool _isValid;

    ShapeFragment _fragmentData;

    public TileType Type => _fragmentData.Type;

    void Awake()
    {
        _buildingModel = Instantiate(_buildingModels[_buildingModelIndex], transform);
        _buildingModelIndex = (_buildingModelIndex + 1) % _buildingModels.Length;
    }

    public void Initilise(ShapeFragment fragmentData)
    {
        _fragmentData = fragmentData;
        transform.localPosition = fragmentData.Position;
        ValidatePosition();

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

    public void SetInvalid()
    {
        TurnOffModels();
        _invalidModel.SetActive(true);
        _isValid = false;
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
        transform.localPosition = _fragmentData.Position;
    }

    void TurnOffModels()
    {
        _buildingModel.SetActive(false);
        _unbuildableModel.SetActive(false);
        _invalidModel.SetActive(false);
    }

    void TurnOnModel()
    {
        if (!_isValid)
            _invalidModel.SetActive(true);
        if (_fragmentData.Type == TileType.Unbuildable)
            _unbuildableModel.SetActive(true);
        if (_fragmentData.Type == TileType.Building)
            _buildingModel.SetActive(true);
    }

    public bool ValidatePosition()
    {
        TurnOffModels();
        _isValid = true;

        if (_gridState.IsSpaceInvalid(transform.position) || _gridState.IsSpaceOccupied(transform.position))
            _isValid = false;

        TurnOnModel();

        return _isValid;
    }
}
