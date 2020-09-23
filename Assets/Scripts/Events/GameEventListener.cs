using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
abstract public class GameEventListener<T0> : MonoBehaviour
{
    public GameEvent<T0> Event;
    public UnityEvent<T0> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(T0 param)
    { Response.Invoke(param); }
}

[Serializable]
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }
}