using UnityEngine;

namespace My.Buildables
{
    [CreateAssetMenu(
        fileName = "Shape_X",
        menuName = AssetMenuConstants.GENERAL + "Shape"
    )]
    public class Shape : ScriptableObject
    {
        [SerializeField] Rotation _rotation;
        [SerializeField] ShapeFragment[] _shape = new ShapeFragment[4];

        public Rotation Rotation => _rotation;

        public ShapeFragment[] Fragments => _shape;
    }

    public enum Rotation
    {
        None = 0,
        One = 1,
        Full = 3
    }
}