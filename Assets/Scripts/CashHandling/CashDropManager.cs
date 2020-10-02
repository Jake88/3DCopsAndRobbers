using QFSW.MOP2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CashDropManager",
    menuName = AssetMenuConstants.CASH + "Cash manager"
)]
public class CashDropManager : ScriptableObject
{
    [SerializeField]
    private CashData[] cashTypes;
    [SerializeField]
    private string poolName = "Cash";

    public void OnEnable()
    {
    }

    public void DropCashWithRandomRadius(Vector3 entityPosition, int amount, float radius)
    {
        var random = Random.insideUnitSphere * radius;
        DropCash(entityPosition + random, amount);
    }
    public void DropCash(Vector3 entityPosition, int amount)
    {
        // Determine what model we should use for the cash object
        var determinedType = cashTypes[cashTypes.Length - 1];
        for (int i = 0; i < cashTypes.Length; i++)
        {
            if (amount < cashTypes[i].maxAmountForType)
            {
                determinedType = cashTypes[i];
                // Break out our for loop
                i = cashTypes.Length;
            }
        }

        MasterObjectPooler.Instance.GetObjectComponent<Cash>(poolName).Initilise(determinedType, amount, entityPosition);
    }
}
