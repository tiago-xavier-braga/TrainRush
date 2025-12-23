using System.Collections.Generic;
using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "DataController", menuName = "XaviGames/SaveSystem/DataController")]
    public class DataController : ScriptableObject
    {
        [SerializeField]
        private List<Model> _models = new List<Model>();
        
        private List<IDataStorage> _dataStorages = new List<IDataStorage>();

        public void SaveAllModels(IDataStorage storage)
        {
            foreach (var model in _models)
            {
                model.Save(storage);
            }

            storage.Save();
        }

        public void LoadAllModels(IDataStorage storage)
        {
            foreach (var model in _models)
            {
                model.Load(storage);
            }
        }
    }
}

