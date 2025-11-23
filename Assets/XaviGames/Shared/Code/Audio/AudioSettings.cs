using UnityEngine;

namespace XaviGames.Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "XaviGames/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [field: SerializeField]
        [Range(0, 1f)]
        public float MasterVolume { get; private set; } = 1f;

        public void SetMute(bool isMuted)
        {
            MasterVolume = isMuted ? 0f : 1f;
        }

        public bool IsMuted()
        {
            return MasterVolume <= 0f;
        }

        public void ToggleMuteState()
        {
            SetMute(!IsMuted());
        }

        public void SetVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
        }
    }
}