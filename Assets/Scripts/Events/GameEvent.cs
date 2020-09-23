using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameEvent<T0> : ScriptableObject
{
	[SerializeField] private T0 param;
	private List<GameEventListener<T0>> listeners =
		new List<GameEventListener<T0>>();

	public void Raise(T0 param)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(param);
	}

	public void RegisterListener(GameEventListener<T0> listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(GameEventListener<T0> listener)
	{ listeners.Remove(listener); }
}


[CreateAssetMenu(
	fileName = "XXXGameEvent",
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
