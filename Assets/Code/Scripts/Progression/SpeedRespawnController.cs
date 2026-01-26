using UnityEngine;
using XaviGames.Attributes;
using XaviGames.EconomySystem;
using XaviGames.ObjectVariables;
using XaviGames.PressurePlate;
using XaviGames.SaveSystem;

namespace XaviGames.Progression
{
    public class SpeedRespawnController : MonoBehaviour
    {
        [Header("Progression Settings")]
        [SerializeField]
        [ReadOnly]
        private int _price;

        [SerializeField]
        private ProgressionSettings _progressionSettings;

        [SerializeField]
        [Min(0f)]
        private float _reducedPerUpgrade;

        [SerializeField]
        private float _minSpeedRespawn;

        [SerializeField]
        private FloatVariable _speedRespawnVariable;

        [Header("Plate Settings")]
        [SerializeField]
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [SerializeField]
        private EconomyController _economyController;

        [Header("Save System")]
        [SerializeField]
        private IntModel _playerCoinsModel;
        
        [SerializeField]
        private IntModel _tierSpeedRespawnModel = null;

        [SerializeField]
        private DataController _dataController = null;

        private float _baseSpeedRespawn;

        private void OnEnable()
        {
            _playerCoinsModel.OnValueChanged += UpdatePlateAnimation;
        }

        private void OnDisable()
        {
            _playerCoinsModel.OnValueChanged -= UpdatePlateAnimation;
        }

        private void Start()
        {
            _baseSpeedRespawn = _speedRespawnVariable.Value;
            UpgradePrice();
            UpdatePlateAnimation(_playerCoinsModel.Value);
            SetSpeedRespawn(_tierSpeedRespawnModel.Value);
        }

        public void TryUpgradeSpeedRespawn()
        {
            if (IsMaxLevel())
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            if (_playerCoinsModel.Value >= _price)
            {
                _economyController.RemoveCoins(_price);

                int newTier = _tierSpeedRespawnModel.Value + 1;
                _tierSpeedRespawnModel.SetValue(newTier);

                SetSpeedRespawn(newTier);
                SaveData();
                UpgradePrice();
                UpdatePlateAnimation(_playerCoinsModel.Value);
                _pressurePlateAnimation.SuccessUnlocked();
            }
            else
            {
                _pressurePlateAnimation.FailureUnlocked();
            }
        }

        private bool IsMaxLevel()
        {
            float nextSpeed = _baseSpeedRespawn - (_reducedPerUpgrade * (_tierSpeedRespawnModel.Value + 1));
            return nextSpeed <= _minSpeedRespawn;
        }

        private void SetSpeedRespawn(int tier)
        {
            float reduction = _reducedPerUpgrade * tier;
            float newSpeed = Mathf.Max(_minSpeedRespawn, _baseSpeedRespawn - reduction);
            _speedRespawnVariable.Value = newSpeed;
        }

        private void SaveData() 
        {
            _dataController.SaveModel(_tierSpeedRespawnModel);
        }

        private void UpgradePrice()
        {
            _price = _progressionSettings.GetSpeedPrice(_tierSpeedRespawnModel.Value);
            _pressurePlateController.SetNewText(_price.ToString());
        }

        private void UpdatePlateAnimation(int value)
        {
            _pressurePlateAnimation.SetAnimationEnabled(value >= _price);
        }
    }
}