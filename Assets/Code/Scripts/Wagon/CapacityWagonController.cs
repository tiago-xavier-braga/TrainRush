using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;
using XaviGames.Events;
using XaviGames.Interaction;
using XaviGames.Managers;
using XaviGames.Train;

namespace XaviGames.Wagon
{
    public class CapacityWagonController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public int Capacity { get; set; } = 0;

        [SerializeField]
        private WagonController _wagonController;

        [SerializeField]
        private WagonUpgradeController _wagonUpgradeController;

        [SerializeField]
        private BoardingQueueController _boardingQueueController;

        [SerializeField]
        private PassengerBoardingAnimation _passengerBoardingAnimation;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private VoidEventChannel _routeCompletedEventChannel;

        [Header("Debug")]
        [field: SerializeField]
        [field: ReadOnly]
        public int CurrentBoardedPassengers { get; private set; } = 0;

        private TrainState _currentTrainState = TrainState.Idle;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(HandleTrainStateChanged);
            _routeCompletedEventChannel.Subscribe(HandleTrainMovementFinished);
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(HandleTrainStateChanged);
            _routeCompletedEventChannel.Unsubscribe(HandleTrainMovementFinished);
        }

        public void IncreaseCapacity(int amount)
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            if (!_wagonController.IsUnlocked)
            {
                return;
            }
            Capacity += amount;
        }

        private void Update()
        {
            if (_currentTrainState != TrainState.WaitingForSignal)
            {
                return;
            }

            int availableSeats = Capacity - CurrentBoardedPassengers;

            if (availableSeats <= 0)
            {
                return;
            }

            CharacterMovementController characterMovement = _boardingQueueController.ReleaseCharacterPosition();

            //TODO: Refactor boarding process
            //if (characterMovement != null)
            //{
            //    _characterSpawnController.DisableCharacter(characterMovement.gameObject);
            //    CurrentBoardedPassengers++;
            //    _passengerBoardingAnimation.OnPassengersBoarded(CurrentBoardedPassengers);
            //}
        }

        private void HandleTrainMovementFinished()
        {
            CurrentBoardedPassengers = 0;
            _passengerBoardingAnimation.OnPassengersBoarded(CurrentBoardedPassengers);
        }

        private void HandleTrainStateChanged(object state)
        {
            if (state is TrainState trainState)
            {
                _currentTrainState = trainState;
            }
        }


        //TODO: Refactor wagon upgrade verification
        //private void VerifyWagonUpgrade()
        //{
        //    foreach (var threshold in _wagonController.WagonData.CapacityThresholds)
        //    {
        //        if (Capacity >= threshold)
        //        {
        //            _wagonController.UpgradeWagon();
        //        }
        //    }
        //}
    }
}