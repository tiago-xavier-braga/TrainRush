using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.SaveSystem
{
    public class DataControllerBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private DataController _dataController;

        [Button]
        private void SaveAllModels()
        {
            _dataController.SaveAllModels();
        }

        [Button]
        private void LoadAllModels()
        {
            _dataController.LoadAllModels();
        }

        [Button]
        private void DeleteAllModels()
        {
            _dataController.DeleteAllModels();
        }
#endif
    }
}