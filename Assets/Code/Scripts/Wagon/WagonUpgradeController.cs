using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.Events;
using XaviGames.SaveSystem;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Wagon
{
    public class WagonUpgradeController : MonoBehaviour
    {
        [Header("Wagons")]
        [SerializeField]
        [ReadOnly]
        private WagonData _wagonData = null;

        [SerializeField]
        private List<WagonData> _availableWagons = new();

        [SerializeField]
        private Transform _modelTransform;

        [SerializeField]
        private VoidEventChannel _onWagonUnlockedEvent;

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

        [Header("Save System")]

        [SerializeField]
        private DataController _dataController;

        private GameObject _currentWagonModel;

        private void OnEnable()
        {
            _onWagonUnlockedEvent.Subscribe(OnWagonUnlocked);
        }

        private void OnDisable()
        {
            _onWagonUnlockedEvent.Unsubscribe(OnWagonUnlocked);
        }

        //TODO: Refactor method to use events or call from other class to upgrade wagon
        public void UpgradeWagon()
        {
            if (_wagonData.WagonOrder >= _availableWagons.Count - 1)
            {
                return;
            }

            _wagonData = _availableWagons.Find(wd => wd.WagonOrder == _wagonData.WagonOrder + 1);
            CreateWagonModel();
            PlaySound();
        }

        private void CreateWagonModel()
        {
            if (_currentWagonModel != null)
            {
                Destroy(_currentWagonModel);
            }

            _currentWagonModel = Instantiate(_wagonData.WagonPrefab, _modelTransform);
            _currentWagonModel.transform.localScale = Vector3.zero;

            _spawnAnimation.Animate(_currentWagonModel, Vector3.zero, Vector3.one);
        }

        private void OnWagonUnlocked()
        {
            CreateWagonModel();
            PlaySound();
        }

        private void PlaySound()
        {
            _audioSource.clip = _wagonUnlockClip;
            _audioSource.volume = _volume * _audioSettings.MasterVolume;
            _audioSource.Play();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
