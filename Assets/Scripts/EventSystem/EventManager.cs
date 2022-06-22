using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Core;
using EventSystem.Data;

namespace EventSystem
{
    public class CustomEventName
    {
        public const string EVENT_GAME_START = "event_game_start";

        public const string EVENT_GAME_STATE_SELECTED = "event_game_state_selected";

        public const string EVENT_GAME_PLAYER_MOVEMENT_COMPLETED = "event_game_player_movement_completed";

        public const string EVENT_UI_SHOW_POPUP = "event_ui_show_popup";
    }

    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, UnityEvent<IGameEventData>> m_eventDictionary;

        private EventManager() { }

        protected override void OnAwake()
        {
            base.OnAwake();

            if (m_eventDictionary == null)
                m_eventDictionary = new Dictionary<string, UnityEvent<IGameEventData>>();
        }

        public static void StartListening(string eventName, UnityAction<IGameEventData> listener)
        {
            UnityEvent<IGameEventData> thisEvent = null;
            if (Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent<IGameEventData>();
                thisEvent.AddListener(listener);
                Instance.m_eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction<IGameEventData> listener)
        {
            // Unity call OnDisable and OnDestroy one object then another when quit application
            // so sometime Singleton has been destroyed before other objects' call OnDisable
            if (!Instance) return;

            UnityEvent<IGameEventData> thisEvent = null;
            if (Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName, IGameEventData eventData)
        {
            UnityEvent<IGameEventData> thisEvent = null;
            if (Instance.m_eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventData);
            }
        }
    }
}
