using UnityEngine;
using XaviGames.Managers;
using XaviGames.ObjectVariables;

namespace XaviGames.Characters
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _characterTransform;

        [SerializeField]
        private Transform _destination;

        [SerializeField]
        private CharacterAnimationController _characterAnimationController;

        [SerializeField]
        private FloatVariable _movementSpeed;

        [SerializeField]
        private float _rotationSpeed = 10f;

        [SerializeField]
        private float _stopDistance = 0.1f;

        private float _cachedMovementSpeed;
        
        private void Start()
        {
            _cachedMovementSpeed = _movementSpeed.Value;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            if (_destination == null)
            {
                return;
            }

            float distance = Vector3.Distance
            (
                _characterTransform.position,
                _destination.position
            );

            _characterAnimationController.SetState
            (
                distance > _stopDistance
                    ? CharactersState.Walking
                    : CharactersState.Idle
            );

            Vector3 direction = _destination.position - _characterTransform.position;
            direction.y = 0f;

            float distanceDirection = direction.magnitude;

            if (distanceDirection <= _stopDistance)
            {
                _characterTransform.rotation = Quaternion.Slerp
                (
                    _characterTransform.rotation,
                    _destination.rotation,
                    _rotationSpeed * Time.fixedDeltaTime
                );
                return;
            }

            direction.Normalize();

            _characterTransform.position +=
                direction * _cachedMovementSpeed * Time.fixedDeltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _characterTransform.rotation = Quaternion.Slerp
            (
                _characterTransform.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime
            );
        }

        public void SetDestination(Transform destination)
        {
            _destination = destination;
        }   
    }
}
