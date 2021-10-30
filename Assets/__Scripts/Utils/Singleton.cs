using UnityEngine;

namespace Tetris.Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get { return instance; }
            private set { instance = value; }
        }

        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        public virtual void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("Already exists one instance of " + typeof(T));
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
