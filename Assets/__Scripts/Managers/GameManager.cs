using UnityEngine;
using Tetris.Utils;
using System;
using UnityEngine.SceneManagement;

namespace Tetris.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameSettings settings;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#else
            Application.Quit();
#endif
        }

        public float GetStandardFallTime() { return settings.StandardFallTime; }
        public float GetAcceleratedFallTime() { return settings.AcceleratedFallTime; }
    }

    [Serializable]
    class GameSettings
    {
        public float StandardFallTime = 1;
        public float AcceleratedFallTime = 0.1f;
    }
}
