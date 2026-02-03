using System.Collections;
using UnityEngine;
using XaviGames.Events;
using XaviGames.ObjectVariables;

namespace XaviGames.Train
{
    public class TrainRespawnController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private FloatVariable _speedRespawnValue = null;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        private Coroutine _trainRespawnCoroutine = null;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(TrainStateChanged);
        }

        private void OnDisable()
        {
            if (_trainRespawnCoroutine != null)
            {
                StopCoroutine(_trainRespawnCoroutine);
            }

            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
        }

        private void Start()
        {
            _trainMovementController.Approaching();
        }

        private void TrainStateChanged(object state)
        {
            TrainState trainState = (TrainState)state;

            if (trainState != TrainState.Finalized)
            {
                return;
            }

            if (_trainRespawnCoroutine != null)
            {
                StopCoroutine(_trainRespawnCoroutine);
            }

            _trainRespawnCoroutine = StartCoroutine(ResetTrain());
        }

        private IEnumerator ResetTrain()
        {
            yield return new WaitForSeconds(_speedRespawnValue.Value);
            _trainMovementController.Approaching();
        }
    }
}