namespace XaviGames.SaveSystem
{
    public interface IDataStorage
    {
        public void SetInt(string key, int value);
        public int GetInt(string key, int defaultValue = 0);

        public void SetFloat(string key, float value);
        public float GetFloat(string key, float defaultValue = 0f);

        public void SetString(string key, string value);
        public string GetString(string key, string defaultValue = "");

        public bool HasKey(string key);
        public void DeleteKey(string key);

        public void Save();
    }
}
