using UnityEngine;
using XaviGames.Animation;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Wagon
{
    public class WagonUpgradeController : MonoBehaviour
    {
        [SerializeField]
        private Transform _modelTransform;

        [Header("Spawn Animation")]
        [SerializeField]
        private SpawnAnimation _spawnAnimation;

        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _wagonUnlockClip;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;

        private GameObject _currentWagonModel;
        private bool _isFirstSpawn = true;

        public void CreateWagonModel(WagonData wagonData)
        {
            if (_currentWagonModel != null)
            {
                Destroy(_currentWagonModel);
            }

            _currentWagonModel = Instantiate(wagonData.WagonPrefab, _modelTransform);
            _currentWagonModel.transform.localScale = Vector3.zero;

            _spawnAnimation.Animate(_currentWagonModel, Vector3.zero, Vector3.one);

            if (!_isFirstSpawn)
            {
                _audioSource.clip = _wagonUnlockClip;
                _audioSource.volume = _volume * _audioSettings.MasterVolume;
                _audioSource.Play();
                return;
            }

            _isFirstSpawn = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
