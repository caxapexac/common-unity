using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Widgets
{
    public class FpsDisplayUgui : MonoBehaviour
    {
        public int FrameRate;
        public Text Text;

        public void Update()
        {
            float current = (int)(1f / Time.unscaledDeltaTime);
            FrameRate = (int)current;
            Text.text = FrameRate + " FPS";
        }
    }
}