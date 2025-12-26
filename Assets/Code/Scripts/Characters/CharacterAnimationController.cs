using XaviGames.Attributes;

namespace XaviGames.Characters
{
    using UnityEngine;

    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private CharactersState _currentState;

        private static readonly int StateParameterHash = Animator.StringToHash("State");

        private void Start()
        {
            UpdateAnimatorState();
        }

        public void SetState(CharactersState newState)
        {
            if (_currentState != newState)
            {
                _currentState = newState;
                UpdateAnimatorState();
            }
        }

        [Button]
        private void UpdateAnimatorState()
        {
            _animator.SetInteger(StateParameterHash, (int)_currentState);
        }
    }

}

