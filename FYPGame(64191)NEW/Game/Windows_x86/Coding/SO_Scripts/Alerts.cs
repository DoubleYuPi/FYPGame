using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Alerts : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        for(int i = listeners.Count - 1; i >=0; i--)
        {
            listeners[i].OnSignalRaised();
        }
    }

    public void RegisterListeners(SignalListener listener)
    {
        listeners.Add(listener);
    }
    public void DeRegisterListeners(SignalListener listener)
    {
        listeners.Remove(listener);
    }
}
