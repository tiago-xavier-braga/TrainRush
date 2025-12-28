using System.Collections;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.EconomySystem;
using XaviGames.SaveSystem;

namespace XaviGames.Interaction
{
    public class FenceGateController : MonoBehaviour
    {
        private enum FenceState
        {
            Idle = 0,
            Opened = 1,
            Closed = 2,
        }

        [Header("Price Settings")]
        [SerializeField]
        private int _price;

        [Header("Unlock Refereneces")]
        [SerializeField]
        private bool IsUnlocked = false;

        [Header("Opening and Closing Mechanism")]
        [SerializeField]
        private bool _canOpen = false;

        [SerializeField]
        private float _openDuration = 2f;

        [SerializeField]
        [ReadOnly]
        private FenceState _fenceDoorState = FenceState.Idle;

        [SerializeField]
        private Animator _animator;

        [Header("Unlock Floor Marking")]
        [SerializeField]
        private GameObject _unlockMarking;

        [SerializeField]
        private ButtonHoldController _unlockController;

        [SerializeField]
        private ButtonHoldAnimation _unlockAnimation;

        [SerializeField]
        private SpawnAnimation _unlockSpawnAnimation;

        [Header("Open Fence Floor Marking")]
        [SerializeField]
        private GameObject _openFenceDoorMarking;

        [SerializeField]
        private ButtonHoldController _openFenceDoorController;

        [SerializeField]
        private ButtonHoldAnimation _openFenceDoorAnimation;

        [SerializeField]
        private SpawnAnimation _openFenceSpawnAnimation;

        [Header("Economy References")]
        [SerializeField]
        private IntModel _playerCoinsModel;

        [SerializeField]
        private EconomyController _economyController;

        [Header("Save System")]
        [SerializeField]
        private IntModel _fenceDoorRotateModel;

        [SerializeField]
        private DataController _dataController;

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        private void Start()
        {
            LoadData();

            if (_unlockController != null)
            {
                _unlockController.SetNewText(_price.ToString());
            }
        }

        private void Update()
        {
            bool canAfford = _playerCoinsModel != null && _playerCoinsModel.Value >= _price;

            if (_unlockAnimation != null)
            {
                _unlockAnimation.EnableAnimation(canAfford);
            }
        }

        public void TryUnlock()
        {
            if (IsUnlocked)
            {
                return;
            }

            if (_playerCoinsModel == null)
            {
                return;
            }

            if (_playerCoinsModel.Value < _price)
            {
                if (_unlockAnimation != null)
                {
                    _unlockAnimation.FailureUnlocked();
                }

                return;
            }

            if (_economyController != null)
            {
                _economyController.RemoveCoins(_price);
            }

            if (_unlockAnimation != null)
            {
                _unlockAnimation.SuccessUnlocked();
            }

            IsUnlocked = true;

            SaveData();
            HideUnlockMarking();
            ShowOpenMarking();
        }

        public void TryOpen()
        {
            if (!IsUnlocked)
            {
                return;
            }

            if (!_canOpen)
            {
                if (_openFenceDoorAnimation != null)
                {
                    _openFenceDoorAnimation.FailureUnlocked();
                }

                return;
            }

            if (_openFenceDoorAnimation != null)
            {
                _openFenceDoorAnimation.SuccessUnlocked();
            }

            StartCoroutine(OpenDoorCoroutine());
        }

        private void HideUnlockMarking()
        {
            if (_unlockSpawnAnimation == null)
            {
                if (_unlockMarking != null)
                {
                    _unlockMarking.SetActive(false);
                }

                return;
            }

            if (_unlockMarking == null)
            {
                return;
            }

            _unlockSpawnAnimation.Animate
            (
                _unlockMarking,
                _unlockMarking.transform.localScale,
                Vector3.zero,
                () => _unlockMarking.SetActive(false)
            );
        }

        private void ShowOpenMarking()
        {
            if (_openFenceDoorMarking == null)
            {
                return;
            }

            _openFenceDoorMarking.SetActive(true);

            if (_openFenceSpawnAnimation == null)
            {
                _openFenceDoorMarking.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                return;
            }

            _openFenceSpawnAnimation.Animate
            (
                _openFenceDoorMarking,
                _openFenceDoorMarking.transform.localScale,
                new Vector3(0.5f, 0.5f, 0.5f)
            );
        }

        private void LoadData()
        {
            if (_fenceDoorRotateModel == null)
            {
                return;
            }

            int fenceDoorState = _fenceDoorRotateModel.Value;

            if (fenceDoorState == 1)
            {
                IsUnlocked = true;

                if (_unlockMarking != null)
                {
                    _unlockMarking.SetActive(false);
                }

                if (_openFenceDoorMarking != null)
                {
                    _openFenceDoorMarking.SetActive(true);
                }
            }
        }

        private void SaveData()
        {
            if (_fenceDoorRotateModel == null)
            {
                return;
            }

            _fenceDoorRotateModel.SetValue(1);

            if (_dataController != null)
            {
                _dataController.SaveModel(_fenceDoorRotateModel);
            }
        }

        private void SetState(FenceState newState)
        {
            if (_fenceDoorState != newState)
            {
                _fenceDoorState = newState;

                if (_animator != null)
                {
                    _animator.SetInteger(StateParameterHash, (int)_fenceDoorState);
                }
            }
        }

        private IEnumerator OpenDoorCoroutine()
        {
            _canOpen = false;

            SetState(FenceState.Opened);

            yield return new WaitForSeconds(_openDuration);

            SetState(FenceState.Closed);

            _canOpen = true;
        }
    }
}
