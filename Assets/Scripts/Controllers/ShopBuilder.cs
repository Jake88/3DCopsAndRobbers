using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditorInternal;
using UnityEngine.InputSystem;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] private GridState gridState;

    private Shape[] shapes;
    private int currentShapeIndex = -1;
    private bool validPosition;
    private bool isEnabled; // Probably not a thing
    public GameObject selectedShop; // change to private and Shop type when we start making actual shops

    // DUMMY STUFF TO TEST CREATING CASH
    public CashDropManager cashDropManager;

    // TODO: SHould probably make this a singleton class

    void Awake()
    {
        shapes = transform.GetComponentsInChildren<Shape>(true);
        RandomiseShape();
        ToggleActive(false);
    }

    // Callback function to run when grid changes. It updates the position of the current shape, and validates it
    public void OnGridPointChange(Vector3 currentPoint)
    {
        shapes[currentShapeIndex].transform.position = currentPoint;

        if (isEnabled)
        {
            Validate();
        }
    }

    void Update()
    {
        if (isEnabled)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Build();
                Validate();
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                shapes[currentShapeIndex].Rotate();
                Validate();
            }
            if (Keyboard.current.periodKey.wasPressedThisFrame)
            {
                RandomiseShape();
                Validate();
            }
        }
        else
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                var x = new Range(1, 60);
                cashDropManager.DropCash(shapes[currentShapeIndex].transform.position, x.RandomInt);
            }
        }

        if (Keyboard.current.periodKey.wasPressedThisFrame)
        {
            ToggleActive(!isEnabled);
        }
    }

    private void ToggleActive(bool enable)
    {
        shapes[currentShapeIndex].ForEachTile((tile) => tile.transform.gameObject.SetActive(enable));
        isEnabled = enable;
        if (enable)
        {
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
            if (gridState.IsSpaceInvalid(tile.position))
            {
                validPosition = false;
                tile.transform.gameObject.SetActive(false);
            }
            else
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
        if (currentShapeIndex > -1)
        {
            shapes[currentShapeIndex].ForEachTile(tile =>
            {
                if (gridState.IsSpaceInvalid(tile.position))
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
}
