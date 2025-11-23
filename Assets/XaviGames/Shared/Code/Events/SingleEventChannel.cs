using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Events
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "XaviGames/SingleEventChannel")]
    public class SingleEventChannel : ScriptableObject
    {
        private UnityAction<object> _event;

        public void Subscribe(UnityAction<object> listener)
        {
            if (listener == null)
            {
                Debug.LogError($"Trying to subscribe a null listener to EventChannel {name}");
                return;
            }

            _event -= listener;
            _event += listener;
        }

        public void Unsubscribe(UnityAction<object> listener)
        {
            if (listener == null)
            {
                Debug.LogError($"Trying to unsubscribe a null listener from EventChannel {name}");
                return;
            }
            
            _event -= listener;
        }

        public void RaiseEvent(object parameter = null)
        {
            _event?.Invoke(parameter);
        }

        public void RaiseEvent()
        {
            RaiseEvent(null);
        }
    }
}