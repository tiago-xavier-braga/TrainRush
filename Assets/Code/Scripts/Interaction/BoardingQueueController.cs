using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;
using XaviGames.Events;
using XaviGames.Train;
using XaviGames.Wagon;

namespace XaviGames.Interaction
{
    public class BoardingQueueController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsUnlocked { get; private set; } = false;

        [SerializeField]
        private WagonController _wagonController;

        [SerializeField]
        private CapacityWagonController _capacityWagonController;

        [SerializeField]
        private SingleEventChannel _onTrainStateChanged;

        [SerializeField]
        private int _intervalMoveCharacters = 1;
        
        [SerializeField]
        private List<CharacterQueuePosition> _characterQueuePositions = new List<CharacterQueuePosition>();

        private TrainState _trainState = TrainState.Idle;

        private void OnEnable()
        {
            _wagonController.OnWagonUnlocked += HandleWagonUnlocked;
            _onTrainStateChanged.Subscribe(TrainStateChanged);
        }

        private void OnDisable()
        {
            _wagonController.OnWagonUnlocked -= HandleWagonUnlocked;
            _onTrainStateChanged.Unsubscribe(TrainStateChanged);
        }

        private void Start()
        {
            StartCoroutine(ProcessQueueLoop());
        }

        private void FixedUpdate()
        {
            if (_trainState != TrainState.WaitingForSignal)
            {
                return;
            }

            _capacityWagonController.OccupySeat();
            ReleaseCharacterPosition();
        }

        public bool HasEmptyPosition()
        {
            if (!IsUnlocked)
            {
                return false;
            }

            return _characterQueuePositions.Last().CharacterMovementController == null;
        }

        public CharacterQueuePosition GetLastEmptyPosition()
        {
            if (!IsUnlocked)
            {
                return null;
            }

            CharacterQueuePosition position = _characterQueuePositions.Last();
            if (position.CharacterMovementController == null)
            {
                return position;
            }

            return null;
        }

        private void ReleaseCharacterPosition()
        {
            CharacterQueuePosition position = _characterQueuePositions.First();

            if (!position.IsCharacterAtPosition())
            {
                return;
            }

            CharacterMovementController characterMovement = position.CharacterMovementController;
            position.ClearCharacter();
        }

        private void HandleWagonUnlocked(bool isUnlocked)
        {
            IsUnlocked = isUnlocked;
        }

        private void TrainStateChanged(object state)
        {
            if (state is TrainState trainState)
            {
                _trainState = trainState;
            }
        }

        private IEnumerator ProcessQueueLoop()
        {
            yield return new WaitForSeconds(1f);
            
            while (true)
            {
                MoveCharactersInQueue();
                yield return new WaitForSeconds(_intervalMoveCharacters);
            }
        }

        private void MoveCharactersInQueue()
        {
            for (int i = _characterQueuePositions.Count - 1; i > 0; i--)
            {
                CharacterQueuePosition currentPosition = _characterQueuePositions[i];
                CharacterQueuePosition previousPosition = _characterQueuePositions[i - 1];

                if (currentPosition.CharacterMovementController != null &&
                    previousPosition.CharacterMovementController == null)
                {
                    CharacterMovementController characterToMove = currentPosition.CharacterMovementController;
                    previousPosition.SetCharacter(characterToMove);
                    characterToMove.SetDestination(previousPosition.Transform);
                    currentPosition.ClearCharacter();
                }
            }
        }
    }
}
