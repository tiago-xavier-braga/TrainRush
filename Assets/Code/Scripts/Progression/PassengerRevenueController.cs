using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Audio;
using XaviGames.EconomySystem;
using XaviGames.Events;
using XaviGames.Wagon;

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
        private CameraFacingTextAnimation _cameraFacingTextAnimation;

        [SerializeField]
        private SoundEffect _coinSoundEffect;

        [SerializeField]
        private VoidEventChannel _trainDepartedEventChannel;

        [SerializeField]
        private List<CapacityWagonController> _passengerBoardingControllers;

        private void OnEnable()
        {
            _trainDepartedEventChannel.Subscribe(GrantCoins);
        }

        private void OnDisable()
        {
            _trainDepartedEventChannel.Unsubscribe(GrantCoins);
        }

        private void GrantCoins()
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
