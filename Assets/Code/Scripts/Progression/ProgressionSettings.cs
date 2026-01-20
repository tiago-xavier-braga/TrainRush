using UnityEngine;

namespace XaviGames.Progression
{
    [CreateAssetMenu(fileName = "ProgressionSettings", menuName = "XaviGames/ProgressionSettings")]
    public class ProgressionSettings : ScriptableObject
    {
        [Header("Cost Settings")]
        [SerializeField]
        private int _basePrice;

        [SerializeField]
        private float _priceMultiplier;

        [Header("Capacity Settings")]
        [SerializeField]
        private int _baseCapacity;

        [SerializeField]
        private float _capacityMultiplier;

        public int GetPrice(int tier)
        {
            return Mathf.RoundToInt(_basePrice * Mathf.Pow(_priceMultiplier, tier));
        }

        public int GetCapacity(int tier)
        {
            return _baseCapacity + Mathf.RoundToInt(_capacityMultiplier * tier);
        }
    }
}
