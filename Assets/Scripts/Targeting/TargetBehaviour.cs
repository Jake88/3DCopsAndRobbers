using System.Collections.Generic;
using UnityEngine;

namespace My.TargetSystem
{
    [RequireComponent(typeof(Targeter))]
    public abstract class TargetBehaviour : MonoBehaviour
    {
        [Tooltip("Won't switch targets until current has left the overlap")]
        [SerializeField] public bool _persistantTargeting;

        protected List<Robber> _newTargets = new List<Robber>();
        protected int _targetCount;
        public bool PersistantTargeting { set { _persistantTargeting = value; } }

        public bool DetermineNewTargets(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1)
        {
            _newTargets.Clear();
            _targetCount = 0;

            // Handle no targets.
            if (potentialTargets.Count == 0)
                return HandleNoTargets(currentTargets);

            bool newTargetsFound;

            // Handle less options than max targets.
            if (potentialTargets.Count <= maxTargets)
                newTargetsFound = HandleNoChoiceTarget(currentTargets, potentialTargets);
            else if (_persistantTargeting && currentTargets.Count > 0)
            {
                foreach (var currentTarget in currentTargets)
                {
                    if (potentialTargets.Contains(currentTarget))
                    {
                        _newTargets.Add(currentTarget);
                        potentialTargets.Remove(currentTarget);
                    }
                }
                if (_newTargets.Count >= maxTargets) return false;
                newTargetsFound = TargetUniquely(currentTargets, potentialTargets, maxTargets);
            }
            // Delegate to actual targeting system.
            else
                newTargetsFound = TargetUniquely(currentTargets, potentialTargets, maxTargets);

            if (!newTargetsFound) return false;

            ApplyNewTargets(currentTargets);

            return true;
        }

        private bool HandleNoChoiceTarget(List<Robber> currentTargets, List<Robber> potentialTargets)
        {
            bool newTargetFound = false;
            foreach (var target in potentialTargets)
            {
                _newTargets.Add(target);
                if (!currentTargets.Contains(target))
                    newTargetFound = true;
            }
            return newTargetFound;
        }

        void ApplyNewTargets(List<Robber> currentTargets)
        {
            currentTargets.Clear();
            foreach (var newTarget in _newTargets)
                currentTargets.Add(newTarget);
        }

        bool HandleNoTargets(List<Robber> currentTargets)
        {
            if (currentTargets.Count == 0)
                return false;
            else
            {
                currentTargets.Clear();
                return true;
            }
        }

        protected abstract bool TargetUniquely(List<Robber> currentTargets, List<Robber> potentialTargets, int maxTargets = 1);
    }
}