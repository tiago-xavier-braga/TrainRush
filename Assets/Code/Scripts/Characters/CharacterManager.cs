using UnityEngine;
using XaviGames.Animation;

namespace XaviGames.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        [field: SerializeField]
        public CharacterAnimationController CharacterAnimationController { get; private set; }

        [field: SerializeField]
        public CharacterMovementController CharacterMovementController { get; private set; }

        [field: SerializeField]
        public SpawnAnimation SpawnAnimation { get; private set; }
    }
}
