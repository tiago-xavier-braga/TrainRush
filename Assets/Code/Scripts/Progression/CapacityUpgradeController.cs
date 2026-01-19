using System.Collections.Generic;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.EconomySystem;
using XaviGames.PressurePlate;
using XaviGames.SaveSystem;
using XaviGames.Wagon;

namespace XaviGames.Progression
{
    public class CapacityUpgradeController : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _price;

        [SerializeField]
        private IntModel _tierCapacityModel;

        [SerializeField]
        private DataController _dataController;

        [Header("Plate Settings")]
        [SerializeField]
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [Header("Economy References")]
        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private List<CapacityWagonController> _capacityWagonControllers;

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
            UpgradePrice();
            UpdatePlateAnimation(_playerCoinsModel.Value);
        }

        //TODO: Implement the tier system for capacity upgrades
        public void TryIncreaseCapacity()
        {
            if (_playerCoinsModel.Value < _price)
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            foreach (var wagon in _capacityWagonControllers)
            {
                wagon.SetCapacity(_tierCapacityModel.Value);
            }

            _economyController.RemoveCoins(_price);
            _pressurePlateAnimation.SuccessUnlocked();

            UpgradePrice();
            SaveData();
        }

        private void SaveData()
        {
            _dataController.SaveModel(_tierCapacityModel);
        }

        private void UpgradePrice()
        {
            _price = GetPrice();
            _pressurePlateController.SetNewText(_price.ToString());
        }

        private int GetCapacity()
        {

            return new int();
        }

        private int GetPrice()
        {
            float baseCost = 35f * Mathf.Pow(_tierCapacityModel.Value + 1, 2.4f);
            return Mathf.RoundToInt(baseCost / 50f) * 50;
        }

        private void UpdatePlateAnimation(int value)
        {
            _pressurePlateAnimation.SetAnimationEnabled(value >= _price);
        }
    }
}
