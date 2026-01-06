using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        [Min(0f)]
        private float _timeToRestart;

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
            TrainState = trainState;
        }

        [Button]
        public void Approaching()
        {
            SetPositionCalculate(_spawnTransform.position, _stationTransform.position);
            TrainState = TrainState.Approaching;
            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
        }

        [Button]
        public void Departing()
        {
            _hornSoundEffect.Play();
            SetPositionCalculate(_stationTransform.position, _endTransform.position);
            TrainState = TrainState.Departing;
            _movementSoundEffect.Play();
            _movementSoundEffect.SetVolume(0f);
        }

        public void WaitingForSignal()
        {
            _hornSoundEffect.Play();
            TrainState = TrainState.WaitingForSignal;
        }

        private IEnumerator FinalizeMovement()
        {
            TrainState = TrainState.Idle;
            yield return new WaitForSeconds(_timeToRestart);
            OnMovementFinished?.Invoke();
            Approaching();
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
                    StartCoroutine(FinalizeMovement());
                    break;
            }
        }
    }
}