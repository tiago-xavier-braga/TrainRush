using UnityEngine;

namespace XaviGames.Wagon
{
    [CreateAssetMenu(fileName = "WagonData", menuName = "XaviGames/Wagon/WagonData")]
    public class WagonData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject WagonPrefab { get; private set; }

        [field: SerializeField]
        public int TierUpgrade { get; private set; }
    }
}
