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

        public void FinishedMatch(int score)
        {
            int currentHighscore = PlayerPrefs.GetInt("Highscore");
            if(score > currentHighscore)
            {
                PlayerPrefs.SetInt("Highscore", score);
            }
        }

        public float GetStandardFallTime() { return settings.StandardFallTime; }
        public float GetAcceleratedFallTime() { return settings.AcceleratedFallTime; }

        public void StartMatch()
        {
            AudioManager.Instance.PlayTransitionWhoosh();
            SceneManager.LoadScene(1);
        }

        public void ReturnToMainMenu()
        {
            //AudioManager.Instance.PlayTransitionWhoosh();
            SceneManager.LoadScene(0);
        }
    }

    [Serializable]
    class GameSettings
    {
        public float StandardFallTime = 0.8f;
        public float AcceleratedFallTime = 0.1f;
    }
}
