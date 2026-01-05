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
