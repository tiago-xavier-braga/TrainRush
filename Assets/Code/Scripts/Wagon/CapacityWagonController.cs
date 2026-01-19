using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Events;
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
        private WagonBoardingAnimation _wagonBoardingAnimation;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [Header("Save System")]
        [SerializeField]
        private IntModel _capacityModel;

        [SerializeField]
        private DataController _dataController;

        [Header("Debug")]
        [field: SerializeField]
        [field: ReadOnly]
        public int CurrentBoarded { get; private set; } = 0;

        private TrainState _currentTrainState = TrainState.Idle;

        public UnityAction OnAvailableSeatsChanged = null;
        public UnityAction<int> OnCapacityChanged = null;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(TrainStateChanged);
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
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

            Capacity = value;
            OnCapacityChanged?.Invoke(Capacity);
            SaveData();
        }

        public void OccupySeat()
        {
            if (CurrentBoarded < Capacity)
            {
                CurrentBoarded++;
                _wagonBoardingAnimation.UpdateBoardingVisuals(CurrentBoarded);
            }
        }

        public void ResetCapacity()
        {
            CurrentBoarded = 0;
            _wagonBoardingAnimation.UpdateBoardingVisuals(CurrentBoarded);
        }

        private void Update()
        {
            if (_currentTrainState != TrainState.WaitingForSignal)
            {
                return;
            }

            int availableSeats = Capacity - CurrentBoarded;

            if (availableSeats <= 0)
            {
                return;
            }

            OnAvailableSeatsChanged?.Invoke();
            return;
        }

        private void LoadData()
        {
            Capacity = _capacityModel.Value;
            OnCapacityChanged?.Invoke(Capacity);
        }

        private void SaveData()
        {
            _capacityModel.SetValue(Capacity);
            _dataController.SaveModel(_capacityModel);
        }

        private void TrainStateChanged(object state)
        {
            if (state is TrainState trainState)
            {
                _currentTrainState = trainState;
            }

            if (_currentTrainState == TrainState.Finalized)
            {
                ResetCapacity();
            }
        }
    }
}