using My.Buildables;
using My.Utilities;
using UnityEngine;

[CreateAssetMenu(
    fileName = "XXX_ShopData",
    menuName = AssetMenuConstants.BUILDABLE + "Shop"
)]
public class ShopData : BuildingData
{
    [SerializeField] int _initialIncome;
    [SerializeField] float _initialIncomeDropSpeed;
    string _initialIncomeString;
    string _initialIncomeDropSpeedString;

    override protected void OnEnable()
    {
        base.OnEnable();
        _initialIncomeString = _initialIncome.ToString();
        _initialIncomeDropSpeedString = _initialIncomeDropSpeed.ToString();
    }

    public int InitialIncome { get => _initialIncome; }
    public string InitialIncomeString { get => _initialIncome.ToString(); }
    public float InitialIncomeDropSpeed { get => _initialIncomeDropSpeed; }
    public string InitialIncomeDropSpeedString { get => _initialIncomeDropSpeedString; }

}
