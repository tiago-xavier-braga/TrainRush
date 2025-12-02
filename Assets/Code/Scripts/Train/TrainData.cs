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
        public float MaxUpdateSpeed { get; private set; }
    }
}