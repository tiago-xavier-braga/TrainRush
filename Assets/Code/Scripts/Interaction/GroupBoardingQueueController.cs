using System.Collections.Generic;
using UnityEngine;
using XaviGames.Characters;

namespace XaviGames.Interaction
{
    public class GroupBoardingQueueController : MonoBehaviour
    {
        [SerializeField]
        private List<BoardingQueueController> _boardingQueueControllers = new List<BoardingQueueController>();

        public bool CanJoinQueue()
        {
            return _boardingQueueControllers.Exists(queueController => queueController.HasEmptyPosition());
        }

        public void RedirectEmptyPosition(CharacterMovementController characterMovement)
        {
            foreach (BoardingQueueController queueController in _boardingQueueControllers)
            {
                CharacterQueuePosition emptyPosition = queueController.GetLastEmptyPosition();

                if (emptyPosition != null)
                {
                    characterMovement.SetDestination(emptyPosition.Transform);
                    emptyPosition.SetCharacter(characterMovement);
                    return;
                }
            }
        }
    }
}