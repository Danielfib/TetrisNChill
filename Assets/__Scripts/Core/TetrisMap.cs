using UnityEngine;
using DG.Tweening;
using Tetris.Managers;

namespace Tetris.Core
{
    public class TetrisMap : MonoBehaviour
    {
        public void Start()
        {
            transform.DOMoveX(0, 1).SetEase(Ease.OutBack).OnComplete(() => {
                MatchHUDManager.Instance.Appear();
                FindObjectOfType<PlayerController>().StartPlaying(); 
            });
        }
    }
}
