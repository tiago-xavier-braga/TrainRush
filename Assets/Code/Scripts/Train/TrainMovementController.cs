using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private TrainState _trainState = TrainState.Approaching;

        [SerializeField]
        private TrainController _trainController;

        [SerializeField]
        private TrafficLightController _trafficLightController;

        [SerializeField]
        private Transform _startPosition;
        
        [SerializeField]
        private Transform _stationPosition;

        [SerializeField]
        private Transform _endPosition;

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            switch (_trainState)
            {
                case TrainState.None:
                    break;
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

            if (Vector3.Distance(transform.position, _stationPosition.position) < 0.1f)
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

            if (Vector3.Distance(transform.position, _endPosition.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        private float EaseAcceleration(float from, float to, float current)
        {
            float value = Mathf.InverseLerp(from, to, current);
            return Mathf.Lerp(0f, _trainController.Speed, value);
        }
    }
}