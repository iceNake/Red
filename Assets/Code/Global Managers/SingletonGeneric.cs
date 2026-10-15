using UnityEngine;

namespace RED.Utility.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        [SerializeField] private bool dontDestroyOnLoad = true;
        
        public bool IsInitialized { get; private set; }
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"Singleton<{typeof(T).Name}> was accessed but does not exist in the scene.");
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
                IsInitialized = true;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                IsInitialized = false;
            }
        }
    }
}

