using UnityEngine;

namespace XaviGames.Wagon
{
    [CreateAssetMenu(fileName = "WagonData", menuName = "XaviGames/Wagon/WagonData")]
    public class WagonData : ScriptableObject
    {
        [field: SerializeField]
        public int WagonOrder { get; private set; }

        [field: SerializeField]
        public GameObject WagonPrefab { get; private set; }

        [field: SerializeField]
        public int MinCapacity { get; private set; }

        [field: SerializeField]
        public int MaxCapacity { get; private set; }
    }
}
