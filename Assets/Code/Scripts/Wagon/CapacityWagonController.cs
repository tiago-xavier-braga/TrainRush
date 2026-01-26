using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Events;
using XaviGames.Managers;
using XaviGames.Progression;
using XaviGames.SaveSystem;
using XaviGames.Train;

namespace XaviGames.Wagon
{
    public class CapacityWagonController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public int Capacity { get; private set; } = 0;

        [SerializeField]
        private WagonBoardingAnimation _wagonBoardingAnimation;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private IntModel _tierCapacityUpgrade;

        [SerializeField]
        private ProgressionSettings _progressionSettings;

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
            _tierCapacityUpgrade.OnValueChanged += (_) => UpdateCapacity();
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
            _tierCapacityUpgrade.OnValueChanged -= (_) => UpdateCapacity();
        }

        public void UpdateCapacity()
        {
            Capacity = _progressionSettings.GetCapacity(_tierCapacityUpgrade.Value);
            OnCapacityChanged?.Invoke(Capacity);
        }

        public void OccupySeat()
        {
            if (CurrentBoarded < Capacity)
            {
                CurrentBoarded++;
                _wagonBoardingAnimation.UpdateBoardingVisuals(CurrentBoarded);
            }
        }

        private void ResetCapacity()
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