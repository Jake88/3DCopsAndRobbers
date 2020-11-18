using My.Buildables;
using My.Utilities;
using UnityEngine;

[CreateAssetMenu(
    fileName = "XXX_ShopData",
    menuName = AssetMenuConstants.BUILDABLE + "Shop"
)]
public class ShopData : BuildingData
{
    [SerializeField] Range _initialIncome;
    [SerializeField] Range _initialIncomeDropSpeed;

    public Range InitialIncome { get => _initialIncome; }
    public Range InitialIncomeDropSpeed { get => _initialIncomeDropSpeed; }

}
