using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Characters;

namespace XaviGames.Interaction
{
    public class CharacterQueuePosition : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        [field: SerializeField]
        [field: ReadOnly]
        public CharacterMovementController CharacterMovementController { get; private set; }

        [SerializeField]
        private float _positionTolerance = 0.1f;

        [Header("Debug")]
        [SerializeField]
        private float _gizmoSize = 0.5f;

        private void Start()
        {
            Transform = transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, _gizmoSize);
        }

        public bool IsCharacterAtPosition()
        {
            if (CharacterMovementController == null)
            {
                return false;
            }

            float distance = Vector3.Distance
            (
                CharacterMovementController.transform.position,
                transform.position
            );

            return distance < _positionTolerance;
        }

        public void SetCharacter(CharacterMovementController characterMovementController)
        {
            CharacterMovementController = characterMovementController;
        }

        [Button]
        public void ClearCharacter()
        {
            CharacterMovementController = null;
        }
    }
}
