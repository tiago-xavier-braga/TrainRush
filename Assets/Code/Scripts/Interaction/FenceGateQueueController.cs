using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using XaviGames.Attributes;
using XaviGames.Characters;

namespace XaviGames.Interaction
{
    public class FenceGateQueueController : MonoBehaviour
    {
        [SerializeField]
        private FenceGateUnlockController _fenceGateUnlockController;

        [SerializeField]
        private FenceGateStateController _fenceGateStateController;

        [SerializeField]
        private CharacterSpawnController _characterSpawnController;

        [SerializeField]
        private GroupBoardingQueueController _groupBoardingQueueController;

        [SerializeField]
        private int _intervalMoveCharacters = 1;

        [SerializeField]
        private List<CharacterQueuePosition> _characterQueuePositions = new List<CharacterQueuePosition>();

        private void Start()
        {
            StartCoroutine(ProcessQueueLoop());
        }

        public void ReleaseCharacterPosition()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            if (!_fenceGateStateController.IsOpen)
            {
                return;
            }

            if (!_groupBoardingQueueController.CanJoinQueue())
            {
                return;
            }

            CharacterQueuePosition position = _characterQueuePositions.First();

            if (!position.IsCharacterAtPosition())
            {
                return;
            }

            CharacterMovementController characterMovement = position.CharacterMovementController;
            position.ClearCharacter();
            _groupBoardingQueueController.RedirectEmptyPosition(characterMovement);
        }

        private IEnumerator ProcessQueueLoop()
        {
            yield return new WaitForSeconds(1f);
            FillQueue();

            while (true)
            {
                MoveCharactersInQueue();
                CheckEmptyPosition();
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

        [Button]
        private void FillQueue()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            foreach (var position in _characterQueuePositions)
            {
                GameObject characterObject = _characterSpawnController.ActivateCharacter();
                var characterManager = characterObject.GetComponent<CharacterManager>();
                CharacterMovementController characterMovementController = characterManager.CharacterMovementController;
                position.SetCharacter(characterMovementController);
                characterMovementController.SetDestination(position.Transform);
            }
        }

        private void CheckEmptyPosition()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }

            CharacterQueuePosition position = _characterQueuePositions.Last();
            if (position.CharacterMovementController == null)
            {
                GameObject characterObject = _characterSpawnController.ActivateCharacter();
                var characterManager = characterObject.GetComponent<CharacterManager>();
                CharacterMovementController characterMovementController = characterManager.CharacterMovementController;
                position.SetCharacter(characterMovementController);
                characterMovementController.SetDestination(position.Transform);
            }
        }
    }
}
