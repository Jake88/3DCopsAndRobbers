using My.Singletons;
using System.Collections.Generic;
using UnityEngine;

public class UI_CopCategoriesPanel : MonoBehaviour
{
    [SerializeField] UI_CopCategory _copCategoryPrefab;

    Dictionary<CopData, UI_CopCategory> _panels;

    public void UpdateCategories()
    {
        if (_panels == null)
        {
            // MIght be able to to go into Start method, but means RefManager.CopResumeManager.CopResumes would have to already be set in Awake.
            _panels = new Dictionary<CopData, UI_CopCategory>();

            foreach (var pair in RefManager.CopResumeManager.CopResumes)
            {
                _panels.Add(pair.Key, Instantiate(_copCategoryPrefab, transform));
                // TODO: Count is probably always < 5. We could cache 0-5 as string digits in a dictionary with the int as key.
                _panels[pair.Key].SetCategoryButton(pair.Value.Count.ToString(), pair.Key.ShopName);
            }
        }

        foreach (var pair in RefManager.CopResumeManager.CopResumes)
        {
            _panels[pair.Key].SetCategoryResumes(pair.Value);
        }
    }
}
