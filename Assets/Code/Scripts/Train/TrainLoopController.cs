using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Train
{
    public class TrainLoopController : MonoBehaviour
    {
        [field: SerializeField]
        public TrafficLightController TrafficLightController { get; private set; }

        [Header("Movement References")]
        [field: SerializeField]
        public  Transform StartTransform { get; private set; }

        [field: SerializeField]
        public Transform StationTransform { get; private set; }

        [field: SerializeField]
        public Transform EndTransform { get; private set; }

        [Header("Temp")]
        [SerializeField]
        private TrainData _trainData;

        [Button]
        public void Initialize()
        {
            GameObject trainInstance = Instantiate(_trainData.TrainPrefab, StartTransform.position, Quaternion.identity, transform);
            trainInstance.GetComponent<TrainController>().Initialize(this);
        }

    }
}
