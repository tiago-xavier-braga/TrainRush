using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Audio;
using XaviGames.EconomySystem;
using XaviGames.Events;
using XaviGames.Train;
using XaviGames.Wagon;

namespace XaviGames.Progression
{
    public class PassengerRevenueController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private ProgressionSettings _progressionSettings;

        [SerializeField]
        private EconomyController _economyController;

        [SerializeField]
        private CameraFacingTextAnimation _cameraFacingTextAnimation;

        [SerializeField]
        private SoundEffect _coinSoundEffect;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private List<CapacityWagonController> _capacityWagonController;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(TrainStateChanged);
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
        }

        private void TrainStateChanged(object value)
        {
            TrainState trainState = (TrainState)value;
            
            if (trainState != TrainState.Departing)
            {
                return;
            }

            GrantCoins();
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
            foreach (var controller in _capacityWagonController)
            {
                totalPassengers += controller.CurrentBoarded;
            }

            return totalPassengers * _progressionSettings.TicketPrice;
        }
    }
}
