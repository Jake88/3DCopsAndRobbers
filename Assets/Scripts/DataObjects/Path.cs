using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace My.Movement
{
    public class Path : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos = true;
        [SerializeField] RadialNode[] _path = new RadialNode[2];
        public List<GraphNode> PathAsAStarNodes { get; private set; }

        void Start()
        {
            PathAsAStarNodes = new List<GraphNode>();
            foreach (var node in _path)
            {
                PathAsAStarNodes.Add(AstarPath.active.GetNearest(node.Position).node);
            }
        }

        public RadialNode StartNode => _path[0];
        public RadialNode EndNode => _path[_path.Length-1];
        public int EndIndex => _path.Length - 1;
        public RadialNode GetNode(int index)
        {
            var lengthMinus1 = _path.Length - 1;
            if (index > lengthMinus1 * 2) return null;

            if (index > lengthMinus1)
            {
                index = (index - lengthMinus1 - lengthMinus1) * -1;
            }
            return _path[index];
        }

        public bool HasArrivedAtBank(int index) => index == _path.Length-1;

        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < _path.Length; i++)
            {
                var node = _path[i];
                Gizmos.DrawWireSphere(node.Position, node.Radius);

                var previousNode = i > 0 ? _path[i - 1] : null;
                if (previousNode != null)
                {
                    Gizmos.DrawLine(node.Position, previousNode.Position);

                }
            }
        }
    }
}
