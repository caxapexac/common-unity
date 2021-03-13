using Caxapexac.Common.Sharp.Runtime.Services.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Widgets
{
    public class AudioMixerSlider : MonoBehaviour
    {
        public AudioMixer mixer;
        public string parameter;
        public bool linearVolume = true;

        private AudioSource _valueChangeSound;
        private Slider _slider;

        private void Start()
        {
            _valueChangeSound = GetComponent<AudioSource>();
            if (mixer.GetFloat(parameter, out float value))
            {
                _slider = GetComponent<Slider>();
                _slider.value = linearVolume ? SoundHelper.GetNormalVolume(value) : value;
                _slider.onValueChanged.AddListener(ValueChanged);
            }
            else
            {
                Debug.LogError("Cant find Audio Mixer parameter " + parameter);
            }
        }

        public void ValueChanged(float value)
        {
            mixer.SetFloat(parameter, linearVolume ? SoundHelper.GetSoundVolume(value) : value);
            if (_valueChangeSound && !_valueChangeSound.isPlaying) _valueChangeSound.Play();
        }

        public void OnDestroy()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }
    }
}