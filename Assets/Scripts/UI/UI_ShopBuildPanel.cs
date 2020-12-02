using My.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopBuildPanel : MonoBehaviour
{
    // TODO: Pass in abilities "row" prefab. These will have to be populated / pooled and instantiated here into the panel to be dynamic
    /*[SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _title;*/

    [SerializeField] Button _close;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _subTitle;
    [SerializeField] Image _image;
    [SerializeField] TMP_Text _cost;
    [SerializeField] TMP_Text _income;
    [SerializeField] TMP_Text _incomeRate;
    [SerializeField] UI_BlueprintSection _blueprintSection;

    GameEventListener_ShopData _selectShopListener;
    CanvasGroup _canvasGroup;

    void Awake()
    {
        _selectShopListener = GetComponent<GameEventListener_ShopData>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _selectShopListener.Response.AddListener(Setup);
    }

    void Update()
    {
        if (_canvasGroup.alpha > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            _close.onClick.Invoke();
        }
    }

    public void ExitShopBuild()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        RefManager.ShopBuilder.Deactivate();
    }

    public void Setup(ShopData data)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        _title.SetText(data.ShopName);
        _subTitle.SetText(data.FlavourText);
        _cost.SetText(data.InitialCostString);
        _income.SetText(data.InitialIncomeString);
        _incomeRate.SetText(data.InitialIncomeDropSpeedString);
        _image.sprite = data.UiSprite;

        RefManager.BlueprintManager.SetShape(data.Shape);
        _blueprintSection.Setup(data);
    }

    void OnDestroy()
    {
        _selectShopListener.Response.RemoveListener(Setup);

    }
}
