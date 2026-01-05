using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;

namespace XaviGames.Interaction
{
    public class FenceGateStateController : MonoBehaviour
    {
        [SerializeField]
        private bool _isOpen = false;

        [SerializeField]
        private FenceGateUnlockController _fenceGateUnlockController;

        [SerializeField]
        private float _openDuration = 2f;

        [SerializeField]
        [ReadOnly]
        private FenceState _fenceDoorState = FenceState.Idle;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private ButtonHoldAnimation _openFenceDoorAnimation;

        [SerializeField]
        private SpawnAnimation _openFenceSpawnAnimation;

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        public void TryOpen()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            if (_isOpen)
            {
                _openFenceDoorAnimation.FailureUnlocked();
                return;
            }

            _isOpen = true;
            SetState(FenceState.Opened);
        }

        public void TryClose()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            if (!_isOpen)
            {
                return;
            }

            Debug.Log("Closing Fence Gate");
            _isOpen = false;
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