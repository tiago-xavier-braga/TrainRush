using UnityEngine;

namespace XaviGames.Progression
{
    [CreateAssetMenu(fileName = "ProgressionSettings", menuName = "XaviGames/ProgressionSettings")]
    public class ProgressionSettings : ScriptableObject
    {
        [Header("Capacity Price Settings")]
        [SerializeField]
        [Min(0f)]
        private int _baseCapacityPrice = 0;

        [SerializeField]
        [Min(1f)]
        private float _capacityPriceMultiplier = 1f;

        [Header("Train Speed Settings")]
        [SerializeField]
        [Min(0f)]
        private int _baseSpeedPrice = 0;

        [SerializeField]
        [Min(0f)]
        private float _speedPriceMultiplier = 0f;

        [Header("Capacity Settings")]
        [SerializeField]
        [Min(0)]
        private int _baseCapacity = 0;

        [SerializeField]
        [Min(0f)]
        private float _capacityMultiplier = 0f;

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

        private int GetPrice(int tier, int basePrice, float priceMultiplier)
        {
            return Mathf.RoundToInt(basePrice * Mathf.Pow(priceMultiplier, tier));
        }
    }
}
