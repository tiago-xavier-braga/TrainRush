using System.Collections;
using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Audio
{
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;

        [SerializeField]
        private bool _isLoop = false;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying = false;

        public void Play()
        {
            if (_isPlaying)
            {
                return;
            }

            _audioSource.volume = _volume * _audioSettings.MasterVolume;
            _audioSource.clip = _clip;
            _audioSource.Play();
            _isPlaying = true;

            StartCoroutine(AwaitAudioFinish());
        }

        public void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _volume = volume;
        }

        public void Stop()
        {
            _audioSource.Stop();
            _isPlaying = false;
        }

        private IEnumerator AwaitAudioFinish()
        {
            yield return new WaitForSeconds(_clip.length + 1);
            _isPlaying = false;
        }
    }
}