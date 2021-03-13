using System.Collections;
using UnityEngine;
// ReSharper disable StringLiteralTypo


namespace Caxapexac.Common.Sharp.Runtime.MonoHelpers
{
    public class FadeAlpha : MonoBehaviour
    {
        [SerializeField]
        public float duration = 1.0f;

        private Color _originalColor;
        private Color _fadedColor = new Color(0, 0, 0, 0);
        private static readonly int _mode = Shader.PropertyToID("_Mode");
        private static readonly int _srcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int _dstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int _zWrite = Shader.PropertyToID("_ZWrite");

        private void Start()
        {
            _originalColor = gameObject.GetComponent<Renderer>().material.color;
            Shader.WarmupAllShaders();
        }

        public void SwitchToFade()
        {
            // get the material from the object
            var mat = GetComponent<Renderer>().material;

            // set the Rendering Mode to Fade (0 is Opaque, 1 is Cutout, 2 is Fade, and 3 is Transparent)
            mat.SetFloat(_mode, 2);
            mat.SetInt(_srcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt(_dstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt(_zWrite, 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            // start the coroutine that blends from one color to another
            StartCoroutine("FadeOut");
        }

        private IEnumerator FadeOut()
        {
            // get the material from the object
            var mat = GetComponent<Renderer>().material;

            // initialize a time counter
            float elapsedTime = 0.0f;

            // store the starting material color
            Color startingColor = mat.color;

            // lerp the fade transition over the amount of time specified in the inspector
            while (elapsedTime < duration)
            {
                // update the color once per frame based on how much time has elapsedTime
                mat.color = Color.Lerp(startingColor, _fadedColor, (elapsedTime / duration));

                // keep track of how much time has passed since the coroutine started
                elapsedTime = elapsedTime + Time.deltaTime;
                yield return null;
            }
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        public void SwitchToOpaque()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            // start the coroutine that blends from one color to another
            StartCoroutine(nameof(FadeIn));
        }

        private IEnumerator FadeIn()
        {
            // get the material from the object
            var mat = GetComponent<Renderer>().material;

            // initialize a time counter
            float elapsedTime = 0.0f;

            // store the starting material color
            Color startingColor = mat.color;

            // lerp the fade transition over the amount of time specified in the inspector
            while (elapsedTime < duration)
            {
                // update the color once per frame based on how much time has elapsedTime
                mat.color = Color.Lerp(startingColor, _originalColor, (elapsedTime / duration));

                // keep track of how much time has passed since the coroutine started
                elapsedTime = elapsedTime + Time.deltaTime;
                yield return null;
            }

            // the transition has finished because the while loop is done
            // just in case the color didn't finish changing all the way back to what it should be:
            mat.color = _originalColor;
            mat.SetFloat(_mode, 0);
            mat.SetInt(_srcBlend, (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt(_dstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt(_zWrite, 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;
        }
    }
}