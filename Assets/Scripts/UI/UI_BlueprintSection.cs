using UnityEngine;
using UnityEngine.UI;

public class UI_BlueprintSection : MonoBehaviour
{
    [SerializeField] Button _buyNewButton;

    public void Setup(ShopData data)
    {
        if (data.Shape)
            _buyNewButton.gameObject.SetActive(false);
        else
            _buyNewButton.gameObject.SetActive(true);
    }
}
