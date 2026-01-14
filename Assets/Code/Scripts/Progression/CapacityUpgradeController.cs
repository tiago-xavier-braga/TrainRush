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
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [SerializeField]
        private IntModel _tierCapacityModel;

        [SerializeField]
        private DataController _dataController;

        [Header("Economy References")]
        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private List<WagonController> _wagonControllers;

        [SerializeField]
        [ReadOnly]
        private List<WagonController> _orderUpgrade;

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
            _orderUpgrade = new List<WagonController>(_wagonControllers);
        }

        //TODO: Implement the tier system for capacity upgrades
        public void IncreaseCapacity()
        {
            if (_playerCoinsModel.Value < _price)
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            WagonController wagonController = new();

            foreach (WagonController wagon in _orderUpgrade)
            {
                if (!wagon.IsUnlocked)
                {
                    continue;
                }

                wagonController = wagon;
                break;
            }

            wagonController.CapacityWagonController.SetCapacity(_amountPerUpgrade);

            _orderUpgrade.Remove(wagonController);
            _orderUpgrade.Add(wagonController);

            _economyController.RemoveCoins(_price);
            _pressurePlateAnimation.SuccessUnlocked();
            
            UpdatePrice();
            SaveData();
        }

        private void SaveData()
        {
            _dataController.SaveModel(_tierCapacityModel);
        }

        private void UpdatePrice()
        {
            //_currentPrice = _initialPrice + (_capacity * 50);
            //_pressurePlateController.SetNewText(_currentPrice.ToString());
        }

        private void UpdatePlateAnimation(int value)
        {
            _pressurePlateAnimation.SetAnimationEnabled(value >= _price);
        }
    }
}
