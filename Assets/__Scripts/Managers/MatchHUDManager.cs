using UnityEngine;
using Tetris.Utils;
using DG.Tweening;
using System;
using System.Collections;

namespace Tetris.Managers
{
    public class MatchHUDManager : Singleton<MatchHUDManager>
    {
        [SerializeField] Transform HUD, endingScreen, map;
        [SerializeField] SimpleHelvetica scoreNumber;
        [SerializeField] float vanishDuration;

        public void ShowEndScreen()
        {
            map.DOMoveX(map.position.x + 10, 2);
            endingScreen.DOMoveX(0, 2);
        }

        public void Start()
        {
            HUD.localScale = Vector3.zero;
        }

        public void Appear()
        {
            HUD.DOScale(1, 0.8f);
        }

        public void VanishEverything(Action doOnCOmplete)
        {
            HUD.DOScale(0, vanishDuration);
            map.DOMoveX(map.position.x + 10, vanishDuration);
            endingScreen.DOMoveX(-20, vanishDuration);

            StartCoroutine(DoAfterCoroutine(doOnCOmplete, vanishDuration));
        }

        private IEnumerator DoAfterCoroutine(Action doWhat, float after)
        {
            yield return new WaitForSeconds(after);
            doWhat.Invoke();
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
