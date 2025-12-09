using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Audio;
using XaviGames.Events;
using XaviGames.Managers;

namespace XaviGames.Train
{
    public class TrainMovementController : MonoBehaviour
    {
        [Header("Scripts References")]
        [SerializeField]
        [ReadOnly]
        private TrainState _trainState = TrainState.Idle;

        [SerializeField]
        private TrainUpgradeController _trainUpgradeController;

        [SerializeField]
        [ReadOnly]
        private float _positionProduct = 0; 

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Running)
            {
                return;
            }

            Move();
        }


        public void SetTrainState(TrainState trainState)
        {
            _trainState = trainState; 
        }


        private void Move()
        {
            if (_trainState != TrainState.Moving)
            {
                return;
            }

            Vector3 direction = transform.forward * _trainUpgradeController.Speed * Time.fixedDeltaTime;
            transform.Translate(direction, Space.World);

            _positionProduct = Vector3.Dot(transform.position, transform.forward);
        }
    }
}