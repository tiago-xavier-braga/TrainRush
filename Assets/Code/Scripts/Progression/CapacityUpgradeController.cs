using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.EconomySystem;
using XaviGames.PressurePlate;
using XaviGames.SaveSystem;

namespace XaviGames.Progression
{
    public class CapacityUpgradeController : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _capacity;

        [SerializeField]
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [SerializeField]
        private int _initialPrice = 100;

        [SerializeField]
        private float _priceMultiplier = 1.5f;

        [Header("Save System")]
        [SerializeField]
        private IntModel _capacityModel;

        [SerializeField]
        private DataController _dataController;

        [Header("Economy References")]
        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private List<WagonUpgradeController> _wagonUpgradeControllers;

        [SerializeField]
        [ReadOnly]
        private List<WagonUpgradeController> _orderUpgrade;

        [SerializeField]
        [ReadOnly]
        private int _currentPrice;

        private void Start()
        {
            LoadValue();

            foreach (var wagonUpgradeController in _wagonUpgradeControllers)
            {
                _orderUpgrade.Add(wagonUpgradeController);
            }
        }

        private void Update()
        {
            bool canAfford = _playerCoinsModel.Value >= _currentPrice;
            _pressurePlateAnimation.SetAnimationEnabled(canAfford);
        }

        public void IncreaseCapacity()
        {
            if (_playerCoinsModel.Value < _currentPrice)
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            WagonUpgradeController wagonUpgradeController = _orderUpgrade.First();
            
            wagonUpgradeController.IncreaseCapacity(1);
            _capacity++;
            
            _orderUpgrade.RemoveAt(0);
            _orderUpgrade.Add(wagonUpgradeController);

            _economyController.RemoveCoins(_currentPrice);
            _pressurePlateAnimation.SuccessUnlocked();
            
            UpdatePrice();
            SaveValue();
        }

        private void LoadValue()
        {
            _capacity = _capacityModel.Value;

            foreach (var wagonUpgradeController in _wagonUpgradeControllers)
            {
                wagonUpgradeController.SetCapacity(_capacity / _wagonUpgradeControllers.Count);
            }

            UpdatePrice();
        }

        private void SaveValue()
        {
            _capacityModel.SetValue(_capacity);
            _dataController.SaveModel(_capacityModel);
        }

        private void UpdatePrice()
        {
            _currentPrice = _initialPrice + (_capacity * 50);
            _pressurePlateController.SetNewText(_currentPrice.ToString());
        }
    }
}
