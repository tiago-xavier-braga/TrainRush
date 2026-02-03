using UnityEngine;
using XaviGames.Animation;
using XaviGames.Events;
using XaviGames.PressurePlate;
using XaviGames.Train;

namespace XaviGames.Interaction
{
    public class TrafficLightController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private PressurePlateAnimation _pressurePlateAnimation;

        [SerializeField]
        private LeverAnimation _leverAnimation;

        [SerializeField]
        private TrafficLight _redLight;

        [SerializeField]
        private TrafficLight _yellowLight;

        [SerializeField]
        private TrafficLight _greenLight;

        private TrainState _trainState;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(OnTrainStateChanged);
        }

        private void OnDisable()
        {
            _onTrainStateChanged?.Unsubscribe(OnTrainStateChanged);
        }

        private void Start()
        {
            _pressurePlateAnimation.SetAnimationEnabled(_trainState == TrainState.WaitingForSignal);
        }

        public void ReleaseTrain()
        {
            if (_trainState != TrainState.WaitingForSignal)
            {
                _pressurePlateAnimation.FailureUnlocked();
                return;
            }

            _greenLight.Enable();
            _redLight.Disable();
            _trainMovementController.Departing();
            _leverAnimation.EnableAnimation();
            _pressurePlateAnimation.SuccessUnlocked();
            _pressurePlateAnimation.SetAnimationEnabled(false);
        }

        private void ResetTrafficLight()
        {
            _redLight.Enable();
            _yellowLight.Disable();
            _greenLight.Disable();
            _leverAnimation.ResetAnimation();
        }

        private void OnTrainStateChanged(object state)
        {
            _trainState = (TrainState)state;

            switch (_trainState)
            {
                case TrainState.Approaching:
                    ResetTrafficLight();
                    break;
                case TrainState.WaitingForSignal:
                    _pressurePlateAnimation.SetAnimationEnabled(true);
                    break;
                case TrainState.Finalized:
                    _greenLight.Disable();
                    _yellowLight.Enable();
                    break;
            }
        }
    }
}
