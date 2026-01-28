using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Characters;
using XaviGames.Events;
using XaviGames.Train;
using XaviGames.Wagon;

namespace XaviGames.Interaction
{
    public class WagonQueueController : MonoBehaviour
    {
        [SerializeField]
        private WagonController _wagonController;

        [SerializeField]
        private CapacityWagonController _capacityWagonController;

        [SerializeField]
        private CharacterSpawnController _characterSpawnController;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private List<QueuePosition> _queuePositions = new List<QueuePosition>();

        private TrainState _trainState = TrainState.Idle;

        private void OnEnable()
        {
            _onTrainStateChanged.Subscribe(TrainStateChanged);
        }

        private void OnDisable()
        {
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
        }

        private void FixedUpdate()
        {
            if (!_wagonController.IsUnlocked)
            {
                return;
            }

            OrganizeQueue();

            if (_trainState != TrainState.WaitingForSignal)
            {
                return;
            }

            if (!IsWagonEmpty())
            {
                return;
            }

            QueuePosition firstPosition = _queuePositions.First();

            if (firstPosition.IsOccupied && firstPosition.IsEntityAtPosition())
            {
                GameObject character = firstPosition.OccupiedEntity;
                _characterSpawnController.DisableCharacter(character);
                _capacityWagonController.OccupySeat();
                firstPosition.ClearOccupiedEntity();
                return;
            }
        }

        public bool IsWagonEmpty()
        {
            if (!_wagonController.IsUnlocked)
            {
                return false;
            }

            return _capacityWagonController.Capacity > _capacityWagonController.CurrentBoarded;
        }

        public bool IsLastPositionEmpty()
        {
            if (!_wagonController.IsUnlocked)
            {
                return false;
            }

            QueuePosition emptyPosition = _queuePositions.Last();

            return !emptyPosition.IsOccupied;
        }

        public void AddCharacter(GameObject character)
        {
            QueuePosition lastPosition = _queuePositions.Last();
            CharacterManager characterManager = character.GetComponent<CharacterManager>();
            characterManager.CharacterMovementController.SetDestination(lastPosition.transform);
            lastPosition.SetOccupiedEntity(character);
        }

        private void OrganizeQueue()
        {
            for (int i = 0; i < _queuePositions.Count - 1; i++)
            {
                QueuePosition queuePosition = _queuePositions[i];
                QueuePosition nextQueuePosition = _queuePositions[Mathf.Clamp(i + 1, 0, _queuePositions.Count - 1)];

                if (!queuePosition.IsOccupied && nextQueuePosition.IsOccupied)
                {
                    CharacterManager characterManager = nextQueuePosition.OccupiedEntity.GetComponent<CharacterManager>();
                    characterManager.CharacterMovementController.SetDestination(queuePosition.transform);
                    queuePosition.SetOccupiedEntity(nextQueuePosition.OccupiedEntity);
                    nextQueuePosition.SetOccupiedEntity(null);
                }
            }
        }

        private void TrainStateChanged(object state)
        {
            _trainState = (TrainState)state;
        }
    }
}
