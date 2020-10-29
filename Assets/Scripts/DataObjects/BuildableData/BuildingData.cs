using QFSW.MOP2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BuildingData : PurchasableData
{
    // public ShopAbility[] abilities;
    [SerializeField] Shape _shape;
    [SerializeField] ObjectPool _pool;

    public Shape Shape { get => _shape; }
    public ObjectPool Pool { get => _pool; }
}
