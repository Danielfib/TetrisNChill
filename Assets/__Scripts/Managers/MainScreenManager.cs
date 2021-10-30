using UnityEngine;
using Tetris.Utils;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Tetris.Managers
{
    public class MainScreenManager : Singleton<MainScreenManager>
    {
        [SerializeField]
        Transform[] vanishToLeft, vanishToRight;
        [SerializeField] float vanishDuration;

        public void StartMatch() 
        {
            foreach(var l in vanishToLeft)
            {
                l.DOMoveX(l.position.x - 18, vanishDuration);
            }

            foreach (var r in vanishToRight)
            {
                r.DOMoveX(r.position.x + 35, vanishDuration);
            }

            Invoke("GoToMatchScene", vanishDuration * 0.8f);
        }

        private void GoToMatchScene()
        {
            DOTween.CompleteAll();
            GameManager.Instance.StartMatch();
        }

        public void Quit()
        {
            GameManager.Instance.Quit();
        }
    }
}
