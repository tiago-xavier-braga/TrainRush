using System;
using System.Collections.Generic;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.SaveSystem;

namespace XaviGames.Wagon
{
    public class WagonController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsUnlocked { get; private set; } = false;

        [Header("Wagons")]
        [field: SerializeField]
        [field: ReadOnly]
        public WagonData WagonData { get; private set; } = null;

        [SerializeField]
        private List<WagonData> _availableWagons = new();

        [Header("Scripts References")]
        [SerializeField]
        public WagonUpgradeController WagonUpgradeController { get; private set; }

        [SerializeField]
        public CapacityWagonController CapacityWagonController { get; private set; }

        [Header("Save System")]
        [SerializeField]
        private IntModel _wagonUnlockedModel;

        [SerializeField]
        private IntModel _wagonOrderSaveModel;

        [SerializeField]
        private DataController _dataController;

        private void Start()
        {
            LoadData();

            if (IsUnlocked)
            {
                CreateWagonModel();
            }
        }

        //TODO: Call this method from an external script when the wagon is unlocked for the first time
        //TODO: Maybe move the unlocking logic to WagonUpdateController
        private void CreateWagonModel()
        {
            WagonUpgradeController.CreateWagonModel(WagonData);
        }

        public void UpgradeWagon()
        {
            if (WagonData.WagonOrder >= _availableWagons.Count - 1)
            {
                return;
            }

            WagonData = _availableWagons.Find(wd => wd.WagonOrder == WagonData.WagonOrder + 1);

            SaveData();
            CreateWagonModel();
        }

        private void LoadData()
        {
            IsUnlocked = _wagonUnlockedModel.Value > 0;
            
            int wagonOrder = _wagonOrderSaveModel.Value;
            WagonData = _availableWagons.Find(wd => wd.WagonOrder == wagonOrder);
        }

        private void SaveData()
        {
            _wagonUnlockedModel.SetValue(IsUnlocked ? 1 : 0);
            _wagonOrderSaveModel.SetValue(WagonData.WagonOrder);
            _dataController.SaveModel(_wagonOrderSaveModel);
            _dataController.SaveModel(_wagonUnlockedModel);
        }
    }
}
