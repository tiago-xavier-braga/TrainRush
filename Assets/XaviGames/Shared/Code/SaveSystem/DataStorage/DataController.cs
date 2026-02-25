using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "DataController", menuName = "XaviGames/SaveSystem/DataController")]
    public class DataController : ScriptableObject
    {
        [SerializeField]
        private List<Model> _models = new List<Model>();

        [SerializeField]
        private List<DataStorageSO> _dataStorages = new List<DataStorageSO>();

        public void SaveAllModels()
        {
            foreach (DataStorageSO dataStorage in _dataStorages)
            {
                IDataStorage saveStorage = dataStorage.Create();
                foreach (var model in _models)
                {
                    model.Save(saveStorage);
                }

                saveStorage.Save();
            }

        }

        public void LoadAllModels()
        {
            DataStorageSO dataStorage = _dataStorages.First();

            IDataStorage loadStorage = dataStorage.Create();
            foreach (var model in _models)
            {
                model.Load(loadStorage);
            }
        }

#if UNITY_EDITOR
        public void DeleteAllModels()
        {
            foreach (DataStorageSO dataStorage in _dataStorages)
            {
                IDataStorage deleteStorage = dataStorage.Create();
                foreach (var model in _models)
                {
                    deleteStorage.DeleteKey(model.Key);
                }

                deleteStorage.Save();
            }
        }
#endif

        public void SaveModel(Model model)
        {
            foreach (DataStorageSO dataStorage in _dataStorages)
            {
                IDataStorage saveStorage = dataStorage.Create();
                model.Save(saveStorage);
                saveStorage.Save();
            }
        }
    }
}

