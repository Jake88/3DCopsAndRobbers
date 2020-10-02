using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the abstract class that allows us to have generics
abstract public class GameEvent<T> : ScriptableObject
{
    private List<GameEventListener<T>> listeners =
        new List<GameEventListener<T>>();

    public void Raise(T param)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(param);
    }

    public void RegisterListener(GameEventListener<T> listener)
    { listeners.Add(listener); }

    public void UnregisterListener(GameEventListener<T> listener)
    { listeners.Remove(listener); }
}

[CreateAssetMenu(
	fileName = "XXX_GameEvent",
	menuName = AssetMenuConstants.EVENT + "Game event"
)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners =
        new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    { listeners.Add(listener); }

    public void UnregisterListener(GameEventListener listener)
    { listeners.Remove(listener); }
}
