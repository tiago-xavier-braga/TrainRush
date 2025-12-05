using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Train
{
    public class TrainLoopController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _movementController;

        [SerializeField]
        [ReadOnly]
        private bool _isEnabled = false;

        [Button]
        public void StartLoop()
        {
            _isEnabled = true;
            _movementController.SetTrainState(TrainState.Moving);
        }
    }
}
