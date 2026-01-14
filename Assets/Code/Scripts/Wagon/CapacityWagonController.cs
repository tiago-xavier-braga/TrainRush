using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;
using XaviGames.Events;
using XaviGames.Interaction;
using XaviGames.Managers;
using XaviGames.SaveSystem;
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
        private BoardingQueueController _boardingQueueController;

        [SerializeField]
        private PassengerBoardingAnimation _passengerBoardingAnimation;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private VoidEventChannel _routeCompletedEventChannel;

        [Header("Save System")]
        [SerializeField]
        private IntModel _capacityModel;

        [SerializeField]
        private DataController _dataController;

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

        private void Start()
        {
            LoadData();
        }

        public void SetCapacity(int value)
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Capacity += value;
            SaveData();
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

        private void LoadData()
        {
            Capacity = _capacityModel.Value;
        }

        private void SaveData()
        {
            _capacityModel.SetValue(Capacity);
            _dataController.SaveModel(_capacityModel);
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
    }
}