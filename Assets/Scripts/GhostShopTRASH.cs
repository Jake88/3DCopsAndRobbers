using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/*
public struct IndividualGhostShopComponents
{
    public Renderer renderer;
    public Transform transform;
}

public class GhostShop : MonoBehaviour
{
    private readonly Color invalidColor = new Color(.5f, 0, 0, .1f);
    private readonly Color validColor = new Color(0, .2f, .5f, .3f);

    public bool previousValidity { get; private set; } = false;

    public IndividualGhostShopComponents[] children;
    public Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        children = new IndividualGhostShopComponents[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            children[i].transform = transform.GetChild(i);
            children[i].renderer = children[i].transform.GetComponentInChildren<Renderer>(true);
        }
    }
    public bool Align(Vector3 primaryPoint)
    {
        // // TODO: CAN I CONVERT THE OCCUPIED CHECKS TO COLLIDER CHECKS???
        bool isValidArea = true;
        for (int i = 0; i < ShapeSelector.current.size; i++)
        {
            Vector3 newPoint = primaryPoint + ShapeSelector.current.shape[i];
            if (grid.IsSpaceOccupied(newPoint))
            {
                isValidArea = false;
                children[i].transform.gameObject.SetActive(false);
            }
            else
            {
                children[i].transform.gameObject.SetActive(true);
            }
            children[i].transform.position = newPoint;
        }
        SetColor(isValidArea);
        DeactiveExcessGhosts();
        return isValidArea;
    }

    private void DeactiveExcessGhosts()
    {
        for (int i = ShapeSelector.current.size; i < children.Length; i++)
        {
            children[i].transform.gameObject.SetActive(false);
        }
    }

    public void SetColor(bool isValid)
    {
        if (previousValidity == isValid) return;
        previousValidity = isValid;

        Color c = isValid ? validColor : invalidColor;
        foreach (var child in children)
        {
            child.renderer.material.color = c;
        }
    }

    public void ToggleActive(bool flag) {
        if (gameObject.activeSelf != flag) gameObject.SetActive(!gameObject.activeSelf);
    }
}


*/