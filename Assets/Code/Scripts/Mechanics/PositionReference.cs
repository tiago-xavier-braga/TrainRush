using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Mechanics
{
    public class PositionReference : MonoBehaviour
    {
        [field: SerializeField]
        public bool IsFree { get; private set; }

        public UnityAction<bool> OnStatusChanged;

        public void SetFree(bool isFree)
        {
            IsFree = isFree;
            OnStatusChanged?.Invoke(IsFree);
        }
    }
}
