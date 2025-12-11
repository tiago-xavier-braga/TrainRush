using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Managers;
using XaviGames.SaveSystem;
using AudioSettings = XaviGames.Audio.AudioSettings;

namespace XaviGames.Train
{
    public class TrainUpgradeController : MonoBehaviour
    {
        [field: SerializeField]
        public int Speed { get; private set; }

        [SerializeField]
        private int _firstSpeed;

        [SerializeField]
        private TrainData _trainData;

        [SerializeField]
        private Transform _modelTransform;

        [SerializeField]
        private SpawnAnimation _spawnAnimation;

        [SerializeField]
        private SoundEffect _soundEffect;

        [Space(8f)]
        [SerializeField]
        private List<TrainData> _allTrainData;

        [Header("Save System")]
        [SerializeField]
        private Model _trainOrderSaveModel = null;

        [SerializeField]
        private Model _speedTrainSaveModel = null;

        private GameObject _currentTrainModel;
        private bool _isFirstSpawn = true;

        private void Start()
        {
            LoadData();
            CreateTrainModel();
        }

        public void IncreaseSpeed(int amount)
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Speed += amount;
            VerifyTrainUpgrade();
            SaveData();
        }

        private void LoadData()
        {
            int trainOrder = 0;

            if (_trainOrderSaveModel.Value != null)
            {
                trainOrder = (int)_trainOrderSaveModel.Value;
            }

            _trainData = _allTrainData.Find(td => td.TrainOrder == trainOrder);

            if (_trainData == null)
            {
                Debug.LogWarning($"No TrainData found for order {trainOrder}. Using the first in the list.");
                _trainData = _allTrainData[0];
            }

            if (_speedTrainSaveModel.Value != null)
            {
                Speed = (int)_speedTrainSaveModel.Value;
            }

            Speed = Speed > _firstSpeed ? Speed : _firstSpeed;
        }

        private void SaveData()
        {
            if (_trainData == null)
            {
                return;
            }

            _trainOrderSaveModel.Value = _trainData.TrainOrder;
            _speedTrainSaveModel.Value = Speed;
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

            _spawnAnimation.Animate(_currentTrainModel, Vector3.zero, Vector3.one);

            if (!_isFirstSpawn)
            {
                _soundEffect.Play();
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