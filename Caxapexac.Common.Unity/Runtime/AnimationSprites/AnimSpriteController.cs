using UnityEngine;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.AnimationSprites
{
    public class AnimSpriteController : MonoBehaviour
    {
        public Image animatedImage;
        public float frameRate = 0.01f;
        public Sprite[] frames;

        private bool _played;
        private int _currentImage;

        private void Start()
        {
            _currentImage = 0;
        }

        public void PlayOnceAnimation()
        {
            InvokeRepeating(nameof(ChangeImage), 0.1f, frameRate);
        }

        public void RepeatableAnimation()
        {
            if (_currentImage != frames.Length)
            {
                InvokeRepeating(nameof(RepeatChangeImage), 0.1f, frameRate);
            }
        }

        public void RewindAnimation()
        {
            if (_currentImage == frames.Length - 1)
            {
                InvokeRepeating(nameof(RewindChangeImage), 0.1f, frameRate);
            }
        }

        public void PingPongAnimation()
        {
            if (_played)
            {
                InvokeRepeating(nameof(RewindChangeImage), 0.1f, frameRate);
                _played = false;
            }

            else
            {
                InvokeRepeating(nameof(ChangeImage), 0.1f, frameRate);
                _played = true;
            }
        }

        private void ChangeImage()
        {
            if (_currentImage == frames.Length - 1)
            {
                CancelInvoke(nameof(ChangeImage));
            }

            else
            {
                _currentImage += 1;
                animatedImage.sprite = frames[_currentImage];
            }
        }

        private void RepeatChangeImage()
        {
            if (_currentImage == frames.Length - 1)
            {
                CancelInvoke(nameof(RepeatChangeImage));
                _currentImage = 0;
            }

            else
            {
                _currentImage += 1;
                animatedImage.sprite = frames[_currentImage];
            }
        }

        private void RewindChangeImage()
        {
            if (_currentImage == frames.Length - frames.Length)
            {
                CancelInvoke(nameof(RewindChangeImage));
                _currentImage = 0;
            }

            else
            {
                _currentImage -= 1;
                animatedImage.sprite = frames[_currentImage];
            }
        }
    }
}