using System.Collections.Generic;
using UnityEngine;

namespace XaviGames.Train
{
    [CreateAssetMenu(fileName = "TrainData", menuName = "XaviGames/Train/TrainData")]
    public class TrainData : ScriptableObject
    {
        [field: SerializeField]
        public int TrainOrder { get; private set; }

        [field: SerializeField]
        public GameObject TrainPrefab { get; private set; }

        [field: SerializeField]
        public float MinSpeed { get; private set; }

        [field: SerializeField]
        public float MaxSpeed { get; private set; }

        [field: SerializeField]
        public List<WagonData> InitialWagons { get; private set; } = new();
    }
}