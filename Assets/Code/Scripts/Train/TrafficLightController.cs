using UnityEngine;
using XaviGames.Attributes;
using XaviGames.Events;

namespace XaviGames.Train
{
    public class TrafficLightController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsRedLight { get; private set; }

        [SerializeField]
        private SingleEventChannel _onTrafficLightStateChange;

        [Button]
        public void ToggleTrafficLightState()
        {
            IsRedLight = !IsRedLight;
            _onTrafficLightStateChange.RaiseEvent(IsRedLight);
        }
    }
}
