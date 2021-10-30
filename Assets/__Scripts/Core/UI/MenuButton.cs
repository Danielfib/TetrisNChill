using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Tetris.Managers;
using UnityEngine.Events;

namespace Tetris.Core.UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] float animationStrength, animationDuration;
        [SerializeField] UnityEvent clickEvent;

        Vector3 initScale;

        private void Start()
        {
            initScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(initScale * animationStrength, animationDuration);
            AudioManager.Instance.PlayBtnHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(initScale, animationDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            clickEvent.Invoke();
        }
    }
}