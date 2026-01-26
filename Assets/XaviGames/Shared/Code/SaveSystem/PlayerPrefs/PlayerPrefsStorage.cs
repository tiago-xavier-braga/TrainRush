using UnityEngine;

namespace XaviGames.SaveSystem
{
    public class PlayerPrefsStorage : IDataStorage
    {
        public void DeleteKey(string key)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            PlayerPrefs.DeleteKey(key);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetFloat(key);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetInt(key);
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (!IsValidKey(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetString(key);
        }

        public bool HasKey(string key)
        {
            if (!IsValidKey(key))
            {
                return false;
            }

            return PlayerPrefs.HasKey(key);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void SetFloat(string key, float value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            PlayerPrefs.SetFloat(key, value);
        }

        public void SetInt(string key, int value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            PlayerPrefs.SetInt(key, value);
        }

        public void SetString(string key, string value)
        {
            if (!IsValidKey(key))
            {
                return;
            }

            PlayerPrefs.SetString(key, value);
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
