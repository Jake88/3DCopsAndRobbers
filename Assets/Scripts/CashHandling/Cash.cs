using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    private int amount;
    private string activeModelKey;
    private SphereCollider collectCollider;
    private float timeUntilExpiry;

    private Dictionary<string, GameObject> cashModels = new Dictionary<string, GameObject>();

    public void Initilise(CashData cashData, int _amount, Vector3 whereToDrop)
    {
        amount = _amount;
        timeUntilExpiry = cashData.expiryTimer;
        transform.position = whereToDrop;

        activeModelKey = cashData.name;
        if (!cashModels.ContainsKey(activeModelKey))
        {
            cashModels.Add(cashData.name, Instantiate(cashData.model, transform.position, Quaternion.identity, this.transform) as GameObject);
        }

        MouseRaycast.ClickEvents.RegisterForPointRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);

        cashModels[activeModelKey].SetActive(true);
    }

    private void Update()
    {
        timeUntilExpiry -= Time.deltaTime;
        if (timeUntilExpiry < 0) Expire();
    }

    public void DetectClicked(RaycastHit hit)
    {
        if (hit.collider.Equals(GetComponent<SphereCollider>()))
        {
            Debug.Log("Collected: " + this.Collect());
        }
    }

    public int Collect()
    {
        CleanUp();
        return amount;
    }

    public void Expire()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        Debug.Log("Cleaning up cash instance");
        MouseRaycast.ClickEvents.DeregisterForPointRaycastHit(LayerMask.GetMask("Cash"), DetectClicked);
        cashModels[activeModelKey].SetActive(false);
        this.gameObject.SetActive(false);
        MasterObjectPooler.Instance.Release(gameObject, "Cash");
    }
}
