using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Behaviours
{
    /// <summary>
    /// Clicks button on keyboard press KeyCode (only when button is interactable)
    /// </summary>
    public class ButtonKeyCodeClicker : MonoBehaviour
    {
        public KeyCode ClickCode;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Update()
        {
            if (!_button.interactable) return;
            if (Input.GetKeyDown(ClickCode)) _button.onClick.Invoke();
        }
    }
}