using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        private TrainState _trainState = TrainState.Idle;

        [SerializeField]
        private TrainUpgradeController _trainUpgradeController;

        [SerializeField]
        private Transform _spawnTransform;

        [SerializeField]
        private Transform _stationTransform;

        [SerializeField]
        private Transform _endTransform;

        [SerializeField]
        private AnimationCurve _movementCurve;

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

        public UnityAction OnMovementFinished;

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Move();
        }


        public void SetTrainState(TrainState trainState)
        {
            _trainState = trainState;
        }

        [Button]
        public void EnterApproaching()
        {
            SetPositionCalculate(_spawnTransform.position, _stationTransform.position);
            _trainState = TrainState.Approaching;
        }

        [Button]
        public void EnterDeparting()
        {
            SetPositionCalculate(_stationTransform.position, _endTransform.position);
            _trainState = TrainState.Departing;
        }

        [Button]
        public void FinalizeMovement()
        {
            OnMovementFinished?.Invoke();
        }

        private void SetPositionCalculate(Vector3 startPosition, Vector3 endPosition)
        {
            _distance = Mathf.Abs(endPosition.z - startPosition.z);
            _approachDuration = _distance / _trainUpgradeController.Speed;

            _cumulativeTime = 0;
            _startPos = startPosition;
            _endPos = endPosition;

            transform.position = startPosition;
        }

        private void Move()
        {
            if (_trainState == TrainState.Idle || _trainState == TrainState.WaitingForSignal)
            {
                return;
            }

            _cumulativeTime += Time.fixedDeltaTime;
            float time = Mathf.Clamp01(_cumulativeTime / _approachDuration);
            float easedTime = _movementCurve.Evaluate(time);

            transform.position = Vector3.Lerp(_startPos, _endPos, easedTime);

            if (time < 1f)
            {
                return;
            }

            switch (_trainState)
            {
                case TrainState.Approaching:
                    _trainState = TrainState.WaitingForSignal;
                    break;
                case TrainState.Departing:
                    _trainState = TrainState.Idle;
                    FinalizeMovement();
                    break;
            }
        }
    }
}