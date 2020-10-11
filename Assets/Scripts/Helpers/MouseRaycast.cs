using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;


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

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Handle all point subscriptions
        foreach (var maskIndex in PointEvents.Dict.Keys)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskIndex))
            {
                foreach (var an in PointEvents.Dict[maskIndex])
                {
                    an(hit);
                }
            }
        }

        // Handle all click subscriptions
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (var maskIndex in ClickEvents.Dict.Keys)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskIndex))
                {
                    foreach (var an in ClickEvents.Dict[maskIndex])
                    {
                        an(hit);
                    }
                }
            }
        }
    }
}