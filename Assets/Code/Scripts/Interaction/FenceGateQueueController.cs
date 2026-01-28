using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Characters;

namespace XaviGames.Interaction
{
    public class FenceGateQueueController : MonoBehaviour
    {
        [SerializeField]
        private FenceGateUnlockController _fenceGateUnlockController;

        [SerializeField]
        private CharacterSpawnController _characterSpawnController;

        [SerializeField]
        private BoardingQueueController _boardingQueueController;

        [SerializeField]
        private List<QueuePosition> _queuePositions = new List<QueuePosition>();

        private void FixedUpdate()
        {
            if (!_fenceGateUnlockController.IsUnlocked)
            {
                return;
            }


            CheckLastPosition();
            OrganizeQueue();
        }

        public void ReleaseCharacter()
        {
            QueuePosition firstPosition = _queuePositions.First();
            
            if (!firstPosition.IsOccupied)
            {
                return;
            }
            
            if (!firstPosition.IsEntityAtPosition())
            {
                return;
            }

            _boardingQueueController.AddCharacter(firstPosition.OccupiedEntity);
            firstPosition.ClearOccupiedEntity();
            OrganizeQueue();
        }

        private void CheckLastPosition()
        {
            QueuePosition lastPosition = _queuePositions.Last();

            if (lastPosition.IsOccupied)
            {
                return;
            }

            GameObject characterSpawn = _characterSpawnController.ActivateCharacter();
            CharacterManager characterManager = characterSpawn.GetComponent<CharacterManager>();
            characterManager.CharacterMovementController.SetDestination(lastPosition.transform);
            lastPosition.SetOccupiedEntity(characterSpawn);
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
    }
}
