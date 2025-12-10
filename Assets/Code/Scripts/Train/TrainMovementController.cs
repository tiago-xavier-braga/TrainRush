using UnityEngine;
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
        private Vector3 _startPos;

        [SerializeField]
        private Vector3 _endPos;

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
            _distance = Mathf.Abs( _stationTransform.position.z - _spawnTransform.position.z);
            _approachDuration = _distance / _trainUpgradeController.Speed;
            
            _cumulativeTime = 0;
            _startPos = _spawnTransform.position;
            _endPos = _stationTransform.position;

            transform.position = _spawnTransform.position;

            _trainState = TrainState.Approaching;
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

            if (time >= 1f)
            {
                _trainState = TrainState.WaitingForSignal;
            }
        }
    }
}