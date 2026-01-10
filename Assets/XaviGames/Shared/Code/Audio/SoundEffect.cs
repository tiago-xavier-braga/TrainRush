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
        [Range(-3f, 3f)]
        private float _minPitch;

        [SerializeField]
        [Range(-3f, 3f)]
        private float _maxPitch;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying = false;

        [SerializeField]
        [ReadOnly]
        private bool _isPaused = false;

        private Coroutine _audioCoroutine = null;

        [Button(true)]
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

            _audioCoroutine = StartCoroutine(AwaitAudioFinish());
        }

        [Button(true)]
        public void PlayOneShort()
        {
            _audioSource.PlayOneShot(_clip, _volume * _audioSettings.MasterVolume);
        }

        public void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _volume = volume;
            _audioSource.volume = _volume * _audioSettings.MasterVolume;
        }

        public void SetPitch(float pitch)
        {
            pitch = Mathf.Clamp01(pitch);
            _audioSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, pitch);
        }

        [Button(true)]
        public void Resume()
        {
            if (!_isPaused)
            {
                return;
            }

            _audioSource.UnPause();
            _isPaused = false;
            _isPlaying = true;
        }

        [Button(true)]
        public void Pause()
        {
            if (_isPaused || !_isPlaying)
            {
                return;
            }

            _audioSource.Pause();
            _isPaused = true;
        }

        [Button(true)]
        public void Stop()
        {
            _audioSource.Stop();
            _isPlaying = false;

            if (_audioCoroutine != null)
            {
                StopCoroutine(_audioCoroutine);
                _audioCoroutine = null;
            }
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        private IEnumerator AwaitAudioFinish()
        {
            yield return null;

            while (_audioSource.isPlaying)
            {
                yield return null;
            }

            _isPlaying = false;
        }
    }
}