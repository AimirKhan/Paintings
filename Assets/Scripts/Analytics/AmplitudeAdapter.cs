using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class AmplitudeAdapter
{
    private AmplitudeAdapter() { }
    public static AmplitudeAdapter Instance;
    private Amplitude m_Amplitude;
    private static readonly object instanceLock = new object();


    public bool isLogging = true;

    public static AmplitudeAdapter GetInstance()
    {
        if (Instance == null)
        {
            lock (instanceLock)
            {
                if (Instance == null)
                {
                    Instance = new AmplitudeAdapter();
                }
            }
        }
        return Instance;
    }

    public void Initialization()
    {
        m_Amplitude = Amplitude.getInstance();
        m_Amplitude.logging = isLogging;
        m_Amplitude.trackSessionEvents(true);
        m_Amplitude.init("522c2b004d0b01086c9bfc7d064a60d1"); //API key
        m_Amplitude.enableCoppaControl();
    }

    public void LogEvent(string nameEvent, IDictionary<string, object> properties)
    {
        //Debug.Log("Status in adapter");
        m_Amplitude?.logEvent(nameEvent, properties);
    }

    public void LogEvent(string nameEvent)
    {
        m_Amplitude?.logEvent(nameEvent);
    }

    public void SetOnceUserProperty(string property, IDictionary<string, object> value)
    {
        m_Amplitude?.setOnceUserProperty(property, value);
    }

    public void SetOnceUserProperty(string property, int value)
    {
        m_Amplitude?.setOnceUserProperty(property, value);
    }

    public void SetUserProperty(string property, IDictionary<string, object> value)
    {
        m_Amplitude?.setUserProperty(property, value);
    }

    public void SetUserProperty(string property, int value)
    {
        m_Amplitude?.setUserProperty(property, value);
    }

    public void AddUserProperty(string property, IDictionary<string, object> value)
    {
        m_Amplitude?.addUserProperty(property, value);
    }

    public void AddUserProperty(string property, int value)
    {
        m_Amplitude?.addUserProperty(property, value);
    }

    public Amplitude Amplitude => m_Amplitude;
}