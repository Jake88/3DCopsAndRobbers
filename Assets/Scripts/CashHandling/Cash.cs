using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Cash : MonoBehaviour
{
    static Vector3 _DropOffset = (Vector3.up * .2f);

    [SerializeField] float _floatHeight = 1f;
    [SerializeField] float _floatSpeed = 1f;
    [SerializeField] float _floatVariance = 0.4f;
    [SerializeField] float _rotateSpeed = 5f;

    SphereCollider _collectCollider;
    Vector3 _initialPosition;
    int _amount;
    float _timeUntilExpiry;

    string _activeModelKey;
    Dictionary<string, GameObject> _cashModels = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _collectCollider = GetComponent<SphereCollider>();
    }

    public void Initilise(CashData cashData, int amount, Vector3 whereToDrop)
    {
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
            _initialPosition + (Vector3.up * (Mathf.Sin(Time.fixedTime) * _floatVariance + _floatHeight)),
            Time.deltaTime * _floatSpeed);

        transform.Rotate(Vector3.up, Time.deltaTime * (360 / _rotateSpeed));

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
        print($"amount collected: {_amount}");
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
