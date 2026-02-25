using UnityEngine;

namespace XaviGames.SaveSystem
{
    [CreateAssetMenu(fileName = "CrazyGamesStorageSO", menuName = "XaviGames/SaveSystem/CrazyGamesStorageSO", order = 1)]
    public class CrazyGamesStorageSO : DataStorageSO
    {
        public override IDataStorage Create()
        {
            return new CrazyGamesStorage();
        }
    }
}
