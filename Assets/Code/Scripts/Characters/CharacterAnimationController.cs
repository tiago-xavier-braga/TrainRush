using XaviGames.Attributes;

namespace XaviGames.Characters
{
    using UnityEngine;

    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        public CharactersState CurrentState { get; private set; }

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        private void Start()
        {
            UpdateAnimatorState();
        }

        public void SetState(CharactersState newState)
        {
            if (CurrentState != newState)
            {
                CurrentState = newState;
                UpdateAnimatorState();
            }
        }

        [Button]
        private void UpdateAnimatorState()
        {
            _animator.SetInteger(StateParameterHash, (int)CurrentState);
        }
    }

}

