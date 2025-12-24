using UnityEngine;

namespace XaviGames.SaveSystem
{
    public abstract class DataStorageSO : ScriptableObject
    {
        public abstract IDataStorage Create();
    }
}
