using UnityEngine;

namespace XaviGames.Train
{
    [CreateAssetMenu(fileName = "WagonData", menuName = "XaviGames/Train/WagonData")]
    public class WagonData : ScriptableObject
    {
        [field: SerializeField]
        public int WagonOrder { get; private set; }

        [field: SerializeField]
        public GameObject WagonPrefab { get; private set; }

        [field: SerializeField]
        public int MaxUpdateCapacity { get; private set; }
    }
}
