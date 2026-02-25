using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "PlayerPrefsStorageSO", menuName = "XaviGames/SaveSystem/PlayerPrefsStorageSO", order = 1)]
    public class PlayerPrefsStorageSO : DataStorageSO
    {
        public override IDataStorage Create()
        {
            return new PlayerPrefsStorage();
        }
    }
}
