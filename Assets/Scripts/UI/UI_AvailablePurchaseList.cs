using UnityEngine;

public class UI_AvailablePurchaseList : MonoBehaviour
{
    [SerializeField] UI_PurchasableButton _buttonPrefab;
    [SerializeField] PurchasableData[] _availableForPurchase;

    void Awake()
    {
        foreach (var data in _availableForPurchase)
        {
            var btn = Instantiate(_buttonPrefab, transform);
            btn.Set(data);
        }
    }
}
