using UnityEngine;
using UnityEngine.Events;
using XaviGames.Animation;
using XaviGames.EconomySystem;
using XaviGames.SaveSystem;

namespace XaviGames.Interaction
{
    public class FenceGateUnlockController : MonoBehaviour
    {
        [field: SerializeField]
        public bool IsUnlocked { get; private set; } = false;

        [SerializeField]
        private int _price;

        [SerializeField]
        private GameObject _unlockMarking;

        [SerializeField]
        private ButtonHoldController _unlockController;

        [SerializeField]
        private ButtonHoldAnimation _unlockAnimation;

        [SerializeField]
        private SpawnAnimation _unlockSpawnAnimation;

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

        public UnityAction OnUnlocked;

        private void Start()
        {
            LoadData();
            _unlockController.SetNewText(_price.ToString());
        }

        private void Update()
        {
            bool canAfford = _playerCoinsModel.Value >= _price;

            _unlockAnimation.EnableAnimation(canAfford);
        }

        public void TryUnlock()
        {
            if (IsUnlocked)
            {
                return;
            }

            if (_playerCoinsModel.Value < _price)
            {
                _unlockAnimation.FailureUnlocked();
                return;
            }

            _economyController.RemoveCoins(_price);
            _unlockAnimation.SuccessUnlocked();

            IsUnlocked = true;

            SaveData();
            HideUnlockMarking();
            OnUnlocked?.Invoke();
        }

        private void HideUnlockMarking()
        {
            _unlockSpawnAnimation.Animate
            (
                _unlockMarking,
                _unlockMarking.transform.localScale,
                Vector3.zero,
                () => _unlockMarking.SetActive(false)
            );
        }

        private void LoadData()
        {
            int fenceDoorState = _fenceDoorRotateModel.Value;

            if (fenceDoorState == 1)
            {
                IsUnlocked = true;
                _unlockMarking.SetActive(false);
                OnUnlocked?.Invoke();
            }
        }

        private void SaveData()
        {
            _fenceDoorRotateModel.SetValue(1);

            if (_dataController != null)
            {
                _dataController.SaveModel(_fenceDoorRotateModel);
            }
        }
    }
}
