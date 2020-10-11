using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    private int _amount;
    private string _activeModelKey;
    private float _timeUntilExpiry;
    private Dictionary<string, GameObject> _cashModels = new Dictionary<string, GameObject>();

    [SerializeField]
    private SphereCollider _collectCollider;


    public void Initilise(CashData cashData, int amount, Vector3 whereToDrop)
    {
        _amount = amount;
        _timeUntilExpiry = cashData.expiryTimer;
        transform.position = whereToDrop;

        _activeModelKey = cashData.name;
        if (!_cashModels.ContainsKey(_activeModelKey))
        {
            _cashModels.Add(cashData.name, Instantiate(cashData.model, transform.position, Quaternion.identity, this.transform) as GameObject);
        }

        MouseRaycast.ClickEvents.RegisterForRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);

        _cashModels[_activeModelKey].SetActive(true);
    }

    private void Update()
    {
        _timeUntilExpiry -= Time.deltaTime;
        if (_timeUntilExpiry < 0) Expire();
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
        CleanUp();
        return _amount;
    }

    private void Expire()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        MouseRaycast.ClickEvents.DeregisterForRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);
        _cashModels[_activeModelKey].SetActive(false);
        this.gameObject.SetActive(false);
        MasterObjectPooler.Instance.Release(gameObject, "CashPool");
    }
}
