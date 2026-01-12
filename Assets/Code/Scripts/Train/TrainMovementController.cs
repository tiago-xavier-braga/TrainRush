using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Managers;
using XaviGames.Progression;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        private float _speed;

        [field: SerializeField]
        public TrainState TrainState { get; private set; } = TrainState.Idle;

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

        public UnityAction OnTrainDeparted;
        public UnityAction OnRouteCompleted;

        private void FixedUpdate()
        {
            Move();
        }

        [Button]
        public void Departing()
        {
            _hornSoundEffect.Play();
            SetPositionCalculate(_stationTransform.position, _endTransform.position);
            TrainState = TrainState.Departing;
            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
            OnTrainDeparted?.Invoke();
        }

        [Button]
        public void Approaching()
        {
            SetPositionCalculate(_spawnTransform.position, _stationTransform.position);
            TrainState = TrainState.Approaching;
            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
        }

        private void WaitingForSignal()
        {
            _hornSoundEffect.Play();
            TrainState = TrainState.WaitingForSignal;
        }

        private void FinalizeMovement()
        {
            TrainState = TrainState.Idle;
            OnRouteCompleted?.Invoke();
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

            if (TrainState == TrainState.Idle || TrainState == TrainState.WaitingForSignal)
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

            switch (TrainState)
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