using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    public class ButtonEvents : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent onInactiveClick;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) onInactiveClick.Invoke();
        }
    }
}