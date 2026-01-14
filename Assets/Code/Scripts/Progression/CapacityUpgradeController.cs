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
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [SerializeField]
        private int _initialPrice = 100;

        [SerializeField]
        private float _priceMultiplier = 1.5f;

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

        [SerializeField]
        [ReadOnly]
        private int _currentPrice;

        private void Start()
        {
            //LoadData();

            foreach (WagonController wagon in _wagonControllers)
            {
                _orderUpgrade.Add(wagon);
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

            WagonController wagonController = new();

            foreach (WagonController wagon in _wagonControllers)
            {
                if (!wagon.IsUnlocked)
                {
                    continue;
                }

                wagonController = wagon;
                break;
            }

            wagonController.CapacityWagonController.IncreaseCapacity(1);

            _orderUpgrade.RemoveAt(0);
            _orderUpgrade.Add(wagonController);

            _economyController.RemoveCoins(_currentPrice);
            _pressurePlateAnimation.SuccessUnlocked();
            
            UpdatePrice();
            //SaveData();
        }

        private void UpdatePrice()
        {
            //_currentPrice = _initialPrice + (_capacity * 50);
            //_pressurePlateController.SetNewText(_currentPrice.ToString());
        }
    }
}
