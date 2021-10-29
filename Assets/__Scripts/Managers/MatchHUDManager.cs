using UnityEngine;
using Tetris.Utils;
using DG.Tweening;

namespace Tetris.Managers
{
    public class MatchHUDManager : Singleton<MatchHUDManager>
    {
        [SerializeField] Transform HUD;
        [SerializeField] SimpleHelvetica scoreNumber;

        public void Start()
        {
            HUD.localScale = Vector3.zero;
        }

        public void Appear()
        {
            HUD.DOScale(1, 0.8f);
        }

        public void Score()
        {
            int currentScore = int.Parse(scoreNumber.Text);
            currentScore++;
            scoreNumber.Text = currentScore.ToString();
            scoreNumber.GenerateText();
        }
    }
}
