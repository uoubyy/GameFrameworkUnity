using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EventSystem.Data;

namespace EventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("The Game Event we are care about.")]
        public GameEvent m_gameEvent;
        public UnityEvent<IGameEventData> m_response = new UnityEvent<IGameEventData>();

        private void OnEnable()
        {
            m_gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            m_gameEvent.UnRegisterListener(this);
        }

        public void OnEventRaised(IGameEventData data = null)
        {
            m_response.Invoke(data);
        }
    }
}
