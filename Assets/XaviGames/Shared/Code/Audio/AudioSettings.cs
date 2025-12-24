using System;
using UnityEngine;
using XaviGames.SaveSystem;

namespace XaviGames.Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "XaviGames/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [field: SerializeField]
        [field: Range(0, 1f)]
        public float MasterVolume = 1f;

        [SerializeField]
        private FloatModel _masterVolumeModel;

        private void OnEnable()
        {
            _masterVolumeModel.OnValueChanged += OnMasterVolumeChanged;
        }

        private void OnDisable()
        {
            _masterVolumeModel.OnValueChanged -= OnMasterVolumeChanged;
        }

        public void SetVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            _masterVolumeModel.SetValue(MasterVolume);
        }
        
        public void SetMute(bool isMuted)
        {
            MasterVolume = isMuted ? 0f : 1f;
            _masterVolumeModel.SetValue(MasterVolume);
        }

        public bool IsMuted()
        {
            return MasterVolume <= 0f;
        }

        public void ToggleMuteState()
        {
            SetMute(!IsMuted());
        }

        private void OnMasterVolumeChanged(float value)
        {
            MasterVolume = value;
        }
    }
}