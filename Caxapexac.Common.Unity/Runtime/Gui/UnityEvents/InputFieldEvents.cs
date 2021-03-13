using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    public class InputFieldEvents : MonoBehaviour
    {
        public UnityEventString onEndEditEvent;
        public UnityEventString onValueChangedEvent;

        private InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<InputField>();
        }

        private void OnEnable()
        {
            _inputField.onEndEdit.AddListener(val => onEndEditEvent.Invoke(val));
            _inputField.onValueChanged.AddListener(val => onValueChangedEvent.Invoke(val));
        }

        private void OnDisable()
        {
            _inputField.onEndEdit.RemoveAllListeners();
            _inputField.onValueChanged.RemoveAllListeners();
        }
    }
}