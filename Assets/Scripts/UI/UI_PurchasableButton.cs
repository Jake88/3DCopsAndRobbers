using UnityEngine;

public abstract class UI_PurchasableButton : MonoBehaviour
{
    abstract public void Set(PurchasableData shopData);
    abstract public void SetPanel();
}
