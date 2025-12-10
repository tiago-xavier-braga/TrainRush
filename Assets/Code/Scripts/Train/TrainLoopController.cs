using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Train
{
    public class TrainLoopController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _trainMovementController;

        [Button]
        public void StartLoop()
        {
        }
    }
}
