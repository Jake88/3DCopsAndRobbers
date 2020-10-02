using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mouseController : MonoBehaviour
{
/*    public GameObject obsticle;
    
    public Camera cam;
    public Grid grid;

    public GhostShop ghostShop;

    private Vector3 previousGridPoint = Vector3.down;

    // Start is called before the first frame update
    void Start()
    {
        obsticle.transform.localScale.Set(grid.tileSize, grid.tileSize, grid.tileSize);
        // TODO: Below is returning game object of indvGhostShop[0]. How to return the PARENT gameobj?
        // ghostShop = individualGhostShops[0].GetComponentInParent<Transform>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShapeSelector.current.Rotate();
            previousGridPoint = Vector3.down;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Floor")))
        {
            // ghostShop.ToggleActive(true);
            Vector3 gridPoint = grid.GetNearestPointOnGrid(hit.point);
            // If nothing has changed and it was invalid, exit
            if (!PositionChanged(gridPoint) && !ghostShop.previousValidity) return;
            // If nothing has changed and it was valid, OR it has changed and is now valid, listen for shop placement
            if (!PositionChanged(gridPoint) && ghostShop.previousValidity ||
                !Vector3.down.Equals(gridPoint) && ghostShop.Align(gridPoint))
            {
                previousGridPoint = gridPoint;
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceShop();
                }
            }
        } else
        {
            // ghostShop.ToggleActive(false);
        }
    }


    private void PlaceShop()
    {
        for (int i = 0; i < ShapeSelector.current.size; i++)
        {
            grid.ToggleSpaceOccupied(ghostShop.children[i].transform.position.x, ghostShop.children[i].transform.position.z);
            Instantiate(obsticle, ghostShop.children[i].transform.position, Quaternion.identity);
        }
        
        //surface.BuildNavMesh();
    } */
}
