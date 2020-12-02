using My.Buildables;
using System;
using UnityEngine;

    public class BlueprintManager : MonoBehaviour
    {
        [SerializeField] Shape[] _basicShapes;
        
        int _currentBasicShapeIndex;

        public Action<Shape> OnShapeChanged;
        public Shape CurrentShape { get; private set; }

        void Start()
        {
            RandomiseShape();
        }

        public void SetShape(Shape specificShape)
        {
            var newShape = specificShape != null ? specificShape : _basicShapes[_currentBasicShapeIndex];
            if (CurrentShape != newShape)
            {
                CurrentShape = newShape;
                OnShapeChanged.Invoke(CurrentShape);
            }
        }

        public void SetShape()
        {
            CurrentShape = _basicShapes[_currentBasicShapeIndex];
            OnShapeChanged.Invoke(CurrentShape);
        }

        public void RandomiseShape()
        {
            var oldIndex = _currentBasicShapeIndex;
            while(_currentBasicShapeIndex == oldIndex)
            {
                _currentBasicShapeIndex = UnityEngine.Random.Range(0, _basicShapes.Length);
            }

            CurrentShape = _basicShapes[_currentBasicShapeIndex];
            OnShapeChanged.Invoke(CurrentShape);
        }
    }


