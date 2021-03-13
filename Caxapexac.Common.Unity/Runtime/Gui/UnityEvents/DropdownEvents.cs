using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    public class DropdownEvents : MonoBehaviour
    {
        public UnityEventInt onValueChangedEvent;

        private Dropdown _dropdown;

        private void Awake()
        {
            _dropdown = GetComponent<Dropdown>();
        }

        private void OnEnable()
        {
            _dropdown.onValueChanged.AddListener(selectedIndex => onValueChangedEvent.Invoke(selectedIndex));
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}