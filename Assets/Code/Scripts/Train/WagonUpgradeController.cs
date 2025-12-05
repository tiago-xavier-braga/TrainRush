using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.Managers;
using XaviGames.SaveSystem;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Train
{
    public class WagonUpgradeController : MonoBehaviour
    {
        [field: SerializeField]
        public float Capacity {  get; set; }

        [SerializeField]
        private WagonData _wagonData;

        [SerializeField]
        private Transform _modelTransform;

        [SerializeField]
        private List<WagonData> _allWagonData;

        [Header("Spawn Animation")]
        [SerializeField]
        private SpawnAnimation _spawnAnimation;

        [Header("Save System")]
        [SerializeField]
        private Model _wagonOrderSaveModel = null;

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

        private void Start()
        {
            LoadData();
            CreateWagonModel();
        }

        public void IncreaseCapacity(float amount)
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Capacity += amount;
            VerifyWagonUpgrade();
        }

        private void LoadData()
        {
            int wagonOrder = 0;

            if (_wagonOrderSaveModel != null)
            {
                wagonOrder = (int)_wagonOrderSaveModel.Value;
            }

            _wagonData = _allWagonData.Find(wd => wd.WagonOrder == wagonOrder);

            if (_wagonData == null)
            {
                Debug.LogWarning($"No WagonData found for order {wagonOrder}. Using the first in the list.");
                _wagonData = _allWagonData[0];
            }
        }

        private void SaveData()
        {
            if (_wagonData == null)
            {
                return;
            }

            _wagonOrderSaveModel.Value = _wagonData.WagonOrder;
        }

        private void VerifyWagonUpgrade()
        {
            if (Capacity < _wagonData.MaxUpdateCapacity)
            {
                return;
            }

            if (_wagonData.WagonOrder >= _allWagonData.Count - 1)
            {
                return;
            }

            _wagonData = _allWagonData.Find(wd => wd.WagonOrder == _wagonData.WagonOrder + 1);

            SaveData();
            CreateWagonModel();
        }

        [Button()]
        private void CreateWagonModel()
        {
            if (_currentWagonModel != null)
            {
                Destroy(_currentWagonModel);
            }

            _currentWagonModel = Instantiate(_wagonData.WagonPrefab, _modelTransform);
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
