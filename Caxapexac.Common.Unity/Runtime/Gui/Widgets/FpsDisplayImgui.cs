using System.Collections;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Widgets
{
    [AddComponentMenu("Gui/Widgets/FpsDisplayImgui")]
    public class FpsDisplayImgui : MonoBehaviour
    {
        public Rect startRect = new Rect(10, 10, 55, 40); // The rect the window is initially displayed at.
        public bool updateColor = true; // Do you want the color to change if the FPS gets low
        public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
        public float frequency = 0.5F; // The update frequency of the fps

        private float _accum; // FPS accumulated over the interval
        private int _frames; // Frames drawn over the interval
        private Color _color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
        private string _sFps = ""; // The fps formatted into a string.
        private GUIStyle _style; // The style the text will be displayed at, based en defaultSkin.label.

        private void Start()
        {
            StartCoroutine(Fps());
        }

        private void Update()
        {
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;
        }

        private IEnumerator Fps()
        {
            // Infinite loop executed every "frenquency" secondes.
            while (true)
            {
                int fps = Mathf.CeilToInt(_accum / _frames);
                _sFps = fps.ToString();
                _color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);
                _accum = 0.0F;
                _frames = 0;
                yield return new WaitForSeconds(frequency);
            }

            // ReSharper disable once IteratorNeverReturns
        }

        private void OnGUI()
        {
            // Copy the default label skin, change the color and the alignement
            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label) {normal = {textColor = Color.white}, fontSize = 18, alignment = TextAnchor.MiddleCenter};
            }
            GUI.color = updateColor ? _color : Color.white;
            startRect = GUI.Window(0, startRect, DoMyWindow, "");
        }

        private void DoMyWindow(int windowId)
        {
            GUI.Label(new Rect(0, 0, startRect.width, startRect.height), _sFps, _style);
            if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        }
    }
}