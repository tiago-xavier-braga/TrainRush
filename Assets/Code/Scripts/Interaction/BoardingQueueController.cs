using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;

namespace XaviGames.Interaction
{
    public class BoardingQueueController : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterQueuePosition> _characterQueuePositions = new List<CharacterQueuePosition>();

        [SerializeField]
        private int _intervalMoveCharacters = 1;

        private void Start()
        {
            StartCoroutine(ProcessQueueLoop());
        }

        public bool HasEmptyPosition()
        {
            return _characterQueuePositions.Any(position => position.CharacterMovementController == null);
        }

        public CharacterQueuePosition GetLastEmptyPosition()
        {
            CharacterQueuePosition position = _characterQueuePositions.Last();
            if (position.CharacterMovementController == null)
            {
                return position;
            }

            return null;
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
