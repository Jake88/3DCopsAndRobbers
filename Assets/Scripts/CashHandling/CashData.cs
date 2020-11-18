using UnityEngine;

namespace My.MoneySystem
{
    [CreateAssetMenu(
    fileName = "CashType_X",
    menuName = AssetMenuConstants.CASH + "Cash type"
)]
    public class CashData : ScriptableObject
    {
        public int maxAmountForType;
        public GameObject model;
        public float expiryTimer = 8;
        public float expireAnimationStartTime = 4;
    }
}