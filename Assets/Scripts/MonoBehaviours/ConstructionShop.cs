using Pathfinding;
using QFSW.MOP2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.Buildables
{
    public class ConstructionShop : MonoBehaviour
    {
        [SerializeField] ObjectPool _constructionTilePool;
        [SerializeField] GridState _gridState;
        [SerializeField] Movement.Path[] _paths;

        Shape _currentShape;
        int _currentRotation;
        List<ConstructionFragment> _constructionTiles;
        BoxCollider _collider;
        bool _isValid;

        public int CurrentRotation => _currentRotation * 90;
        public Bounds Bounds => _collider.bounds;
        public bool IsValid => _isValid;

        public ConstructionFragment[] Fragments => _constructionTiles.ToArray();

        public void OnMouseMove(Vector3 gridPosition)
        {
            transform.position = gridPosition;
            Validate();
        }

        public void SetShape(Shape shape)
        {
            _constructionTilePool.ReleaseAll();
            _constructionTiles.Clear();

            _currentShape = shape;

            foreach (var shapeFragment in _currentShape.Fragments)
            {
                var tile = _constructionTilePool.GetObjectComponent<ConstructionFragment>();

                tile.Initilise(shapeFragment);
                _constructionTiles.Add(tile);
            }

            DetermineNewBounds();
            Validate();
        }

        void Awake()
        {
            _constructionTiles = new List<ConstructionFragment>();
            _collider = GetComponent<BoxCollider>();
        }

        void Start()
        {
            // Make sure all Construction Fragments sit under this.
            _constructionTilePool.ObjectParent.transform.parent = transform;
            _collider.enabled = true;
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire2")) DetermineRotatation();
        }

        void DetermineRotatation()
        {
            if (_currentShape.Rotation == Rotation.None) return;

            if ((int)_currentShape.Rotation == _currentRotation)
                ResetRotationAndValidate();
            else
                RotateAndValidate();

            DetermineNewBounds();
        }

        void ResetRotationAndValidate()
        {
            _currentRotation = 0;
            _isValid = true;

            _constructionTiles.ForEach(tile =>
            {
                tile.ResetPosition();
                if (!tile.ValidatePosition()) _isValid = false;
            });

            StartCoroutine(PositionBlocksPath());
        }

        void RotateAndValidate()
        {
            _currentRotation++;

            _isValid = true;

            _constructionTiles.ForEach(tile =>
            {
                tile.Rotate();
                if (!tile.ValidatePosition()) _isValid = false;
            });
            StartCoroutine(PositionBlocksPath());
        }

        void Validate()
        {
            _isValid = true;

            _constructionTiles.ForEach(tile =>
            {
                if (!tile.ValidatePosition()) _isValid = false;
            });

            if (gameObject.activeSelf)
                StartCoroutine(PositionBlocksPath());
        }

        IEnumerator PositionBlocksPath()
        {
            yield return null;

            Physics.SyncTransforms();
            var guo = new GraphUpdateObject(Bounds);
            foreach (var path in _paths)
            {
                if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, path.PathAsAStarNodes, true))
                {
                    _isValid = false;
                    _constructionTiles.ForEach(tile => tile.SetInvalid());
                }
            }
        }

        void DetermineNewBounds()
        {
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            _constructionTiles.ForEach(tile =>
            {
                if (tile.transform.localPosition.x < min.x) min.x = Mathf.RoundToInt(tile.transform.localPosition.x);
                if (tile.transform.localPosition.z < min.z) min.z = Mathf.RoundToInt(tile.transform.localPosition.z);
                if (tile.transform.localPosition.x > max.x) max.x = Mathf.RoundToInt(tile.transform.localPosition.x);
                if (tile.transform.localPosition.z > max.z) max.z = Mathf.RoundToInt(tile.transform.localPosition.z);
            });

            var size = new Vector3(
                Mathf.Abs(min.x) + Mathf.Abs(max.x) + 1,
                1,
                Mathf.Abs(min.z) + Mathf.Abs(max.z) + 1
                );
            var center = (min + max) / 2;

            _collider.center = center;
            _collider.size = size;
        }
    }
}