using My.ModifiableStats;
using UnityEngine;

namespace My.TargetSystem
{
    public class ArcTargeter : Targeter
    {
#if UNITY_EDITOR
        float _viewRadiusForGizmos;
        float _viewAngleForGizmos;
#endif

        [SerializeField] ModifiableStat _viewRadius = new ModifiableStat(1);
        [SerializeField] ModifiableStat _viewAngle = new ModifiableStat(45, 360);
        [SerializeField] LayerMask _obstacleMask;

        void OnValidate()
        {
            _viewRadiusForGizmos = _viewRadius.BaseValue;
            _viewAngleForGizmos = _viewAngle.BaseValue;
        }

        protected override void FindTargets()
        {
            _potentialTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius.Value, _targetMask);
            foreach (Collider target in targetsInViewRadius)
            {
                var targetTransform = target.GetComponent<Robber>();

                Vector3 dirToTarget = (targetTransform.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle.Value / 2)
                {
                    if (_obstacleMask.Equals(LayerMask.GetMask(NO_MASK)))
                        _potentialTargets.Add(targetTransform);
                    else
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.transform.position);

                        if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, _obstacleMask))
                            _potentialTargets.Add(targetTransform);
                    }
                }
            }

            if (_previouslyCountedPotentialTargets == _potentialTargets.Count) return;
            _previouslyCountedPotentialTargets = _potentialTargets.Count;

            // Perform targeting behaviour
            if (_targetBehaviour.DetermineNewTargets(_currentTargets, _potentialTargets, _maxTargets.IntValue))
            {
                TargetChanged();
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = new Color(0, 0, 255, .3f);
            Gizmos.DrawWireSphere(transform.position, _viewRadiusForGizmos);

            Vector3 viewAngleA = DirFromAngle(-_viewAngleForGizmos / 2, false);
            Vector3 viewAngleB = DirFromAngle(_viewAngleForGizmos / 2, false);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * _viewRadiusForGizmos / 4);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * _viewRadiusForGizmos);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * _viewRadiusForGizmos);
        }
    }
}