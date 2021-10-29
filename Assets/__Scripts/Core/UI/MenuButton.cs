using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Tetris.Managers;

namespace Tetris.UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] float animationStrength, animationDuration;

        Vector3 initScale;

        private void Start()
        {
            initScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(initScale * animationStrength, animationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(initScale, animationDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameManager.Instance.Play();
        }
    }
}