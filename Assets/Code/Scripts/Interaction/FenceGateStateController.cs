using System.Collections;
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
        private GameObject _openFenceDoorMarking;

        [SerializeField]
        private ButtonHoldController _openFenceDoorController;

        [SerializeField]
        private ButtonHoldAnimation _openFenceDoorAnimation;

        [SerializeField]
        private SpawnAnimation _openFenceSpawnAnimation;

        private static readonly int StateParameterHash = Animator.StringToHash("State");


        private void OnEnable()
        {
            _fenceGateUnlockController.OnUnlocked += ShowOpenMarking;
        }

        private void OnDisable()
        {
            _fenceGateUnlockController.OnUnlocked -= ShowOpenMarking;
        }

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

            _openFenceDoorAnimation.SuccessUnlocked();
            SetState(FenceState.Opened);
        }

        private void ShowOpenMarking()
        {
            _openFenceDoorMarking.SetActive(true);

            _openFenceSpawnAnimation.Animate
            (
                _openFenceDoorMarking,
                _openFenceDoorMarking.transform.localScale,
                new Vector3(0.5f, 0.5f, 0.5f)
            );
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