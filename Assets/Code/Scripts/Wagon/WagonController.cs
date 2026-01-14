using System;
using System.Collections.Generic;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Events;
using XaviGames.SaveSystem;

namespace XaviGames.Wagon
{
    public class WagonController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsUnlocked { get; private set; } = false;

        [Header("Scripts References")]
        [SerializeField]
        public WagonUpgradeController WagonUpgradeController { get; private set; }

        [SerializeField]
        public CapacityWagonController CapacityWagonController { get; private set; }

        [SerializeField]
        private VoidEventChannel _onWagonUnlockedEvent;

        [Header("Save System")]
        [SerializeField]
        private IntModel _wagonUnlockedModel;

        [SerializeField]
        private DataController _dataController;

        private void Start()
        {
            LoadData();
        }

        public void UnlockWagon()
        {
            if (IsUnlocked)
            {
                return;
            }

            IsUnlocked = true;
            _onWagonUnlockedEvent.RaiseEvent();
            SaveData();
        }

        private void LoadData()
        {
            IsUnlocked = _wagonUnlockedModel.Value > 0;
        }

        private void SaveData()
        {
            _wagonUnlockedModel.SetValue(IsUnlocked ? 1 : 0);
            _dataController.SaveModel(_wagonUnlockedModel);
        }
    }
}
