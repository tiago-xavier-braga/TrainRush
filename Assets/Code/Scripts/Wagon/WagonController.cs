using UnityEngine;
using UnityEngine.Events;
using XaviGames.Attributes;
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
        private WagonSpawnController _wagonSpawnController;

        [Header("Save System")]
        [SerializeField]
        private IntModel _wagonUnlockedModel;

        [SerializeField]
        private DataController _dataController;

        public UnityAction<bool> OnWagonUnlocked;

        private void Start()
        {
            LoadData();

            if (IsUnlocked)
            {
                _wagonSpawnController.CreateWagon();
                OnWagonUnlocked?.Invoke(true);
            }
        }

        [Button(true)]
        public void UnlockWagon()
        {
            if (IsUnlocked)
            {
                return;
            }

            //TODO: Add unlock wagon effects... Sounds?
            IsUnlocked = true;
            _wagonSpawnController.CreateWagon();
            OnWagonUnlocked?.Invoke(true);
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
