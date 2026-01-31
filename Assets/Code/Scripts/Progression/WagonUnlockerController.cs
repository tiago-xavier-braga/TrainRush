using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.EconomySystem;
using XaviGames.PressurePlate;
using XaviGames.SaveSystem;
using XaviGames.Wagon;

namespace XaviGames.Progression
{
    public class WagonUnlockerController : MonoBehaviour
    {
        [SerializeField]
        private List<WagonController> _wagonControllers;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private ProgressionSettings _progressionSettings;

        [SerializeField]
        private PressurePlateController _pressurePlateController;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        private int price = 0;

        private void Start()
        {
            int lockedWagonsCount = _wagonControllers.Count(wagon => !wagon.IsUnlocked);
            _pressurePlateController.gameObject.SetActive(lockedWagonsCount > 0);

            UpdatePrice();
        }

        private void Update()
        {

            _pressurePlateAnimation.SetAnimationEnabled(_playerCoinsModel.Value >= price);
        }

        public void TryUnlockWagon()
        {
            List<WagonController> lockedWagons = _wagonControllers.FindAll(wagon => !wagon.IsUnlocked);

            if (lockedWagons.Count == 0)
            {
                Debug.Log("All wagons are already unlocked.");
                return;
            }

            if (price > _playerCoinsModel.Value)
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            _economyController.RemoveCoins(price);
            lockedWagons.First().UnlockWagon();
            _pressurePlateAnimation.SuccessUnlocked();

            if (lockedWagons.Count == 1)
            {
                _pressurePlateController.Despawn();
            }
            else
            {
                UpdatePrice();
            }
        }

        private void UpdatePrice()
        {
            int unlockedCount = _wagonControllers.Count(wagon => wagon.IsUnlocked);
            price = _progressionSettings.GetWagonPrice(unlockedCount);
            _pressurePlateController.SetNewText(price.ToString());
        }
    }
}