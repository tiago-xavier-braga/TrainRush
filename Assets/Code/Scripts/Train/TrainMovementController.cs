using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Events;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        [ReadOnly]
        private TrainState _trainState = TrainState.None;

        [SerializeField]
        private TrainUpgradeController _trainUpgradeController;

        [SerializeField]
        private SingleEventChannel _onTrafficLightStateChange;

        [SerializeField]
        private SoundEffect _movementSoundEffect;

        [SerializeField]
        private SoundEffect _hornSoundEffect;

        [Header("Movement References")]
        [SerializeField]
        private Transform _startPosition;

        [SerializeField]
        private Transform _stationPosition;

        [SerializeField]
        private Transform _endPosition;

        private Transform _fromStation;
        private Transform _toPosition;

        private void OnEnable()
        {
            _onTrafficLightStateChange.Subscribe(ToggleTrafficLightState);
        }

        private void OnDisable()
        {
            _onTrafficLightStateChange.Unsubscribe(ToggleTrafficLightState);
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            if (_trainState != TrainState.Moving)
            {
                return;
            }

        }

        public void SetTrainState(TrainState state)
        {
            _trainState = state;
        }

        private void ToggleTrafficLightState(object state)
        {
            bool isTrafficLight = (bool)state;

            if (isTrafficLight)
            {

            }
        }
    }
}