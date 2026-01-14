using System.Collections;
using UnityEngine;
using XaviGames.Events;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;

namespace XaviGames.Train
{
    public class TrainRespawnController : MonoBehaviour
    {
        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private FloatVariable _speedRespawnValue = null;

        [SerializeField]
        private VoidEventChannel _routeCompletedEventChannel;

        private Coroutine _trainRespawnCoroutine = null;

        private void OnEnable()
        {
            _routeCompletedEventChannel.Subscribe(HandleRouteCompleted);
        }

        private void OnDisable()
        {
            StopCoroutine(_trainRespawnCoroutine);
            _routeCompletedEventChannel.Unsubscribe(HandleRouteCompleted);
        }

        private void Start()
        {
            _trainMovementController.Approaching();
        }

        private void HandleRouteCompleted()
        {
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