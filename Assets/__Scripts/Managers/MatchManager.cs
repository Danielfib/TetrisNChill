using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class MatchManager : Singleton<MatchManager>
    {
        public void PlayAgain()
        {
            MatchHUDManager.Instance.VanishEverything(() => GameManager.Instance.StartMatch()); ;
        }

        public void ReturnToMain()
        {
            MatchHUDManager.Instance.VanishEverything(() => GameManager.Instance.ReturnToMainMenu());
        }
    }
}
