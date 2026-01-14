using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Events;
using XaviGames.Managers;
using XaviGames.Progression;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        private TrainState _trainState = TrainState.Idle;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private Transform _spawnTransform;

        [SerializeField]
        private Transform _stationTransform;

        [SerializeField]
        private Transform _endTransform;

        [SerializeField]
        private AnimationCurve _movementCurve;

        [SerializeField]
        private SoundEffect _movementSoundEffect;

        [SerializeField]
        private SoundEffect _hornSoundEffect;

        [Header("Debug")]
        [SerializeField]
        [ReadOnly]
        private float _cumulativeTime = 0;

        [SerializeField]
        [ReadOnly]
        private float _distance = 0;

        [SerializeField]
        [ReadOnly]
        private float _approachDuration = 0;

        [SerializeField]
        [ReadOnly]
        private Vector3 _startPos;

        [SerializeField]
        [ReadOnly]
        private Vector3 _endPos;

        [Header("Events")]
        [SerializeField]
        private VoidEventChannel _trainDepartedEventChannel;

        [SerializeField]
        private VoidEventChannel _routeCompletedEventChannel;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        private void FixedUpdate()
        {
            Move();
        }

        [Button]
        public void Approaching()
        {
            SetPositionCalculate(_spawnTransform.position, _stationTransform.position);
            _trainState = TrainState.Approaching;
            _onTrainStateChanged.RaiseEvent(_trainState);

            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
        }

        [Button]
        public void Departing()
        {
            SetPositionCalculate(_stationTransform.position, _endTransform.position);
            
            _trainState = TrainState.Departing;
            _onTrainStateChanged.RaiseEvent(_trainState);
            _trainDepartedEventChannel?.RaiseEvent();
            
            _hornSoundEffect.Play();
            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
        }

        private void WaitingForSignal()
        {
            _hornSoundEffect.Play();
            _trainState = TrainState.WaitingForSignal;
            _onTrainStateChanged.RaiseEvent(_trainState);
        }

        private void FinalizeMovement()
        {
            _trainState = TrainState.Idle;
            _onTrainStateChanged.RaiseEvent(_trainState);
            _routeCompletedEventChannel?.RaiseEvent();
        }

        private void SetPositionCalculate(Vector3 startPosition, Vector3 endPosition)
        {
            _distance = Mathf.Abs(endPosition.z - startPosition.z);
            _approachDuration = _distance / _speed;

            _cumulativeTime = 0;
            _startPos = startPosition;
            _endPos = endPosition;

            transform.position = startPosition;
        }

        private void Move()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                _movementSoundEffect.Pause();
                return;
            }
            else
            {
                _movementSoundEffect.Resume();
            }

            if (_trainState == TrainState.Idle || _trainState == TrainState.WaitingForSignal)
            {
                return;
            }

            _cumulativeTime += Time.fixedDeltaTime;
            float time = Mathf.Clamp01(_cumulativeTime / _approachDuration);
            float easedTime = _movementCurve.Evaluate(time);

            transform.position = Vector3.Lerp(_startPos, _endPos, easedTime);

            _movementSoundEffect.SetVolume(time);

            if (time < 1f)
            {
                return;
            }

            _movementSoundEffect.Stop();

            switch (_trainState)
            {
                case TrainState.Approaching:
                    WaitingForSignal();
                    break;
                case TrainState.Departing:
                    FinalizeMovement();
                    break;
            }
        }
    }
}