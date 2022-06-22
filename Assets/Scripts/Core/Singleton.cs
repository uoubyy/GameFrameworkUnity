using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        private static bool _quitting;

        public static T Instance
        {
            get
            {
                if (_quitting)
                    return null;

                if (_instance == null && !_quitting)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newGO = new GameObject();
                        _instance = newGO.AddComponent<T>();
                        newGO.name = "(singleton)" + typeof(T).ToString();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {

            if (_instance == null)
            {
                _instance = gameObject.GetComponent<T>();
                gameObject.name = "(singleton)" + typeof(T).ToString();
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
                throw new System.Exception(string.Format("Instance of {0} already exists, removing {0}.", GetType().FullName, ToString()));
            }

            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnApplicationQuit()
        {
            _quitting = true;
        }

        protected virtual void OnDestroy()
        {

        }

        public static bool InstanceExists { get { return !_quitting && _instance != null; } }
    }
}