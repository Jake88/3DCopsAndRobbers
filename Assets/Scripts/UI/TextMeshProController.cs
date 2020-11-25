using System.Collections;
using TMPro;
using UnityEngine;

public class TextMeshProController : MonoBehaviour
{
    [SerializeField] float _flashColorReturnTime = 1f;
    [SerializeField] Color _flashColor = Color.red; // Make this private and set it in FlashColor based on paramter
    float _remainingFlashColorReturnTime;
    Coroutine _colorFadeCoroutine;

    TMP_Text _tmp;
    Color _originalColor;

    void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
        _originalColor = _tmp.color;
    }

    public void UpdateText(int numberToDisplay)
    {
        _tmp.SetText(numberToDisplay.ToString());
    }

    public void UpdateText(string textToDisplay)
    {
        _tmp.SetText(textToDisplay);
    }

    // Pass flash color in here via event.
    public void FlashColour()
    {
        if (_colorFadeCoroutine != null) StopCoroutine(_colorFadeCoroutine);
        _tmp.color = _flashColor;
        _remainingFlashColorReturnTime = _flashColorReturnTime;
        _colorFadeCoroutine = StartCoroutine(ColorFade());
    }

    IEnumerator ColorFade()
    {
        var fadeInTime = .1f;
        var initialFadeToColor = fadeInTime;
        while (initialFadeToColor > 0)
        {
            initialFadeToColor -= Time.deltaTime;
            var percentageRemaining = (fadeInTime - initialFadeToColor) / fadeInTime;

            var newColor = new Color(
                Mathf.Lerp(_originalColor.r, _flashColor.r, percentageRemaining),
                Mathf.Lerp(_originalColor.g, _flashColor.g, percentageRemaining),
                Mathf.Lerp(_originalColor.b, _flashColor.b, percentageRemaining)
                );

            _tmp.color = newColor;

            yield return null;
        }

        while (_remainingFlashColorReturnTime > 0)
        {
            _remainingFlashColorReturnTime -= Time.deltaTime;
            var percentageRemaining = (_flashColorReturnTime - _remainingFlashColorReturnTime) / _flashColorReturnTime;

            var newColor = new Color(
                Mathf.Lerp(_flashColor.r, _originalColor.r, percentageRemaining),
                Mathf.Lerp(_flashColor.g, _originalColor.g, percentageRemaining),
                Mathf.Lerp(_flashColor.b, _originalColor.b, percentageRemaining)
                );

            _tmp.color = newColor;

            yield return null;
        }
    }

}
