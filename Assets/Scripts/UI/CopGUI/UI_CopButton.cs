using My.Cops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CopButton : UI_PurchasableButton
{
    [SerializeField] GameEvent_CopResume _onSelectCop;
    [SerializeField] TMP_Text _name;
    [SerializeField] TMP_Text _cost;
    [SerializeField] TMP_Text _attackSpeed;
    [SerializeField] TMP_Text _damage;
    [SerializeField] TMP_Text _range;
    [SerializeField] Image _background;

    CopResume _copResume;
    Button _button;

    public void Set(CopResume resume)
    {
        Debug.Log("Testing" + resume);
        _copResume = resume;
        _name.SetText(resume.Name);
        _cost.SetText(resume.BaseSalaryString);
        _attackSpeed.SetText(resume.AttackSpeedString);
        _damage.SetText(resume.DamageString);
        _range.SetText("todo");
        _background.sprite = resume.CopData.UiSprite;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetPanel);
    }

    public override void Set(PurchasableData data) { }

    override public void SetPanel()
    {
        _onSelectCop.Raise(_copResume);

        // TODO: Not sure if we are still using shop builder for cops?
        // RefManager.ShopBuilder.Activate(_shopData);
    }

    void OnDestroy() => _button.onClick.RemoveListener(SetPanel);
}