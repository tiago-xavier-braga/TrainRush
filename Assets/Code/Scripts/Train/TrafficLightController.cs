using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Train
{
    public class TrafficLightController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public bool IsRedLight { get; private set; }

        [Button]
        public void ToggleTrafficLightState()
        {
            IsRedLight = !IsRedLight;
        }
    }
}
