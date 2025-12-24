using UnityEditor.Overlays;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Audio;
using XaviGames.EconomySystem;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;
using XaviGames.UnlockSystem;

namespace XaviGames.GameMechanics
{
    public class FenceDoorRotate : MonoBehaviour
    {
        [Header("Price Settings")]
        [SerializeField]
        private int _price;

        [Header("Unlock Refereneces")]
        [field: SerializeField]
        public bool IsUnlocked { get; private set; } = false;

        [Header("Unlock Floor Marking")]
        [SerializeField]
        private GameObject _unlockMarking;

        [SerializeField]
        private UnlockController _unlockController;

        [SerializeField]
        private SpawnAnimation _unlockSpawnAnimation;

        [Header("Open Fence Floor Marking")]
        [SerializeField]
        private GameObject _openFenceDoorMarking;

        [SerializeField]
        private UnlockController _openFenceDoorController;

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

        private void Start()
        {
            LoadData();
            _unlockController.SetNewPrice(_price.ToString());
        }

        private void Update()
        {
            _unlockController.EnableAnimation(_playerCoinsModel.Value < _price);
        }

        public void TryUnlock()
        {
            if (IsUnlocked)
            {
                return;
            }

            if (_playerCoinsModel.Value < _price)
            {
                _unlockController.FailureUnlocked();
                return;
            }

            _economyController.RemoveCoins(_price);
            _unlockController.SuccessUnlocked();

            IsUnlocked = true;

            SaveData();
            HideUnlockMarking();
            ShowOpenMarking();
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

        private void LoadData()
        {             
            int fenceDoorState = 0;
            fenceDoorState = _fenceDoorRotateModel.Value;

            if (fenceDoorState == 1)
            {
                IsUnlocked = true;
                _unlockMarking.SetActive(false);
                _openFenceDoorMarking.SetActive(true);
            }
        }

        private void SaveData()
        {
            _fenceDoorRotateModel.SetValue(1);
            _dataController.SaveModel(_fenceDoorRotateModel);
        }
    }
}