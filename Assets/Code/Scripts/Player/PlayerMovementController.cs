using UnityEngine;
using UnityEngine.InputSystem;
using XaviGames.Attributes;
using XaviGames.Characters;
using XaviGames.ObjectVariables;

namespace XaviGames.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerTransform;

        [SerializeField]
        private Rigidbody _playerRigidbody;

        [SerializeField]
        private CharacterAnimationController _characterAnimationController;

        [SerializeField]
        private FloatVariable _movementSpeed;

        [SerializeField]
        private float _rotationSpeed = 10f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _movementInput;

        public void OnMovementValue(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (_movementInput != Vector2.zero)
            {
                _characterAnimationController.SetState(CharactersState.Walking);
            }
            else
            {
                _characterAnimationController.SetState(CharactersState.Idle);
            }
        }

        private void FixedUpdate()
        {
            Vector3 moveInput = new Vector3(_movementInput.x, 0f, _movementInput.y);
            moveInput *= _movementSpeed.Value * Time.fixedDeltaTime;
            Vector3 nextPosition = _playerTransform.position;
            nextPosition += moveInput;

            _playerRigidbody.MovePosition(nextPosition);

            if (moveInput == Vector3.zero)
            {
                return;
            }

            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            Quaternion nextRotation = Quaternion.Slerp(
                _playerTransform.rotation,
                targetRot,
                _rotationSpeed * Time.fixedDeltaTime
            );

            _playerRigidbody.MoveRotation(nextRotation);
        }
    }
}