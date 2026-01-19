using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Wagon
{
    public class WagonSpawnController : MonoBehaviour
    {
        [SerializeField]
        private WagonController _wagonController;
        
        [SerializeField]
        private CapacityWagonController _capacityWagonController;

        [Header("Wagons")]
        [SerializeField]
        private List<WagonData> _availableWagons = new();

        [Space()]
        [Header("Spawn")]

        [SerializeField]
        private Transform _spawnTransform;
        
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

        private WagonData _currentWagonData;
        private GameObject _currentWagonObject;
        private int _currentCapacity = 0;

        private void OnEnable()
        {
            _capacityWagonController.OnCapacityChanged += CapacityValueChanged;
        }

        private void OnDisable()
        {
            _capacityWagonController.OnCapacityChanged -= CapacityValueChanged;
        }
        
        public void CreateWagon()
        {
            if (!_wagonController.IsUnlocked)
            {
                return;
            }

            if (_currentWagonObject != null)
            {
                Destroy(_currentWagonObject);
            }

            _currentWagonData = GetWagonData();

            _currentWagonObject = Instantiate(_currentWagonData.WagonPrefab, _spawnTransform);
            _currentWagonObject.transform.localScale = Vector3.zero;

            _spawnAnimation.Animate(_currentWagonObject, Vector3.zero, Vector3.one);
        }

        private WagonData GetWagonData()
        {
            foreach (var wagonData in _availableWagons)
            {
                if (wagonData.MinCapacity < _currentCapacity && _currentCapacity <= wagonData.MaxCapacity)
                {
                    return wagonData;
                }
            }

            return null;
        }

        private void VerifyUpgradeWagon()
        {
            if (_currentCapacity > _currentWagonData.MaxCapacity)
            {
                CreateWagon();
                PlaySound();
            }
        }
        
        private void PlaySound()
        {
            _audioSource.clip = _wagonUnlockClip;
            _audioSource.volume = _volume * _audioSettings.MasterVolume;
            _audioSource.Play();
        }

        private void CapacityValueChanged(int value)
        {
            _currentCapacity = value;
            VerifyUpgradeWagon();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
