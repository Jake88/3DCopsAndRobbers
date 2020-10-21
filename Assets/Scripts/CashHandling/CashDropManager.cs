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
    [SerializeField] CashData[] _cashTypes;
    [SerializeField] string _poolName = "CashPool";

    public void OnEnable()
    {
    }

    public void DropCashWithRandomRadius(Vector3 entityPosition, int amount, CashSource source, float radius)
    {
        var random = Random.insideUnitSphere * radius;
        random.y = 0;
        DropCash(entityPosition + random, amount, source);
    }
    public void DropCash(Vector3 entityPosition, int amount, CashSource source)
    {
        // Determine what model we should use for the cash object
        var determinedType = _cashTypes[_cashTypes.Length - 1];
        for (int i = 0; i < _cashTypes.Length; i++)
        {
            if (amount < _cashTypes[i].maxAmountForType)
            {
                determinedType = _cashTypes[i];
                // Break out our for loop
                i = _cashTypes.Length;
            }
        }

        MasterObjectPooler.Instance.GetObjectComponent<Cash>(_poolName).Initilise(determinedType, amount, entityPosition, source);
    }
}
