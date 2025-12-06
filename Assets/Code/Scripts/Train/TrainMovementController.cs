using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Audio;
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
        private SoundEffect _movementSoundEffect;

        [SerializeField]
        private SoundEffect _hornSoundEffect;

        [Header("Movement References")]
        [SerializeField]
        private Transform _fromPosition;

        [SerializeField]
        private Transform _toPosition;

        [SerializeField]
        private float _minSpeedFactor = 0.05f;

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

            if (_fromPosition == null || _toPosition == null)
            {
                return;
            }

            Vector3 from = _fromPosition.position;
            Vector3 to = _toPosition.position;

            Vector3 segment = to - from;
            float totalDistance = segment.magnitude;
            if (totalDistance < 0.001f)
            {
                return;
            }

            Vector3 dir = segment / totalDistance;

            float traveled = Vector3.Dot(transform.position - from, dir);
            float progress = Mathf.Clamp01(traveled / totalDistance); 

            float curve = Mathf.Sin(progress * Mathf.PI);
            float speedFactor = Mathf.Max(curve, _minSpeedFactor);

            float speed = _trainUpgradeController.Speed * speedFactor;
            float moveStep = speed * Time.fixedDeltaTime;

            float remaining = totalDistance - traveled;

            _movementSoundEffect.SetVolume(speedFactor);

            if (speed > 1f)
            {
                _movementSoundEffect.Play();
            }

            if (moveStep >= remaining)
            {
                _hornSoundEffect.Play();
                transform.position = to;
                return;
            }

            transform.position += dir * moveStep;
        }

        public void SetTrainState(TrainState state)
        {
            _trainState = state;
        }

        public void SetPositions(Transform from, Transform to)
        {
            _fromPosition = from;
            _toPosition = to;
        }
    }
}