using System;
using UnityEngine;
using UnityEngine.Events;

namespace XaviGames.Shared
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "Xavi Games/EventChannel")]
    public class EventChannel : ScriptableObject
    {
        public object Parameter { get; private set; } = null;

        private Action<object> _event;

        public void Subscribe(Action<object> listener)
        {
            if (listener == null)
            {
                Debug.LogError("Trying to subscribe a null listener to EventChannel");
                return;
            }
            
            _event += listener;
        }

        public void Unsubscribe(Action<object> listener)
        {
            if (listener == null)
            {
                Debug.LogError("Trying to unsubscribe a null listener from EventChannel");
                return;
            }
            
            _event -= listener;
        }

        public void RaiseEvent(object parameter = null)
        {
            if (parameter == null)
            {
                parameter = true;
            }

            Parameter = parameter;
            _event?.Invoke(parameter);
        }

        public void RaiseEvent()
        {
            RaiseEvent(null);
        }
    }
}