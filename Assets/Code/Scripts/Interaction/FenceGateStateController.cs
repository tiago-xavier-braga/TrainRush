using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Interaction
{
    public class FenceGateStateController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsOpen { get; private set; } = false;

        [SerializeField]
        private FenceGateUnlockController _fenceGateUnlockController;

        [SerializeField]
        [ReadOnly]
        private FenceState _fenceDoorState = FenceState.Idle;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private ButtonHoldAnimation _buttonHoldAnimation;

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        public void TryOpen()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            if (IsOpen)
            {
                _buttonHoldAnimation.FailureUnlocked();
                return;
            }

            IsOpen = true;
            SetState(FenceState.Opened);
        }

        public void TryClose()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
            SetState(FenceState.Closed);
        }

        private void SetState(FenceState newState)
        {
            if (_fenceDoorState != newState)
            {
                _fenceDoorState = newState;

                _animator.SetInteger(StateParameterHash, (int)_fenceDoorState);
            }
        }
    }
}