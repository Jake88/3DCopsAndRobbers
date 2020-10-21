using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Cash : MonoBehaviour
{
    [Header("Required game managers")]
    PlayerMoney _playerMoney;

    [Header("Animation configuration")]
    [SerializeField] float _floatHeight = 1f;
    [SerializeField] float _floatSpeed = 1f;

    static Vector3 _DropOffset = new Vector3(0, .2f, 0);

    SphereCollider _collectCollider;

    CashSource _source;
    Vector3 _initialPosition;
    float _timeUntilExpiry;
    // bool _fromRobber;

    int _amount;

    string _activeModelKey;
    Dictionary<string, GameObject> _cashModels = new Dictionary<string, GameObject>();

    void Awake()
    {
        _collectCollider = GetComponent<SphereCollider>();
        _playerMoney = RefManager.PlayerMoney;
    }

    public void Initilise(CashData cashData, int amount, Vector3 whereToDrop, CashSource source)
    {
        _source = source;
        _amount = amount;
        _timeUntilExpiry = cashData.expiryTimer;
        transform.position = whereToDrop + _DropOffset;
        _initialPosition = whereToDrop;

        SetActiveModel(cashData);
        StartListeningForUserClick();
    }

    void Update()
    {
        // Float
        transform.position = Vector3.Lerp(
            transform.position,
            _initialPosition + (Vector3.up * _floatHeight),
            Time.deltaTime * _floatSpeed);

        // Expire
        _timeUntilExpiry -= Time.deltaTime;
        if (_timeUntilExpiry < 0) Expire();
    }

    void StartListeningForUserClick()
    {
        MouseRaycast.ClickEvents.RegisterForRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);
    }

    void SetActiveModel(CashData cashData)
    {
        _activeModelKey = cashData.name;
        if (!_cashModels.ContainsKey(_activeModelKey))
        {
            _cashModels.Add(cashData.name, Instantiate(cashData.model, transform.position, Quaternion.identity, this.transform) as GameObject);
        }
        _cashModels[_activeModelKey].SetActive(true);
    }

    public void DetectClicked(RaycastHit hit)
    {
        if (hit.collider.Equals(_collectCollider))
        {
            this.Collect();
        }
    }

    public int Collect()
    {
        switch (_source)
        {
            case CashSource.Robber:
                _playerMoney.RecoverMoney(_amount);
                break;
            case CashSource.Shop:
                _playerMoney.EarnMoney(_amount);
                break;
        }
        CleanUp();
        return _amount;
    }

    void Expire()
    {
        CleanUp();
    }

    void CleanUp()
    {
        MouseRaycast.ClickEvents.DeregisterForRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);
        _cashModels[_activeModelKey].SetActive(false);
        this.gameObject.SetActive(false);
        MasterObjectPooler.Instance.Release(gameObject, "CashPool");
    }
}
