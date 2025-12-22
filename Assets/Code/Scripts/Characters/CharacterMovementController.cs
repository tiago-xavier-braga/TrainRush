using UnityEngine;
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


        private void Update()
        {
            if (_destination == null)
            {
                return;
            }

            float distance = Vector3.Distance(
                _characterTransform.position,
                _destination.position
            );

            _characterAnimationController.SetState(
                distance > _stopDistance
                    ? CharactersState.Walking
                    : CharactersState.Idle
            );
        }

        private void FixedUpdate()
        {
            if (_destination == null)
            {
                return;
            }

            Vector3 direction = _destination.position - _characterTransform.position;
            direction.y = 0f;

            float distance = direction.magnitude;

            if (distance <= _stopDistance)
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
                direction * _movementSpeed.Value * Time.fixedDeltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _characterTransform.rotation = Quaternion.Slerp    
            (
                _characterTransform.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}
