using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;
using XaviGames.Progression;
using XaviGames.Train;

namespace XaviGames.Interaction
{
    public class PassengerBoardingController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private WagonUpgradeController _wagonUpgradeController;

        [SerializeField]
        private BoardingQueueController _boardingQueueController;

        [SerializeField]
        private CharacterSpawnController _characterSpawnController;

        [SerializeField]
        private PassengerBoardingAnimation _passengerBoardingAnimation;

        [Header("Debug")]
        [field: SerializeField]
        [field: ReadOnly]
        public int CurrentBoardedPassengers { get; private set; } = 0;

        private void OnEnable()
        {
            _trainMovementController.OnRouteCompleted += HandleTrainMovementFinished;
        }

        private void OnDisable()
        {
            _trainMovementController.OnRouteCompleted -= HandleTrainMovementFinished;
        }

        private void Update()
        {
            if (_trainMovementController.TrainState != TrainState.WaitingForSignal)
            {
                return;
            }

            int availableSeats = _wagonUpgradeController.Capacity - CurrentBoardedPassengers;

            if (availableSeats <= 0)
            {
                return;
            }

            CharacterMovementController characterMovement = _boardingQueueController.ReleaseCharacterPosition();

            if (characterMovement != null)
            {
                _characterSpawnController.DisableCharacter(characterMovement.gameObject);
                CurrentBoardedPassengers++;
                _passengerBoardingAnimation.OnPassengersBoarded(CurrentBoardedPassengers);
            }
        }

        private void HandleTrainMovementFinished()
        {
            CurrentBoardedPassengers = 0;
            _passengerBoardingAnimation.OnPassengersBoarded(CurrentBoardedPassengers);
        }
    }
}