using System;
using System.Collections.Generic;
using UnityEngine;

namespace My.InputHandling
{
    public class MouseRaycast : MonoBehaviour
    {
        // I hate this class. 
        public class EventDictionary
        {
            public Dictionary<int, List<Action<RaycastHit>>> Dict = new Dictionary<int, List<Action<RaycastHit>>>();

            private readonly Queue<Rem> q = new Queue<Rem>();
            private delegate void Rem();

            public void RegisterForRaycastHit(int layerMask, Action<RaycastHit> callback)
            {
                if (!Dict.ContainsKey(layerMask))
                {
                    Dict.Add(layerMask, new List<Action<RaycastHit>>());
                }
                Dict[layerMask].Add(callback);
            }

            public void DeregisterForRaycastHit(int layerMask, Action<RaycastHit> callback)
            {
                if (Dict[layerMask].Contains(callback))
                {
                    q.Enqueue(() => Dict[layerMask].Remove(callback));
                }
            }

            public void ContinueRemovingListeners()
            {
                foreach (var callback in q)
                {
                    callback();
                }
                q.Clear();
            }
        }

        public static EventDictionary PointEvents { get; } = new EventDictionary();
        public static EventDictionary ClickEvents { get; } = new EventDictionary();

        [SerializeField] private Camera _camera;


        private void Awake()
        {
            _camera = _camera ?? Camera.main;
        }

        private void Update()
        {
            // Due to race conditions, we store any deregistrations from the previous frame and continue them here.
            PointEvents.ContinueRemovingListeners();
            ClickEvents.ContinueRemovingListeners();

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Handle all point subscriptions
            foreach (var maskIndex in PointEvents.Dict.Keys)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskIndex))
                {
                    foreach (var action in PointEvents.Dict[maskIndex])
                    {
                        action(hit);
                    }
                }
            }

            // Handle all click subscriptions
            if (Input.GetButtonDown("Fire1"))
            {
                foreach (var maskIndex in ClickEvents.Dict.Keys)
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskIndex))
                    {
                        foreach (var action in ClickEvents.Dict[maskIndex])
                        {
                            action(hit);
                        }
                    }
                }
            }
        }
    }
}
