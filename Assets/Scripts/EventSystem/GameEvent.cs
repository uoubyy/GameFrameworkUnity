using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem.Data;

namespace EventSystem
{
    [CreateAssetMenu(fileName = "newGameEvent", menuName = "EventSystem/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> m_eventListeners = new List<GameEventListener>();

        public void Raise(IGameEventData data = null)
        {
            foreach (GameEventListener listener in m_eventListeners)
                listener.OnEventRaised(data);
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!m_eventListeners.Contains(listener))
                m_eventListeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            if (m_eventListeners.Contains(listener))
                m_eventListeners.Remove(listener);
        }
    }
}
