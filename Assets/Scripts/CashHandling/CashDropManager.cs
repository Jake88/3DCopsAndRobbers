using QFSW.MOP2;
using UnityEngine;

namespace My.MoneySystem
{
    [CreateAssetMenu(
    fileName = "CashDropManager",
    menuName = AssetMenuConstants.CASH + "Cash manager"
)]
    public class CashDropManager : ScriptableObject
    {
        [SerializeField] CashData[] _cashTypes;
        [SerializeField] ObjectPool _cashPool;

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

            _cashPool.GetObjectComponent<Cash>().Initilise(determinedType, amount, entityPosition, source);
        }
    }
}