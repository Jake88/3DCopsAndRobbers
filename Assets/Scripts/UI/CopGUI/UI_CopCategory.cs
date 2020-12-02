using My.Cops;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CopCategory : MonoBehaviour
{
    [SerializeField] TMP_Text _categoryName;
    [SerializeField] TMP_Text _categoryAmount;

    [SerializeField] UI_CopButton[] _copButtons;

    void Start()
    {
   //     GetComponentInParent<UI_CopCategoriesPanel>().ShowCategory()
     //   GetComponent<Button>().onClick.AddListener()    
    }


    public void SetCategoryButton(string amount, string name = null)
    {
        if (name != null) _categoryName.SetText(name);
        _categoryAmount.SetText(amount);
    }

    public void SetCategoryResumes(List<CopResume> resumes)
    {
        for (int i = 0; i < _copButtons.Length; i++)
        {
            if (i < resumes.Count)
            {
                _copButtons[i].Set(resumes[i]);
                _copButtons[i].gameObject.SetActive(true);
            }
            else
                _copButtons[i].gameObject.SetActive(false);
        }
    }
}