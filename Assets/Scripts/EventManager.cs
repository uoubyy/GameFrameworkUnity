using System;
using System.Collections.Generic;
using UnityEngine;

public class EventType
{
    static string AchievementUnlock = "AchievementUnlock";
}

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<Dictionary<string, object>>> m_eventDictionary;

    private static EventManager _instance;

    public static EventManager Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(EventManager)) as EventManager;

            if (!_instance)
            {
                Debug.Log("Failed to find the active EventManager Instance!");
            }
            else
            {
                _instance.Init();
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    private void Init()
    {
        m_eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
    }

    public static void RegisterListener(string eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;
        if(Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.m_eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.m_eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void UnregisterListener(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (Instance == null) return;

        Action<Dictionary<string, object>> thisEvent;
        if(Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.m_eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message)
    {
        Action<Dictionary<string, object>> thisEvent = null;
        if (Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.Invoke(message);
    }
}
