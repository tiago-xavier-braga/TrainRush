using System.Collections.Generic;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Managers;
using XaviGames.SaveSystem;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Train
{
    public class TrainController : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; private set; }

        [SerializeField]
        private TrainData _trainData;

        [SerializeField]
        private Transform _modelTransform;
        
        [Space(8f)]
        [SerializeField]
        private List<TrainData> _allTrainData;

        [Header("Spawn Animation")]
        [SerializeField]
        private float _spawnDuration = 1f;

        [SerializeField]
        private LeanTweenType _spawnEaseType;

        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _trainUnlockClip;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioSettings _audioSettings;

        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;

        [Header("Save System")]
        [SerializeField]
        private Model _trainOrderSaveModel = null;

        private GameObject _currentTrainModel;
        private bool _isFirstSpawn = true;

        private void Start()
        {
            LoadData();
            CreateTrainModel();
        }

        public void IncreaseSpeed(float amount)
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Speed += amount;
            VerifyTrainUpgrade();
        }

        private void LoadData()
        {
            int trainOrder = 0;

            if (_trainOrderSaveModel != null)
            {
                trainOrder = (int)_trainOrderSaveModel.Value;
            }

            _trainData = _allTrainData.Find(td => td.TrainOrder == trainOrder);

            if (_trainData == null)
            {
                Debug.LogWarning($"No TrainData found for order {trainOrder}. Using the first in the list.");
                _trainData = _allTrainData[0];
            }
        }

        private void SaveData()
        {
            if (_trainData == null)
            {
                return;
            }

            _trainOrderSaveModel.Value = _trainData.TrainOrder;
        }

        private void VerifyTrainUpgrade()
        {
            if (Speed < _trainData.MaxUpdateSpeed)
            {
                return;
            }

            if (_trainData.TrainOrder >= _allTrainData.Count - 1)
            {
                return;
            }

            _trainData = _allTrainData.Find(td => td.TrainOrder == _trainData.TrainOrder + 1);

            SaveData();
            CreateTrainModel();
        }

        [Button()]
        private void CreateTrainModel()
        {
            if (_currentTrainModel != null)
            {
                Destroy(_currentTrainModel);
            }

            _currentTrainModel = Instantiate(_trainData.TrainPrefab, _modelTransform);
            _currentTrainModel.transform.localScale = Vector3.zero;

            LeanTween.scale(_currentTrainModel, Vector3.one, _spawnDuration).setEase(_spawnEaseType);

            if (!_isFirstSpawn)
            {
                _audioSource.clip = _trainUnlockClip;
                _audioSource.volume = _volume * _audioSettings.MasterVolume;
                _audioSource.Play();
                return;
            }

            _isFirstSpawn = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}