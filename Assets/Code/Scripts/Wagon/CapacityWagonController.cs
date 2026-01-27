using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Events;
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

        [field: SerializeField]
        [field: ReadOnly]
        public int CurrentBoarded { get; private set; } = 0;

        [SerializeField]
        private WagonBoardingAnimation _wagonBoardingAnimation;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private IntModel _tierCapacityUpgrade;

        [SerializeField]
        private ProgressionSettings _progressionSettings;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(TrainStateChanged);
            _tierCapacityUpgrade.OnValueChanged += TierUpgradeChanged;
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
            _tierCapacityUpgrade.OnValueChanged += TierUpgradeChanged;
        }

        private void Start()
        {
            TierUpgradeChanged(_tierCapacityUpgrade.Value);
            ResetCapacity();
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

        private void TrainStateChanged(object state)
        {
            TrainState _currentTrainState = (TrainState)state;

            if (_currentTrainState == TrainState.Finalized)
            {
                ResetCapacity();
            }
        }

        private void TierUpgradeChanged(int newTier)
        {
            Capacity = _progressionSettings.GetCapacity(newTier);
        }
    }
}