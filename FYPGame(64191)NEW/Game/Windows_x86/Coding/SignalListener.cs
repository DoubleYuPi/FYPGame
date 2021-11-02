using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public Alerts alerts;
    public UnityEvent eventAlert;
    public void OnSignalRaised()
    {
        eventAlert.Invoke();
    }

    private void OnEnable()
    {
        alerts.RegisterListeners(this);
    }

    private void OnDisable()
    {
        alerts.DeRegisterListeners(this);
    }
}
