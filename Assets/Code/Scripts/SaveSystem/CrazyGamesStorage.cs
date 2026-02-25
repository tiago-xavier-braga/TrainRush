using CrazyGames;
using UnityEngine;

namespace XaviGames.SaveSystem
{
    public class CrazyGamesStorage : IDataStorage
    {
        public void DeleteKey(string key)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            CrazySDK.Data.DeleteKey(key);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return CrazySDK.Data.GetFloat(key);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return CrazySDK.Data.GetInt(key);
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return CrazySDK.Data.GetString(key);
        }

        public bool HasKey(string key)
        {
            if (!IsValidKey(key))
            {
                return false;
            }

            return CrazySDK.Data.HasKey(key);
        }

        public void Save()
        {
            
        }

        public void SetFloat(string key, float value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            CrazySDK.Data.SetFloat(key, value);
        }

        public void SetInt(string key, int value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            CrazySDK.Data.SetInt(key, value);
        }

        public void SetString(string key, string value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            CrazySDK.Data.SetString(key, value);
        }

        private bool IsValidKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError($"The key provided does not exist.");
                return false;
            }

            return true;
        }
    }
}
