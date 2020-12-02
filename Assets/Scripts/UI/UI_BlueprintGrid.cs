using UnityEngine;
using UnityEngine.UI;
using My.Buildables;
using My.Singletons;

namespace My.UI
{
    public class UI_BlueprintGrid : MonoBehaviour
    {
        [SerializeField] Color _unselectedColor;
        [SerializeField] Color _unbuildableColor;
        [SerializeField] Color _selectedColor;

        [SerializeField] Shape _testShape;

        Image[] _images;
        int _centerIndex;
        int _rowLength;

        void Awake()
        {
            _images = GetComponentsInChildren<Image>();
            _centerIndex = _images.Length / 2;
            _rowLength = Mathf.RoundToInt(Mathf.Sqrt(_images.Length));
        }

        void Start()
        {
            SetShape(_testShape);
            RefManager.BlueprintManager.OnShapeChanged += SetShape;
        }

        void SetShape(Shape shape)
        {
            foreach (var image in _images)
            {
                image.color = _unselectedColor;
            }

            foreach (var fragment in shape.Fragments)
            {
                int fragmentIndex = _centerIndex + (fragment.Y * _rowLength) + (fragment.X);

                switch (fragment.Type)
                {
                    case TileType.Building:
                        _images[fragmentIndex].color = _selectedColor;
                        break;
                    case TileType.Unbuildable:
                        _images[fragmentIndex].color = _unbuildableColor;
                        break;
                    case TileType.Unprotected:
                        _images[fragmentIndex].color = Color.green;
                        break;
                    default:
                        _images[fragmentIndex].color = _unselectedColor;
                        break;
                }
            }
        }
        void OnDestroy()
        {
            RefManager.BlueprintManager.OnShapeChanged -= SetShape;
        }
    }
}
