using XaviGames.Attributes;

namespace XaviGames.Characters
{
    using UnityEngine;

    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private CharactersState currentState;

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        private void Start()
        {
            UpdateAnimatorState();
        }

        public void SetState(CharactersState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                UpdateAnimatorState();
            }
        }

        [Button]
        private void UpdateAnimatorState()
        {
            animator.SetInteger(StateParameterHash, (int)currentState);
        }
    }

}

