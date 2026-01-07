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

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private int _currentBoardedPassengers = 0;

        private void OnEnable()
        {
            _trainMovementController.OnMovementFinished += HandleTrainMovementFinished;
        }

        private void OnDisable()
        {
            _trainMovementController.OnMovementFinished -= HandleTrainMovementFinished;
        }

        private void Update()
        {
            if (_trainMovementController.TrainState != TrainState.WaitingForSignal)
            {
                return;
            }

            int availableSeats = _wagonUpgradeController.Capacity - _currentBoardedPassengers;

            if (availableSeats <= 0)
            {
                return;
            }

            CharacterMovementController characterMovement = _boardingQueueController.ReleaseCharacterPosition();

            if (characterMovement != null)
            {
                _characterSpawnController.DisableCharacter(characterMovement.gameObject);
                _currentBoardedPassengers++;
            }
        }

        private void HandleTrainMovementFinished()
        {
            _currentBoardedPassengers = 0;
        }
    }
}