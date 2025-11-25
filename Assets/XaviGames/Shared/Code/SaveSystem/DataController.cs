using System.Collections.Generic;
using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "DataController", menuName = "XaviGames/SaveSystem/DataController")]
    public class DataController : ScriptableObject
    {
        [SerializeField]
        private List<Model> _models = new List<Model>();

        public void Save()
        {
            foreach (var model in _models)
            {
                switch (model.Value)
                {
                    case int intValue:
                        PlayerPrefs.SetInt(model.Key, intValue);
                        break;
                    case float floatValue:
                        PlayerPrefs.SetFloat(model.Key, floatValue);
                        break;
                    case string stringValue:
                        PlayerPrefs.SetString(model.Key, stringValue);
                        break;
                    default:
                        Debug.LogError($"Unsupported data type for key: {model.Key}");
                        break;
                }
            }

            PlayerPrefs.Save();
        }

        public void Load()
        {
            foreach (var model in _models)
            {
                switch (model.Type)
                {
                    case DataType.Int:
                        model.Value = PlayerPrefs.GetInt(model.Key, 0);
                        break;
                    case DataType.Float:
                        model.Value = PlayerPrefs.GetFloat(model.Key, 0f);
                        break;
                    case DataType.String:
                        model.Value = PlayerPrefs.GetString(model.Key, string.Empty);
                        break;
                    default:
                        Debug.LogError($"Unsupported data type for key: {model.Key}");
                        break;
                }
            }
        }
    }
}

