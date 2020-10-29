using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "XXX_ShopData",
    menuName = AssetMenuConstants.SHOP + "Shop data"
)]
public class ShopData : BuildingData
{
    [SerializeField] Range _initialIncome;
    [SerializeField] Range _initialIncomeDropSpeed;

    public Range InitialIncome { get => _initialIncome; }
    public Range InitialIncomeDropSpeed { get => _initialIncomeDropSpeed; }

}
