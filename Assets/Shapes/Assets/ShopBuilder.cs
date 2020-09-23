using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] private GridState gridState;

    private Shape[] shapes;
    private int currentShapeIndex = -1;
    private bool validPosition;
    public GameObject selectedShop; // change to private and Shop type when we start making actual shops


    // TODO: SHould probably make this a singleton class

    void Start()
    {
        shapes = transform.GetComponentsInChildren<Shape>(true);

        // RandomiseShape();
        SelectShape(1);

        //grid.SubscribeToPointChanges(OnGridPointChange);
    }

    // Callback function to run when grid changes. It updates the position of the current shape, and validates it
    public void OnGridPointChange(Vector3 currentPoint)
    {
        shapes[currentShapeIndex].transform.position = currentPoint;
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
        shapes[currentShapeIndex].ForEachTile((tile) => {
            if (gridState.IsSpaceOccupied(tile.position))
            {
                validPosition = false;
                tile.transform.gameObject.SetActive(false);
            } else
            {
                tile.transform.gameObject.SetActive(true);
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
        // Ensure next shape is different
        var newShape = currentShapeIndex;
        while (currentShapeIndex == newShape)
        {
            newShape = Random.Range(0, shapes.Length);
        }

        SelectShape(newShape);
    }

    /*
     * Using the supplied index, set the new shape as active
     */
    public void SelectShape(int newShapeIndex)
    {
        Vector3 currentPosition = new Vector3(0,0,0);
        // Turn off whatever the previous shape was, if we had one
        if (currentShapeIndex != -1)
        {
            currentPosition = shapes[currentShapeIndex].transform.position;
            shapes[currentShapeIndex].gameObject.SetActive(false);
        }

        currentShapeIndex = newShapeIndex;

        shapes[currentShapeIndex].gameObject.SetActive(true);
        shapes[currentShapeIndex].transform.position = currentPosition; // TODO: This was pointing to public current point from grid. Maybe it should be a scriptable Vector3????
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
                gridState.ToggleSpaceOccupied(tile.position);
                var go = Instantiate(selectedShop, tile.position, Quaternion.identity);
            });
            // Use the bounds of the shape to only update that area of our grid, for performance;
            Bounds bounds = shapes[currentShapeIndex].GetBounds();
            var guo = new GraphUpdateObject(bounds);
            AstarPath.active.UpdateGraphs(guo);

            RandomiseShape();
        }
    }

    private void OnDrawGizmos()
    {
        shapes[currentShapeIndex].ForEachTile(tile => {
            if (gridState.IsSpaceOccupied(tile.position))
            {
                Gizmos.color = new Color(1, 0, 0);
            }
            else
            {
                Gizmos.color = new Color(0, 1, 0);
            }
            Gizmos.DrawWireCube(tile.position, new Vector3(1, 1, 1));
        });
    }
}
