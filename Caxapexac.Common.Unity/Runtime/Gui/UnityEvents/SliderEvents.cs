using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    public class SliderEvents : MonoBehaviour
    {
        public UnityEventFloat onValueChangedEvent;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(val => onValueChangedEvent.Invoke(val));
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }
    }
}