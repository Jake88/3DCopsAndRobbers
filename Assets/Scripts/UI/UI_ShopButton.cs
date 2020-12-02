using My.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopButton : UI_PurchasableButton
{
    [SerializeField] GameEvent_ShopData _onSelectShop;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _cost;
    [SerializeField] TMP_Text _income;
    [SerializeField] TMP_Text _incomeRate;
    [SerializeField] Image _background;

    ShopData _shopData;
    Button _button;

    override public void Set(PurchasableData shopData)
    {
        _shopData = (ShopData)shopData;
        _title.SetText(_shopData.ShopName);
        _cost.SetText(_shopData.InitialCostString);
        _income.SetText(_shopData.InitialIncomeString);
        _incomeRate.SetText(_shopData.InitialIncomeDropSpeedString);
        _background.sprite = _shopData.UiSprite;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetPanel);
    }

    override public void SetPanel()
    {
        _onSelectShop.Raise(_shopData);
        RefManager.ShopBuilder.Activate(_shopData);
    }

    void OnDestroy() => _button.onClick.RemoveListener(SetPanel);
}
