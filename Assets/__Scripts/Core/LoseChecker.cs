using UnityEngine;
using Tetris.Managers;

namespace Tetris.Core
{
    public class LoseChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<PlayerController>().StopPlaying();
            MatchHUDManager.Instance.ShowEndScreen();
        }
    }
}
