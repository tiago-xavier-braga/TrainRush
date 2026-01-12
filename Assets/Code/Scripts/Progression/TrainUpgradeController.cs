using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;
using XaviGames.Train;

namespace XaviGames.Progression
{
    public class TrainUpgradeController : MonoBehaviour
    {
        [SerializeField]
        private TrainData _trainData;

        [SerializeField]
        private FloatVariable _speedRespawnValue = null;

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
        private IntModel _trainOrderSaveModel = null;

        [SerializeField]
        private IntModel _tierSpeedRespawnRight = null;

        [SerializeField]
        private DataController _dataController;

        private GameObject _currentTrainModel;
        private bool _isFirstSpawn = true;

        private void OnEnable()
        {
            _tierSpeedRespawnRight.OnValueChanged += OnSpeedRespawnChanged;
        }

        private void OnDisable()
        {
            _tierSpeedRespawnRight.OnValueChanged -= OnSpeedRespawnChanged;
        }

        private void Start()
        {
            LoadData();
            CreateTrainModel();
        }


        public void OnSpeedRespawnChanged(int value)
        {
            VerifyTrainUpgrade();
            SaveData();
        }

        private void LoadData()
        {
            int trainOrder = 0;

            trainOrder = _trainOrderSaveModel.Value;

            _trainData = _allTrainData.Find(td => td.TrainOrder == trainOrder);

            if (_trainData == null)
            {
                Debug.LogWarning($"No TrainData found for order {trainOrder}. Using the first in the list.");
                _trainData = _allTrainData[0];
            }
        }

        private void SaveData()
        {
            _trainOrderSaveModel.SetValue(_trainData.TrainOrder);
            _dataController.SaveModel(_trainOrderSaveModel);
        }

        private void VerifyTrainUpgrade()
        {
            if (_speedRespawnValue.Value > _trainData.MaxUpdateSpeed)
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
                _soundEffect.PlayOneShort();
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