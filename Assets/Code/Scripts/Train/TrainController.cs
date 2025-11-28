using System;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainController : MonoBehaviour
    {
        [Serializable]
        private enum TrainState
        {
            Approaching = 0,
            WaitingTrafficLight = 1,
            Departing = 2,
        }

        [SerializeField]
        [ReadOnly]
        private TrainState _trainState = TrainState.Approaching;

        [SerializeField]
        private TrainData _trainData;

        [field: SerializeField]
        public float Speed { get; private set; }

        private TrainLoopController _trainLoopController;
        private TrafficLightController _trafficLightController;
        private GameManager _gameManager;

        private Transform _startPosition;
        private Transform _stationPosition;
        private Transform _endPosition;

        public void Initialize(TrainLoopController trainLoopController)
        {
            _trainLoopController = trainLoopController;
            _trafficLightController = _trainLoopController.TrafficLightController;
            _gameManager = GameManager.Instance;
            Speed = _trainData.MinSpeed;
            _startPosition = _trainLoopController.StartTransform;
            _stationPosition = _trainLoopController.StationTransform;
            _endPosition = _trainLoopController.EndTransform;
        }

        private void FixedUpdate()
        {
            if (_gameManager.GameState != GameState.Running)
            {
                return;
            }

            switch (_trainState)
            {
                case TrainState.Approaching:
                    RunApproach();
                    break;
                case TrainState.WaitingTrafficLight:
                    WaitingTrafficLight();
                    break;
                case TrainState.Departing:
                    RunDeparting();
                    break;
            }
        }

        private void RunApproach()
        {
            float speed = EaseAcceleration(_startPosition.position.z, _stationPosition.position.z, transform.position.z);
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, _trainLoopController.StationTransform.position) < 0.1f)
            {
                _trainState = TrainState.WaitingTrafficLight;
            }
        }

        private void WaitingTrafficLight()
        {
            if (_trafficLightController.IsRedLight)
            {
                return;
            }

            _trainState = TrainState.Departing;
        }

        private void RunDeparting()
        {
            float speed = EaseAcceleration(_stationPosition.position.z, _endPosition.position.z, transform.position.z);
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, _trainLoopController.EndTransform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        private float EaseAcceleration(float from, float to, float current)
        {
            float value = Mathf.InverseLerp(from, to, current);
            return Mathf.Lerp(_trainData.MinSpeed, _trainData.MaxSpeed, value);
        }
    }
}