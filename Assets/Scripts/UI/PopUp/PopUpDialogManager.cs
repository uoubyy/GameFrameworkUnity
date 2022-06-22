using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

using EventSystem;
using EventSystem.Data;
using Utility;

namespace UI.PopUp
{
    public class PopUpDialogManager : MonoBehaviour
    {
        [SerializeField]
        private PopUpDialog m_prefab;

        [SerializeField]
        private int m_initPoolSize;

        [SerializeField]
        private int m_maxPoolSize;

        private ObjectPool<PopUpDialog> m_popUpPool;

        void Awake()
        {
            m_popUpPool = new ObjectPool<PopUpDialog>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, m_initPoolSize, m_maxPoolSize);
        }

        PopUpDialog CreatePooledObject()
        {
            PopUpDialog instance = Instantiate(m_prefab, Vector3.zero, Quaternion.identity);
            instance.gameObject.SetActive(false);
            instance.OnDisableCallback += ReturnToPool;
            return instance;
        }

        void OnTakeFromPool(PopUpDialog instance)
        {

        }

        void OnReturnToPool(PopUpDialog instance)
        {

        }

        void OnDestroyObject(PopUpDialog instance)
        {

        }

        void ReturnToPool(PopUpDialog instance)
        {
            m_popUpPool.Release(instance);
        }

        void Start()
        {
            EventManager.StartListening(CustomEventName.EVENT_UI_SHOW_POPUP, OnShowPopUpDialog);
        }

        void OnDestroy()
        {
            EventManager.StopListening(CustomEventName.EVENT_UI_SHOW_POPUP, OnShowPopUpDialog);
        }

        public void OnShowPopUpDialog(IGameEventData eventData)
        {
            PopUpMessageData messageData;
            if(Utils.TryConvertVal<IGameEventData, PopUpMessageData>(eventData, out messageData))
            { PopUpDialog instance = m_popUpPool.Get();
                instance.Init(messageData.closeAction, messageData.confirmAction, messageData.title, messageData.message, messageData.layer);
                instance.Show();
            }
        }
    }
}
