using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerVector3 : GameEventListener<Vector3>
{
    public new GameEventVector3 Event;
    [SerializeField]
    public new UnityEvent<Vector3> Response; // TODO: Looks like I need to make a new class that inherits UnityEvent and takes the Vector3 automatically
    // OR just download the beta and try to use that

    public new void OnEventRaised(Vector3 param)
    { Response.Invoke(param); }
}