using UnityEngine;
using XaviGames.SaveSystem;

namespace XaviGames.Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "XaviGames/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [field: SerializeField]
        [Range(0, 1f)]
        public float MasterVolume { get; private set; } = 1f;

        [SerializeField]
        private Model _masterVolumeModel;

        public void SetVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            _masterVolumeModel.Value = MasterVolume;
        }
        
        public void SetMute(bool isMuted)
        {
            MasterVolume = isMuted ? 0f : 1f;
            _masterVolumeModel.Value = MasterVolume;
        }

        public bool IsMuted()
        {
            return MasterVolume <= 0f;
        }

        public void ToggleMuteState()
        {
            SetMute(!IsMuted());
        }

    }
}