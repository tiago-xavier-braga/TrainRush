using UnityEngine;

namespace XaviGames.Audio
{
    [CreateAssetMenu(fileName = "SoundEffect", menuName = "XaviGames/SoundEffect")]
    public class SoundEffect : ScriptableObject
    {
        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        private float _volume = 1.0f;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        private bool _loop = false;

        private bool _isPlaying = false;

        private AudioSource _audioSource;

        public void Play(AudioSource audioSource = null)
        {
            if (_isPlaying)
            {
                return;
            }

            if (audioSource == null)
            {
                GameObject tempGO = new GameObject($"TempAudio {_clip.name}");
                _audioSource = tempGO.AddComponent<AudioSource>();
            }
            else
            {
                _audioSource = audioSource;
            }

            _audioSource.clip = _clip;
            _audioSource.volume = _volume * _audioSettings.MasterVolume;
            _audioSource.loop = _loop;

            _audioSource.Play();
            _isPlaying = true;

            float time = 0f;
            while (time <= _clip.length)
            {
                time += Time.deltaTime;
            }

            if (audioSource == null)
            {
                Destroy(_audioSource.gameObject);
            }

            _isPlaying = false;
        }
    }
}
