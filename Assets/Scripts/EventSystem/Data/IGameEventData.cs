using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventSystem.Data
{
    public interface IGameEventData
    {

    }

    // In game event
    public struct CustomEventData : IGameEventData
    {
        public int m_ID;
    }

    public struct StateSelectedData : IGameEventData
    {
        public bool bStartPoint;
        public bool bEndPoint;
        public string stateName;
    }

    public struct PlayerLocationData : IGameEventData
    {
        public string stateName;
    }

    // UI event
    public struct PopUpMessageData : IGameEventData
    {
        public UnityAction closeAction;
        public UnityAction confirmAction;
        public string title; 
        public string message;
        public int layer;
    }
}
