using UnityEngine;
using Tetris.Utils;
using UnityEngine.SceneManagement;

namespace Tetris.Managers
{
    public class MainScreenManager : Singleton<MainScreenManager>
    {
        public void StartMatch() 
        {
            SceneManager.LoadScene(1);
        }
    }
}
