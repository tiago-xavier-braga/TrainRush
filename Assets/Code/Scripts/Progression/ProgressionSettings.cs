using UnityEngine;

namespace XaviGames.Progression
{
    [CreateAssetMenu(fileName = "ProgressionSettings", menuName = "XaviGames/ProgressionSettings")]
    public class ProgressionSettings : ScriptableObject
    {
        [Header("Capacity Price Settings")]
        [SerializeField]
        [Min(0f)]
        private int _baseCapacityPrice;

        [SerializeField]
        [Min(1f)]
        private float _capacityPriceMultiplier;

        [Header("Train Speed Settings")]
        [SerializeField]
        [Min(0f)]
        private int _baseSpeedPrice;

        [SerializeField]
        [Min(0f)]
        private float _speedPriceMultiplier;

        [Header("Capacity Settings")]
        [SerializeField]
        [Min(0)]
        private int _baseCapacity;

        [SerializeField]
        [Min(0f)]
        private float _capacityMultiplier;

        [Header("Wagons Settings")]
        [SerializeField]
        [Min(0)]
        private int _baseWagonPrice;

        [SerializeField]
        [Min(0f)]
        private float _wagonPriceMultiplier;

        public int GetCapacityPrice(int tier)
        {
            return GetPrice(tier, _baseCapacityPrice, _capacityPriceMultiplier);
        }

        public int GetSpeedPrice(int tier)
        {
            return GetPrice(tier, _baseSpeedPrice, _speedPriceMultiplier);
        }

        public int GetCapacity(int tier)
        {
            return _baseCapacity + Mathf.RoundToInt(_capacityMultiplier * tier);
        }

        public int GetWagonPrice(int tier)
        {
            return GetPrice(tier, _baseWagonPrice, _wagonPriceMultiplier);
        }

        private int GetPrice(int tier, int basePrice, float priceMultiplier)
        {
            return Mathf.RoundToInt(basePrice + priceMultiplier * tier);
        }

    }
}
