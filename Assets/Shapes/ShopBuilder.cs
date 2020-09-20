using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] private Grid grid;
    
    private Shape[] shapes;
    private int currentShapeIndex = -1;
    private bool validPosition;
    public GameObject selectedShop; // change to private and Shop type when we start making actual shops

    void Start()
    {
        shapes = transform.GetComponentsInChildren<Shape>(true);

        RandomiseShape();

        grid.SubscribeToPointChanges(OnGridPointChange);
    }

    // Callback function to run when grid changes. It updates the position of the current shape, and validates it
    void OnGridPointChange()
    {
        shapes[currentShapeIndex].transform.position = grid.currentPoint;
        Validate();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Build();
            Validate();
        }
        if (Input.GetMouseButtonDown(1))
        {
            shapes[currentShapeIndex].Rotate();
            Validate();
        }
        if (Input.GetKeyUp("."))
        {
            RandomiseShape();
            Validate();
        }
    }

    /* Check whether the current location of the shop is valid, using grid.IsSpaceOccupied
     * Turns off any invalid shape blocks, and sets valid blocks to red to indicate the placement is unavailable
     */
    private void Validate()
    {
        validPosition = true;
        shapes[currentShapeIndex].ForEachTile(tile => {
            if (grid.IsSpaceOccupied(tile.position))
            {
                validPosition = false;
                tile.gameObject.SetActive(false);
            } else
            {
                tile.gameObject.SetActive(true);
            }
        });

        // Likely just placeholder stuff
        if (validPosition)
        {
            shapes[currentShapeIndex].SetColor(new Color(0, .5f, .5f, .6f));
        }
        else
        {
            shapes[currentShapeIndex].SetColor(new Color(.8f, 0, 0, .3f));
        }
    }

    /*
     * Select a new shape at random. Maybe should belong in a different script.
     */
    public void RandomiseShape()
    {
        // Turn off whatever the previous shape was, if we had one
        if (currentShapeIndex != -1) shapes[currentShapeIndex].gameObject.SetActive(false);

        // Ensure next shape is different
        var previousIndex = currentShapeIndex;
        while (currentShapeIndex == previousIndex)
        {
            currentShapeIndex = Random.Range(0, shapes.Length);
        }

        shapes[currentShapeIndex].gameObject.SetActive(true);
        shapes[currentShapeIndex].transform.position = grid.currentPoint;
    }

    /*
     * Place our new shop. Currently just Instantiating dummy blocks in each spot.
     * This will be changed significantly, and I'll utilise pooling so there's less performance worries
     */
    public void Build()
    {
        if (validPosition)
        {
            shapes[currentShapeIndex].ForEachTile(tile => {
                grid.ToggleSpaceOccupied(tile.position);
                var go = Instantiate(selectedShop, tile.position, Quaternion.identity);
            });
            // Use the bounds of the shape to only update that area of our grid, for performance;
            Bounds bounds = shapes[currentShapeIndex].GetBounds();
            var guo = new GraphUpdateObject(bounds);
            AstarPath.active.UpdateGraphs(guo);
        }
    }
}
