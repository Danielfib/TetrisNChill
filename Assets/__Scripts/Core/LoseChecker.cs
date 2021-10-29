using UnityEngine;
using Tetris.Managers;

public class LoseChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("perdeu");
        FindObjectOfType<PlayerController>().StopPlaying();
        MatchHUDManager.Instance.ShowEndScreen();
    }
}
