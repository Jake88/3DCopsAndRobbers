using Pathfinding;
using UnityEngine;

public class Cop : MonoBehaviour, IBuildable
{
    public void Build(ConstructionShop constructionShop)
    {
        transform.position = constructionShop.transform.position;
        transform.Rotate(Vector3.up, constructionShop.CurrentRotation);

        gameObject.SetActive(true);
        // Use the bounds of the shape to only update that area of our grid, for performance;
        SyncNavMesh(constructionShop);
    }

    static void SyncNavMesh(ConstructionShop constructionShop)
    {
        Physics.SyncTransforms();
        var guo = new GraphUpdateObject(constructionShop.Bounds);
        AstarPath.active.UpdateGraphs(guo);
    }
}
