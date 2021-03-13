using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Behaviours
{
    public class WindowShower : MonoBehaviour
    {
        public bool State = false;
        public KeyCode keyCode;
        public GameObject content;

        private void Update()
        {
            if (Input.GetKeyDown(keyCode)) State = !State;
            if (content.activeSelf != State) content.SetActive(State);
        }

        public void ChangeState()
        {
            State = !State;
        }

        public void Show()
        {
            State = true;
        }

        public void Close()
        {
        }
    }
}