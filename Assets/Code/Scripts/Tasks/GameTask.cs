using UnityEngine;

namespace XaviGames.Tasks
{
    public class GameTask : MonoBehaviour
    {
        [field: SerializeField]
        public Transform TargetTransform { get; private set; } = null;

        [field: SerializeField]
        public bool IsAvailable { get; protected set; } = false;

        public void SetTaskAvailable(bool available)
        {
            IsAvailable = available;
        }
    }
}