using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Events
{
    [CreateAssetMenu(fileName = "VoidEventChannel", menuName = "XaviGames/Events/VoidEventChannel")]
    public class VoidEventChannel : ScriptableObject
    {
        private UnityAction _event;

        public void Subscribe(UnityAction listener)
        {
            if (listener == null)
            {
                Debug.LogError($"Trying to subscribe a null listener to EventChannel {name}");
                return;
            }

            _event -= listener;
            _event += listener;
        }

        public void Unsubscribe(UnityAction listener)
        {
            if (listener == null)
            {
                Debug.LogError($"Trying to unsubscribe a null listener from EventChannel {name}");
                return;
            }

            _event -= listener;
        }

        public void RaiseEvent()
        {
            _event?.Invoke();
        }
    }
}

