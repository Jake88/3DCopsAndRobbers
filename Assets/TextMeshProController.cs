using TMPro;
using UnityEngine;

public class TextMeshProController : MonoBehaviour
{
    TextMeshProUGUI _tmp;

    void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int numberToConvert)
    {
        _tmp.text = numberToConvert.ToString();
    }

    public void UpdateText(string s)
    {
        _tmp.text = s;
    }
}
