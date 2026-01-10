using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Audio;
using XaviGames.EconomySystem;
using XaviGames.Interaction;
using XaviGames.Train;

namespace XaviGames.Progression
{
    public class PassengerRevenueController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private int _ticketPrice;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private CameraFacingTextAnimation _cameraFacingTextAnimation;

        [SerializeField]
        private SoundEffect _coinSoundEffect;

        [SerializeField]
        private List<PassengerBoardingController> _passengerBoardingControllers;

        private void OnEnable()
        {
            _trainMovementController.OnTrainDeparted += GrantCoins;
        }

        private void OnDisable()
        {
            _trainMovementController.OnTrainDeparted -= GrantCoins;
        }

        public void GrantCoins()
        { 
            int reward = CalculateReward();

            if (reward == 0)
            {
                return;
            }

            _cameraFacingTextAnimation.SetText($"+{reward}");
            _cameraFacingTextAnimation.Enable();
            _coinSoundEffect.PlayOneShort();
            _economyController.AddCoins(reward);
        }

        private int CalculateReward()
        {
            int totalPassengers = 0;
            foreach (var controller in _passengerBoardingControllers)
            {
                totalPassengers += controller.CurrentBoardedPassengers;
            }

            return totalPassengers * _ticketPrice;
        }
    }
}
