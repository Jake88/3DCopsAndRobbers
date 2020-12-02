using UnityEngine;
using UnityEngine.UI;

public class UI_PanelToggler : MonoBehaviour
{
    [SerializeField] Color _selectedColor = new Color(46, 53, 89);
    [SerializeField] CanvasGroup _panelToToggle;
    [SerializeField] CanvasGroup[] _elementsToHide;

    Button _button;
    Image _image;
    Color _ogButtonColor;

    void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _button.onClick.AddListener(TogglePanel);
        _ogButtonColor = _image.color;
    }

    private void TogglePanel()
    {
        if (_panelToToggle.alpha > 0)
            Unselect();
        else
            Select();
    }

    public void Unselect()
    {
        HideGroup(_panelToToggle);
        _image.color = _ogButtonColor;
    }

    public void Select()
    {
        foreach (var el in _elementsToHide)
            HideGroup(el);

        ShowGroup(_panelToToggle);
        _image.color = _selectedColor;
    }

    void HideGroup(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
    void ShowGroup(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    void OnDestroy()
    {
        _button.onClick.RemoveListener(TogglePanel);
    }
}
