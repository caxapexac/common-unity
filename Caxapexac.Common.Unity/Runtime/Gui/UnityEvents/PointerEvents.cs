using UnityEngine;
using UnityEngine.EventSystems;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    /// <summary>
    /// Example usage: play sound on button hover
    /// </summary>
    public class PointerEvents : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
        IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public UnityEventPointerEventData onPointerDownEvent;
        public UnityEventPointerEventData onPointerClickEvent;
        public UnityEventPointerEventData onPointerUpEvent;
        public UnityEventPointerEventData onPointerExitEvent;
        public UnityEventPointerEventData onPointerEnterEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDownEvent.Invoke(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClickEvent.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUpEvent.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExitEvent.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnterEvent.Invoke(eventData);
        }
    }
}